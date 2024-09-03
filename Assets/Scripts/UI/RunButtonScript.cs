using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class RunButtonScript : MonoBehaviour
{
    public Button runButton;
    public TMPro.TMP_InputField codeInputField;
    public TerminalScript terminalScript;  // Referencia al script de la terminal
    public static RootNode ast;

    void Start()
    {
        if (runButton != null)
        {
            runButton.onClick.AddListener(OnRunButtonClick);
        }
    }

    void OnRunButtonClick()
    {
        terminalScript.ClearTerminal();  // Limpia la terminal antes de ejecutar
        //Debug.Log("Entre a OnRunButtonClick");

        List<CompilingError> LexicalErrors = new List<CompilingError>();
        List<CompilingError> ParsingErrors = new List<CompilingError>();
        List<CompilingError> SemanticErrors = new List<CompilingError>();

        LexicalAnalyzer lexer = Compiling.Lexical;
        string text = codeInputField.text;  // Obtener el texto del InputField

        //Debug.Log("text: " + text);

        IEnumerable<Token> tokens = lexer.GetTokens("code", text, LexicalErrors);
        if (HandleErrors(LexicalErrors)) return;

        // Parser
        Parser parser = new Parser(new TokenList(tokens), ParsingErrors);
        ast = parser.ParseCode();
        if (HandleErrors(ParsingErrors)) return;

        // Realizar el análisis semántico
        SemanticVisitor semanticVisitor = new SemanticVisitor(SemanticErrors);
        semanticVisitor.Visit(ast);
        if (HandleErrors(SemanticErrors)) return;

        // //*!este es el codigo que hay que hacerlo despues de que ya se halla hecho el player setup
        // ExecutionVisitor executionVisitor = new(new Dictionary<string, object>());
        // executionVisitor.Visit(ast);
        //*!esta hecho en el GameManagerWrapper
        
        LoadFactionSelectionScene();
    }

    bool HandleErrors(List<CompilingError> errors)
    {
        if (errors.Count > 0)
        {
            foreach (var error in errors)
            {
                terminalScript.DisplayError($"Error Code: {error.Code} Location: ({error.Location.Line},{error.Location.Column}): {error.Message}");
            }
            return true;  // Hay errores, no se procede al cambio de escena
        }
        return false;  // No hay errores, se puede proceder al cambio de escena
    }

    void LoadFactionSelectionScene()
    {
        SceneManager.LoadScene("FactionSelection");
    }
}
