using System.Text;

class Program
{


    public static void GenerateMermaidDiagram(ASTNode rootNode, string filePath)
    {
        var sb = new StringBuilder();
        sb.AppendLine("```mermaid");
        sb.AppendLine("graph TD");
        rootNode.PrintMermaid(sb, "root");
        sb.AppendLine("```");

        File.WriteAllText(filePath, sb.ToString());
        //Console.WriteLine($"Mermaid diagram written to {filePath}");
    }
    static bool HandleErrors(List<CompilingError> errors)
    {
        if (errors.Count > 0)
        {
            foreach (var error in errors)
            {
                Console.WriteLine($"Error Code: {error.Code} Location: ({error.Location.Line},{error.Location.Column}): {error.Message}");
            }
            return true;
        }
        return false;
    }
    static void Main(string[] args)
    {
        List<CompilingError> LexicalErrors = [];
        List<CompilingError> ParsingErrors = [];
        List<CompilingError> SemanticErrors = [];

        LexicalAnalyzer lexer = Compiling.Lexical;
        string text = File.ReadAllText("./code.txt");  
        IEnumerable<Token> tokens = lexer.GetTokens("code", text, LexicalErrors);
        if(HandleErrors(LexicalErrors)) return;
     
        // Parser
        Parser parser = new Parser(new TokenList(tokens),ParsingErrors);
        RootNode ast = parser.ParseCode();
        if(HandleErrors(ParsingErrors)) return;

        // Realizar el análisis semántico
        var semanticVisitor = new SemanticVisitor();
        semanticVisitor.Visit(ast);

        GenerateMermaidDiagram(ast,"effectNodeDiagram.md");

    }
}
