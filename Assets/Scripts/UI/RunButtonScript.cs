using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RunButtonScript : MonoBehaviour
{
    public Button runButton;
    public TMPro.TMP_InputField codeInputField;

    void Start()
    {
        if (runButton != null)
        {
            runButton.onClick.AddListener(OnRunButtonClick);
        }
    }

    void OnRunButtonClick()
    {
        Debug.Log("Entre a OnRunButtonClick");
        List<CompilingError> LexicalErrors = new List<CompilingError>();
        List<CompilingError> ParsingErrors = new List<CompilingError>();
        List<CompilingError> SemanticErrors = new List<CompilingError>();


        LexicalAnalyzer lexer = Compiling.Lexical;
        string text = codeInputField.text;  // Obtener el texto del InputField

        Debug.Log("text: "+text);

        IEnumerable<Token> tokens = lexer.GetTokens("code", text, LexicalErrors);
        if (HandleErrors(LexicalErrors)) return;

        // Parser
        Parser parser = new Parser(new TokenList(tokens), ParsingErrors);
        RootNode ast = parser.ParseCode();
        if (HandleErrors(ParsingErrors)) return;

        // Realizar el análisis semántico
        SemanticVisitor semanticVisitor = new SemanticVisitor(SemanticErrors);
        semanticVisitor.Visit(ast);
        if (HandleErrors(SemanticErrors)) return;

    }

    static bool HandleErrors(List<CompilingError> errors)
    {
        if (errors.Count > 0)
        {
            foreach (var error in errors)
            {
                Debug.Log($"Error Code: {error.Code} Location: ({error.Location.Line},{error.Location.Column}): {error.Message}");
            }
            return true;
        }
        return false;
    }
}
