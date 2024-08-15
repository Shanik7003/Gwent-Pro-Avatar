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
        Console.WriteLine($"Mermaid diagram written to {filePath}");
    }
    static bool HandleErrors(List<CompilingError> errors)
    {
        if (errors.Count > 0)
        {
            foreach (var error in errors)
            {
                Console.WriteLine(error);
            }
            return true;
        }
        return false;
    }
    static void Main(string[] args)
    {
        List<CompilingError> LexicalErrors = new List<CompilingError>();
        List<CompilingError> ParsingErrors = new List<CompilingError>();
        List<CompilingError> SemanticErrors = new List<CompilingError>();

        LexicalAnalyzer lexer = Compiling.Lexical;
        string text = File.ReadAllText("./code.txt");  

        IEnumerable<Token> tokens = lexer.GetTokens("code", text, LexicalErrors);
        if(HandleErrors(LexicalErrors)) return;
     
        TokenList Tokens = new TokenList(tokens);

        // Parser
        Parser parser = new Parser(Tokens,ParsingErrors);

        // Parsear el efecto
        RootNode ast = parser.ParseCode();

        // Realizar el análisis semántico
        var semanticVisitor = new SemanticVisitor();
        semanticVisitor.Visit(ast);


        GenerateMermaidDiagram(ast,"effectNodeDiagram.md");
    }
}
