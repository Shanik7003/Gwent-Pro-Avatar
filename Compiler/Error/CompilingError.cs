public class CompilingError
    {
        public ErrorCode Code { get; private set; }

        public string Message { get; private set; }

        public CodeLocation Location {get; private set;}

        public CompilingError(CodeLocation location, ErrorCode code, string message)
        {
            this.Code = code;
            this.Message = message;
            Location = location;
        }
    }

    public enum ErrorCode
    {
        None,
        Expected,  // Se esperaba un token específico
        Invalid,   // Token inválido o formato inválido
        Unknown,   // Token o símbolo desconocido
        SyntaxError, // Error de sintaxis general
        TypeMismatch, // Error de chequeo de tipos durante el análisis semántico
        UndeclaredVariable, // Uso de una variable no declarada
        FunctionArgumentMismatch, // Mismatch en el número o tipo de argumentos de función
    }