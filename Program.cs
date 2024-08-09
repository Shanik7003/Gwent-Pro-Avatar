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
    static void Main(string[] args)
    {
        LexicalAnalyzer lex = Compiling.Lexical;

        string text = File.ReadAllText("./code.txt");  

        /* You can uncomment fragments of the following lines to test the game step by step*/

        IEnumerable<Token> tokens = lex.GetTokens("code", text, new List<CompilingError>());
         
        Console.ForegroundColor = ConsoleColor.Red; 
        System.Console.WriteLine("LEXER");
        Console.ResetColor();

        foreach (Token token in tokens)
        {
            Console.WriteLine(token);
        }
        // Ejemplo de tokens
        TokenList Tokens = new TokenList(tokens); // Asumiendo que TokenList es tu clase que contiene los tokens ya procesados

        // Parser
        Parser parser = new Parser(Tokens);

        // Parsear el efecto
        ASTNode ast = parser.ParseCode();

        Console.ForegroundColor = ConsoleColor.Cyan; 
        System.Console.WriteLine("Parser");
        Console.ResetColor();
        // Imprimir el AST
        // parser.PrintAST(ast);
        //parser.SaveASTAsMermaid(ast, "ast_graph.md");
        GenerateMermaidDiagram(ast,"effectNodeDiagram.md");
    }
}
