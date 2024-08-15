using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Xml;
class Parser
{
    public TokenList Tokens { get; private set; }
    private List<CompilingError> parsingErrors;
    public Parser(TokenList tokens,List<CompilingError> errors)
    {
        Tokens = tokens;
        parsingErrors = errors;
    }
    // Método para agregar errores
    private void AddParsingError(CodeLocation location, ErrorCode code, string message)
    {
        parsingErrors.Add(new CompilingError(location, code, message));
    }
    public RootNode ParseCode()
    {
        RootNode root = new RootNode();
        while (!Tokens.End)
        {
            if (Tokens.MatchValue("effect"))
            {
                EffectNode effect = ParseEffect();
                root.Effects.Add(effect);
            }
            if (Tokens.MatchValue("card")) 
            {
                CardNode card = ParseCard();
                root.Cards.Add(card);
            }
        }

        return root;
    }

    public CardNode ParseCard()
    {
        Tokens.Expect("{");
        CardType type = ParseType();
        Tokens.Expect(",");
        string name  = ParseNameField();
        Tokens.Expect(",");
        Faction faction = ParseFaction();
        Tokens.Expect(",");
        int power = ParsePower();
        Tokens.Expect(",");
        Position[] range = ParseRange();
        Tokens.Expect(",");
        List<EffectInvocationNode> effects = ParseOnActivationBody();
        Tokens.Expect("}");
        CardNode cardNode = new CardNode(name,type,faction,power,range,effects);  
        return cardNode;
    }
    public CardType ParseType()
    {
        // HashSet de tipos válidos derivados del enumerado
        HashSet<string> ValidCardTypes = Enum.GetNames(typeof(CardType)).ToHashSet();
        Tokens.Expect("Type");
        Tokens.Expect(":");
        var type = Tokens.Expect(TokenType.Text).Value;
        if (ValidCardTypes.Contains(type))
        {
            return Enum.Parse<CardType>(type);
        }
        else
        {
            throw new ArgumentException($"Invalid card type: {type}");
        }
    }
    public Faction ParseFaction()
    {
        // HashSet de tipos válidos derivados del enumerado
        HashSet<string> ValidFactions = Enum.GetNames(typeof(Faction)).ToHashSet();
        Tokens.Expect("Faction");
        Tokens.Expect(":");
        var faction = Tokens.Expect(TokenType.Text).Value;
        if (ValidFactions.Contains(faction))
        {
            return Enum.Parse<Faction>(faction);
        }
        else
        {
            throw new ArgumentException($"Invalid faction : {faction}");
        }
    }
    public int ParsePower()
    {
        Tokens.Expect("Power");
        Tokens.Expect(":");
        var power = Tokens.Expect(TokenType.Number).Value;
        int result;
        if (int.TryParse(power,out result))
        {
            return result;
        }
        else
        {
            throw new ArgumentException($"Expected type int but found: {power.GetType()}");
        }
    }
    public Position[] ParseRange()
    {
         // HashSet de tipos válidos derivados del enumerado
        HashSet<string> ValidPositions = Enum.GetNames(typeof(Position)).ToHashSet();
        Tokens.Expect("Range");
        Tokens.Expect(":");
        Tokens.Expect("[");
        List<Position> Range = [];
        List<string> positions = [];
        while(Tokens.LookAhead().Value != TokenValues.ClosedSquareBraket)
        {

            if (Tokens.LookAhead().Value == ",")
            {
                Tokens.Next();//consume la coma
                positions.Add(Tokens.Expect(TokenType.Text).Value);
            }
            else
            {
                positions.Add(Tokens.Expect(TokenType.Text).Value);
            }
        }
        Tokens.Next();//consume el ]
        foreach (var pos in positions)
        {
            if (ValidPositions.Contains(pos))
            {
                Range.Add(Enum.Parse<Position>(pos));
            }
            else
            {
                throw new ArgumentException($"Invalid Position : {pos}");
            }
        }
        return Range.ToArray();
    }
    public List<EffectInvocationNode> ParseOnActivationBody()
    {
        List<EffectInvocationNode> effects = [];
        Tokens.Expect("OnActivation");
        Tokens.Expect(":");
        Tokens.Expect("[");
        if (Tokens.LookAhead().Value == "{")
        {
            effects.Add(ParseEffectInvocation());
            while (Tokens.LookAhead().Value == ",")
            {
                Tokens.Expect(",");
                effects.Add(ParseEffectInvocation());
            }
            Tokens.Expect("]");
        }
        return effects;
    }
    public EffectInvocationNode ParseEffectInvocation()
    {
        SelectorNode? selector = null;
        EffectInvocationNode? postAction = null;
        Tokens.Expect("{");
        EffectField effectfield = ParseEffectField();
        while (Tokens.LookAhead().Value != "}")
        {
            selector = ParseSelectorField();
            postAction = ParsePostAction();
        }
        Tokens.Expect("}");
        EffectInvocationNode effect = new EffectInvocationNode(effectfield,selector,postAction);
        return effect;
    }
    public EffectField ParseEffectField()
    {
            Tokens.Expect("Effect");
            Tokens.Expect(":");
            if (Tokens.LookAhead().Type == TokenType.Text)
            {
                string Name = Tokens.LookAhead().Value;
                Tokens.Next();
                return new EffectField(Name);
            }
            Tokens.Expect("{");
            string name = ParseNameField();
            List<CardParam> cardParams = [];
            while (Tokens.LookAhead().Value != "}")
            {
                Tokens.Expect(",");
                cardParams.Add(ParseCardParam());
            }
            Tokens.Expect("}");
            EffectField effectField = new EffectField(name,cardParams);
            return effectField;
    }
    public CardParam ParseCardParam()
    {
        string name = Tokens.Expect(TokenType.Identifier).Value;
        Tokens.Expect(":");
        string value = Tokens.LookAhead().Value;
        Tokens.Next();//consume el value
        return new CardParam(name, value);
    }
    public SelectorNode? ParseSelectorField()
    {
        if (Tokens.LookAhead().Value ==",") Tokens.Next();
        if (Tokens.LookAhead().Value == "Selector")
        {
            Tokens.Expect("Selector");
            Tokens.Expect(":");
            Tokens.Expect("{");
            Source source = ParseSource();
            bool single = ParseSingle();
            MyPredicate predicate  = ParsePredicate();
            Tokens.Expect("}");
            return new SelectorNode(source,single,predicate);
        }
        return null;
    }
    public Source ParseSource()
    {
        // HashSet de tipos válidos derivados del enumerado
        HashSet<string> ValidSources = Enum.GetNames(typeof(Source)).ToHashSet();
        Tokens.Expect("Source");
        Tokens.Expect(":");
        string source = Tokens.Expect(TokenType.Text).Value;
        if (ValidSources.Contains(source))
        {
            return Enum.Parse<Source>(source);
        }
        else
        {
            throw new ArgumentException($"Invalid source : {source}");
        }
    }
    public bool ParseSingle()
    {
        if (Tokens.LookAhead().Value == ",") Tokens.Next();
        if (Tokens.LookAhead().Value == "Single")
        {
            Tokens.Expect("Single");
            Tokens.Expect(":");
            string single = Tokens.Expect(TokenType.Keyword).Value;
            if (single == "true" || single == "false") return single=="true";
            else throw new ArgumentException($"Invalid single : {single}");
        }
        else return false;

    }
    public MyPredicate ParsePredicate()
    {
        if (Tokens.LookAhead().Value == ",") Tokens.Next();
        Tokens.Expect("Predicate");
        Tokens.Expect(":");
        Tokens.Expect("(");
        string param = Tokens.Expect(TokenType.Identifier).Value;
        Tokens.Expect(")");
        Tokens.Expect("=>");
        var condition = ParseExpression();
        return new MyPredicate(param,condition);
    }
    public EffectInvocationNode? ParsePostAction()
    {
        if (Tokens.LookAhead().Value ==",") Tokens.Next();
        if (Tokens.LookAhead().Value == "PostAction")
        {
            Tokens.Expect("PostAction");
            Tokens.Expect(":");
            EffectInvocationNode postAction = ParseEffectInvocation();
            return postAction;
        }
        return null;
    }

//parsear effecto
    public EffectNode ParseEffect()
    {
        Tokens.Expect("{");
        var effectBody = ParseEffectBody();
        Tokens.Expect("}");
        return effectBody;
    }

    public EffectNode ParseEffectBody()
    {
        var name = ParseNameField();
        Tokens.Expect(",");
        List<ParamNode> parameters = new List<ParamNode>();
        if (Tokens.LookAhead().Value == "Params")
        {
            parameters = ParseParamsField();
        }

        var action = ParseActionField();
        return new EffectNode(name, parameters, action);
    }

    public string ParseNameField()
    {
        Tokens.Expect("Name");
        Tokens.Expect(":");
        var name = Tokens.Expect(TokenType.Text).Value;
        return name;
    }

    public List<ParamNode> ParseParamsField()
    {
        Tokens.Expect("Params");
        Tokens.Expect(":");
        Tokens.Expect("{");

        var paramList = new List<ParamNode>();
        while (Tokens.LookAhead().Value != TokenValues.ClosedCurlyBraces)
        {
            var param = ParseParam();
            paramList.Add(param);
            if (Tokens.LookAhead().Type == TokenType.Comma)
            {
                Tokens.Next(); // consume ','
            }
        }
        //System.Console.WriteLine("SALI DEL WHILE");
        Tokens.Expect("}");
        Tokens.Expect(",");
        return paramList;
    }
    public ParamNode ParseParam()
    {
        // Console.WriteLine("ParseParam - Current Token: " + Tokens.LookAhead().Value + " (Expected Identifier for param name)");
        var name = Tokens.Expect(TokenType.Identifier).Value;
        // Console.WriteLine("ParseParam - Current Token: " + Tokens.LookAhead().Value + " (Expected ':' after param name)");
        Tokens.Expect(":");
        // Console.WriteLine("ParseParam - Current Token: " + Tokens.LookAhead().Value + " (Expected Identifier for param type)");
        var type = Tokens.Expect(TokenType.Type).Value;
        return new ParamNode(name, type);
    }


    public ActionNode ParseActionField()
    {
        Tokens.Expect("Action");
        Tokens.Expect(":");
        return ParseFunction();
    }

    public ActionNode ParseFunction()
    {
        Tokens.Expect("(");

        IdentifierNode targets = new(Tokens.Expect(TokenType.Identifier).Value,true);
        Tokens.Expect(",");
        IdentifierNode context = new(Tokens.Expect(TokenType.Identifier).Value,true,true);//porque ademas de ser dinamico es de tipo context
        Tokens.Expect(")");
        Tokens.Expect("=>");
        Tokens.Expect("{");
    
        var statements = new List<StatementNode>();
        while (Tokens.LookAhead().Value != "}")
        {
            statements.Add(ParseStatement());
        }

        Tokens.Expect("}");
        return new ActionNode(targets,context,statements);
    }
    
    public StatementNode ParseStatement()
    {
        // Maneja la declaración 'for'
        if (Tokens.LookAhead().Value == "for")
        {
            return ParseForStatement();
        }
        // Maneja la declaración 'while'
        else if (Tokens.LookAhead().Value == "while")
        {
            return ParseWhileStatement();
        }
        // Maneja la declaración de asignación
        else if (Tokens.LookAhead().Type == TokenType.Identifier)
        {
            return ParseAssignment();
        }
        else
        {
            throw new Exception($"Unexpected token: {Tokens.LookAhead().Value}");
        }
    }

    public ForStatement ParseForStatement()
    {
        Tokens.Expect("for");
        var variable = new IdentifierNode(Tokens.Expect(TokenType.Identifier).Value,true);//porque la variable del for es un identifier dinamico
        Tokens.Expect("in");
        IdentifierNode iterable =  new IdentifierNode (Tokens.Expect(TokenType.Identifier).Value);

        if (Tokens.LookAhead().Value != "{")// si no le pusieron llaves entonces parsea un statement solamente 
        {
            var body = new List<ASTNode>();
            body.Add(ParseStatement());
            return new ForStatement(variable,iterable, body);
        }
        else
        {
            Tokens.Expect("{");
            var body = new List<ASTNode>();
            while (Tokens.LookAhead().Value != "}")
            {
                body.Add(ParseStatement());
            }
            Tokens.Expect("}");
            Tokens.Expect(";");
            return new ForStatement(variable, iterable, body);
        }
      
    }


    public WhileStatement ParseWhileStatement()
    {
        Tokens.Expect("while");
        Tokens.Expect("(");
        var condition = ParseExpression();
        Tokens.Expect(")");
        if (Tokens.LookAhead().Value != "{")// si no le pusieron llaves entonces parsea un statement solamente 
        {
            var body = new List<ASTNode>();
            body.Add(ParseStatement());
            return new WhileStatement(condition, body);
        }
        else
        {
            Tokens.Expect("{");
            var body = new List<ASTNode>();
            while (Tokens.LookAhead().Type != TokenType.ClosedCurlyBrace)
            {
                body.Add(ParseStatement());
            }
            Tokens.Expect("}");
            return new WhileStatement(condition, body);
        }
  
    }

    public AssignmentOrMethodCall ParseAssignment()
    {
        ExpressionNode variable = ParseExpression();
        var op = Tokens.LookAhead().Value;
        if (op == "+=" || op== "-="|| op == "*=" || op == "/=")
        {
            Tokens.Next(); //consumelo 
            var value = ParseExpression();
            Tokens.Expect(";");
            return new CompoundAssignmentNode(variable, op,value);
        }
        else if(Tokens.LookAhead().Value == "(")//es una llamada a metodo como un statement, en una sola linea, no eat dentro de un assignment
        {
            return ParseMethodCall(variable as PropertyAccessNode);
        }
        else
        {
            Tokens.Expect("=");
            var value = ParseExpression();
            if (Tokens.LookAhead().Value == "(")//es una llamada a metodo dentro de un assingment
            {
                return new Assignment(variable, ParseExpressionMethodCall(value as PropertyAccessNode));
            }
            else
            {
                Tokens.Expect(";");
                return new Assignment(variable, value);
            } 
        }
    }
    public MethodCallNode ParseMethodCall(PropertyAccessNode call)
    {
        Tokens.Next();//consume (
        if (Tokens.CanLookAhead() && Tokens.LookAhead().Type == TokenType.Identifier)//tiene parametro 
        {
        var param = Tokens.Expect(TokenType.Identifier);
        Tokens.Expect(")"); // Espera y consume ')'
        Tokens.Expect(";");
        return new MethodCallNode(call.Property.Name,param.Value,call.Target);
        }
        //no tiene parametro 
        Tokens.Expect(")"); // Espera y consume ')'
        Tokens.Expect(";");
        var mcall = new MethodCallNode(call.Property.Name, call.Target);
        return mcall;
    }
    public ExpressionMethodCall ParseExpressionMethodCall(PropertyAccessNode call)
    {
        Tokens.Next();//consume (
        if (Tokens.CanLookAhead() && Tokens.LookAhead().Type == TokenType.Identifier)//tiene parametro 
        {
        var param = Tokens.Expect(TokenType.Identifier);
        Tokens.Expect(")"); // Espera y consume ')'
        Tokens.Expect(";");
        return new ExpressionMethodCall(call.Property.Name,param.Value,call.Target);
        }
        //no tiene parametro 
        Tokens.Expect(")"); // Espera y consume ')'
        Tokens.Expect(";");
        var mcall = new ExpressionMethodCall(call.Property.Name, call.Target);
        return mcall;
    }

    #region ParseEspression
    public ExpressionNode ParseExpression()
    {
        return ParseLogicalTerm();
    }

    public ExpressionNode ParseLogicalTerm()
    {
        var node = ParseLogicalFactor();
        bool flag = false;
        while (Tokens.CanLookAhead() && Tokens.LookAhead().Value == "||")
        {
            flag = true;
            var op = Tokens.LookAhead().Value;
            Tokens.Next(); // Consume el operador

            var right = ParseLogicalFactor();
            node = new BinaryOperation(node, op, right);
        }
        return node;
    }

    public ExpressionNode ParseLogicalFactor()
    {
        var node = ParseEqualityExpr();
        bool flag = false;
        while (Tokens.CanLookAhead() && Tokens.LookAhead().Value == "&&")
        {
            flag = true;
            var op = Tokens.LookAhead().Value;
            Tokens.Next(); // Consume el operador

            var right = ParseEqualityExpr();
            node = new BinaryOperation(node, op, right);
        }

        return node;
    }

    public ExpressionNode ParseEqualityExpr()
    {
        var node = ParseRelationalExpr();
        bool flag = false;

        while (Tokens.CanLookAhead() && (Tokens.LookAhead().Value == "==" || Tokens.LookAhead().Value == "!="))
        {
            flag = true;
            var op = Tokens.LookAhead().Value;
            Tokens.Next(); // Consume el operador

            var right = ParseRelationalExpr();
            node = new BinaryOperation(node, op, right);
        }
        return node;
    }

    public ExpressionNode ParseRelationalExpr()
    {
        var node = ParseAdditiveExpr();
        bool flag = false;
        while (Tokens.CanLookAhead() && (Tokens.LookAhead().Value == "<" || Tokens.LookAhead().Value == ">" || Tokens.LookAhead().Value == "<=" || Tokens.LookAhead().Value == ">="))
        {
            flag = true;
            var op = Tokens.LookAhead().Value;
            Tokens.Next(); // Consume el operador

            var right = ParseAdditiveExpr();
            node = new BinaryOperation(node, op, right);
        }

        return node;
    }

    public ExpressionNode ParseAdditiveExpr()
    {
        var node = ParseMultiplicativeExpr();
        bool flag = false;
        while (Tokens.CanLookAhead() && (Tokens.LookAhead().Value == "+" || Tokens.LookAhead().Value == "-"))
        {
            flag = true;
            var op = Tokens.LookAhead().Value;
            Tokens.Next(); // Consume el operador

            var right = ParseMultiplicativeExpr();
            node = new BinaryOperation(node, op, right);
        }

        return node;
    }

    public ExpressionNode ParseMultiplicativeExpr()
    {
        var node = ParsePrimary();
        bool flag = false;
        while (Tokens.CanLookAhead() && (Tokens.LookAhead().Value == "*" || Tokens.LookAhead().Value == "/"))
        {
            flag = true;
        //  System.Console.WriteLine("NO ENCONTRE UN SIGNO DE MULTIPLICACION O DIVISION ");
            var op = Tokens.LookAhead().Value;
            Tokens.Next(); // Consume el operador

            var right = ParsePrimary();
            node = new BinaryOperation(node, op, right);
        }
        
        return node;
    }

    public ExpressionNode ParsePrimary()
    {
        var token = Tokens.LookAhead();

        if (token.Type == TokenType.Number)
        {
            Tokens.Next(); // Consume el número

            return new Number(double.Parse(token.Value));
        }

        if (token.Type == TokenType.Identifier || token.Type == TokenType.Text)
        {
            ExpressionNode target = new IdentifierNode(Tokens.LookAhead().Value); // guardalo para los targets 
            Tokens.Next(); // Consume el identificador o el texto

            while (Tokens.CanLookAhead() && Tokens.LookAhead().Value == ".")
            {
                Tokens.Next();//Consume . 
                string property = Tokens.LookAhead().Value;//porque puede ser Idenrifier o Keyword ejemplo Faction
                Tokens.Next();
                var exp = new PropertyAccessNode(property,target);
    
                while (Tokens.CanLookAhead() && Tokens.LookAhead().Value == ".")
                {
                    Tokens.Next();//Consume . 
                    target = new PropertyAccessNode(exp.Property.Name,target);
                    // target = new IdentifierNode(target.Name + "." + exp.Property.Name);//concatenando el nuevo identifier despues del punto 
                    exp = new PropertyAccessNode(Tokens.LookAhead().Value,target);
                    Tokens.Next();//consume el indentificador 
                    break;
                }
                //dejo de haber un identifier, ahora puede venir un "(" o ";" o "operator"
                if (Tokens.LookAhead().Value == ";" || Tokens.LookAhead().Value == "(" || Tokens.LookAhead().Type == TokenType.Operator)
                {
                    return exp;
                }
            }
            return new IdentifierNode(token.Value);
        }

        if (token.Value == "(")
        {
            Tokens.Next(); // Consume '('
    
            var expression = ParseExpression();
            Tokens.Expect(")"); // Espera y consume ')'
    
            return expression;
        }
        throw new Exception("Unexpected token: " + token.Value);
    }
    #endregion
}
