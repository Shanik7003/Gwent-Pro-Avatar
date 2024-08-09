using System;
using System.Collections.Generic;
using System.Linq;
public class LexicalAnalyzer
    {
        Dictionary<string, string> operators = new Dictionary<string, string>();
        Dictionary<string, string> keywords = new Dictionary<string, string>();
        Dictionary<string, string> texts = new Dictionary<string, string>();
        Dictionary<string, string> types = new Dictionary<string, string>();

        public IEnumerable<string> Keywords { get { return keywords.Keys; } }

        /* Associates an operator symbol with the correspondent token value */
        public void RegisterOperator(string op, string tokenValue)
        {
            this.operators[op] = tokenValue;
        }

        /* Associates a keyword with the correspondent token value */
        public void RegisterKeyword(string keyword, string tokenValue)
        {
            this.keywords[keyword] = tokenValue;
        }

        /* Associates a Text literal starting delimiter with their correspondent ending delimiter */
        public void RegisterText(string start, string end)
        {
            this.texts[start] = end;
        }
        public void RegisterType(string type, string tokenValue)
        {
            this.types[type] = tokenValue;
        }

        
        /* Matches a new symbol in the code and read it from the string. The new symbol is added to the token list as an operator. */
        private bool MatchSymbol(TokenReader stream, List<Token> tokens)
        {
            foreach (var op in operators.Keys.OrderByDescending(k => k.Length))
                if (stream.Match(op))
                {
                    tokens.Add(new Token(TokenType.Symbol, operators[op], stream.Location));
                    return true;
                }
            return false;
        }

        /* Matches a Text part in the code and read the literal from the stream.
        The tokens list is updated with the new string token and errors is updated with new errors if detected. */
        private bool MatchText (TokenReader stream, List<Token> tokens, List<CompilingError> errors)
        {
            foreach (var start in texts.Keys.OrderByDescending(k=>k.Length))
            {
                string text;
                if (stream.Match(start))
                {
                    if (!stream.ReadUntil(texts[start], out text))
                        errors.Add(new CompilingError(stream.Location, ErrorCode.Expected, texts[start]));
                    tokens.Add(new Token(TokenType.Text, text, stream.Location));
                    return true;
                }
            }
            return false;
        }

        /* Returns all tokens read from the code and populate the errors list with all lexical errors detected. */
        public IEnumerable<Token> GetTokens(string fileName, string code, List<CompilingError> errors)
        {
            List<Token> tokens = new List<Token>();

            TokenReader stream = new TokenReader(fileName, code);

            while (!stream.EOF)
            {
                string value;

                if (stream.ReadWhiteSpace())
                    continue;

                if (stream.ReadID(out value))
                {
                    if (types.ContainsKey(value))
                        tokens.Add(new Token(TokenType.Type, types[value], stream.Location));
                    else if (keywords.ContainsKey(value))
                        tokens.Add(new Token(TokenType.Keyword, keywords[value], stream.Location));
                    else 
                        tokens.Add(new Token(TokenType.Identifier, value, stream.Location));
                    continue;
                }

                if(stream.ReadNumber(out value))
                {
                    double d;
                    if (!double.TryParse(value, out d))
                        errors.Add(new CompilingError(stream.Location, ErrorCode.Invalid, "Number format"));
                    tokens.Add(new Token(TokenType.Number, value, stream.Location));
                    continue;
                }

                if (MatchText(stream, tokens, errors))
                    continue;

                if (MatchSymbol(stream, tokens))
                    continue;

                var unkOp = stream.ReadAny();
                errors.Add(new CompilingError(stream.Location, ErrorCode.Unknown, unkOp.ToString()));
            }

            return tokens;
        }

        /* Allows to read from a string numbers, identifiers and matching some prefix. 
        It has some useful methods to do that */
        class TokenReader
        {
            string FileName;
            string code;
            int pos;
            int line;
            int lastLB;

            public TokenReader(string fileName, string code)
            {
                this.FileName = fileName;
                this.code = code;
                this.pos = 0;
                this.line = 1;
                this.lastLB = -1;
            }

            public CodeLocation Location
            {
                get
                {
                    return new CodeLocation
                    {
                        File = FileName,
                        Line = line,
                        Column = pos - lastLB
                    };
                }
            }

            /* Peek the next character */
            public char Peek()
            {
                if (pos < 0 || pos >= code.Length)
                    throw new InvalidOperationException();
                return code[pos];
            }
            /// <summary>
            /// Devuelve true si la posición actual es mayor o igual a la longitud del código fuente.
            /// </summary>
            /// <value></value>
            public bool EOF
            {
                get { return pos >= code.Length; }
            }
            /// <summary>
            /// Propósito: Comprobar si se ha alcanzado el final de una línea o el final del código.
            /// Funcionamiento: Devuelve true si se ha alcanzado el final del archivo o si el carácter
            /// actual es un salto de línea.
            /// </summary>
            /// <value></value>
            public bool EOL
            {
                get { return EOF || code[pos] == '\n'; }
            }
            /// <summary>
            /// Propósito: Comprobar si el código fuente a partir de la posición actual coincide con 
            /// un prefijo dado.
            /// Funcionamiento: Devuelve true si el código fuente desde la posición actual coincide con el prefijo 
            /// especificado.
            /// </summary>
            /// <param name="prefix"></param>
            /// <returns></returns>
            public bool ContinuesWith(string prefix)
            {
                if (pos + prefix.Length > code.Length)
                    return false;
                for (int i = 0; i < prefix.Length; i++)
                    if (code[pos + i] != prefix[i])
                        return false;
                return true;
            }
            /// <summary>
            /// Propósito: Intentar hacer coincidir un prefijo con el código fuente a partir de la posición actual.
            ///Funcionamiento: Si hay coincidencia, avanza la posición y devuelve true, de lo contrario devuelve false.
            /// </summary>
            /// <param name="prefix"></param>
            /// <returns></returns>
            public bool Match(string prefix)
            {
                if (ContinuesWith(prefix))
                {
                    pos += prefix.Length;
                    return true;
                }

                return false;
            }
            /// <summary>
            /// Propósito: Comprobar si un carácter es válido en un identificador.
            ///Funcionamiento: Devuelve true si el carácter es un guion bajo, una letra 
            /// (al inicio) o una letra o dígito (después del inicio).
            /// </summary>
            /// <param name="c"></param>
            /// <param name="begining"></param>
            /// <returns></returns>
            public bool ValidIdCharacter(char c, bool begining)
            {
                return c == '_' || (begining ? char.IsLetter(c) : char.IsLetterOrDigit(c));
            }
            /// <summary>
            /// Propósito: Leer un identificador del código fuente.
            ///Funcionamiento: Lee caracteres válidos para un identificador y devuelve true si se ha leído un identificador, false en caso contrario.
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public bool ReadID(out string id)
            {
                id = "";
                while (!EOL && ValidIdCharacter(Peek(), id.Length == 0))
                    id += ReadAny();
                return id.Length > 0;
            }
            /// <summary>
            /// Propósito: Leer un número del código fuente.
            /// Funcionamiento: Lee caracteres numéricos, incluyendo la parte decimal si existe,
            ///  y devuelve true si se ha leído un número, false en caso contrario.
            /// </summary>
            /// <param name="number"></param>
            /// <returns></returns>
            public bool ReadNumber(out string number)
            {
                number = "";
                while (!EOL && char.IsDigit(Peek()))
                    number += ReadAny();
                if (number.Length > 0 && !EOL && Match("."))
                {
                    // read decimal part
                    number += '.';
                    while (!EOL && char.IsDigit(Peek()))
                        number += ReadAny();
                }

                if (number.Length == 0)
                    return false;

                // Load Number posfix, i.e., 34.0F
                // Not supported exponential formats: 1.3E+4
                while (!EOL && char.IsLetterOrDigit(Peek()))
                    number += ReadAny();

                return number.Length > 0;
            }
            /// <summary>
            /// Propósito: Leer el código fuente hasta encontrar un delimitador final.
            /// Funcionamiento: Lee caracteres hasta que se encuentre el delimitador final especificado
            ///  y devuelve true si lo encuentra, false en caso contrario.
            /// </summary>
            /// <param name="end"></param>
            /// <param name="text"></param>
            /// <returns></returns>
            public bool ReadUntil(string end, out string text)
            {
                text = "";
                while (!Match(end))
                {
                    if (EOL || EOF)
                        return false;
                    text += ReadAny();
                }
                return true;
            }
            /// <summary>
            /// Propósito: Leer y omitir espacios en blanco del código fuente.
            ///Funcionamiento: Avanza la posición en el código fuente si el carácter actual 
            /// es un espacio en blanco y devuelve true, false en caso contrario.
            /// </summary>
            /// <returns></returns>
            public bool ReadWhiteSpace()
            {
                if (char.IsWhiteSpace(Peek()))
                {
                    ReadAny();
                    return true;
                }
                return false;
            }
            /// <summary>
            /// Propósito: Leer el siguiente carácter del código fuente y avanzar la posición.
            ///Funcionamiento: Lee y devuelve el carácter actual, avanza la posición, y actualiza la 
            /// línea y la columna si es necesario.
            /// </summary>
            /// <returns></returns>
            public char ReadAny()
            {
                if (EOF)
                    throw new InvalidOperationException();

                if (EOL)
                {
                    line++;
                    lastLB = pos;
                }
                return code[pos++];
            }
        }

    }


