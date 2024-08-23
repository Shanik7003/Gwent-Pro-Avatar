using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Net.Http.Headers;
using UnityEngine;
using TMPro;

public class CodeInputManager : MonoBehaviour
{
    public TMP_InputField inputField;
    public TextMeshProUGUI lineNumberText;
    public ScrollRect scrollRect;
    private int currentLine = 1;
    void Start()
    {
        lineNumberText.text = "1";
        inputField.onValueChanged.AddListener(OnInputFieldChanged);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (inputField.isFocused)
            {
                EnterPressed();
            }
        }
        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Delete))
        {
            if (inputField.isFocused)
            {
                BackspacePressed();
            }
        }
    }
    void EnterPressed()
    {
        int caretPosition = inputField.caretPosition;
        int textLength = inputField.text.Length;

        // Solo inserta un salto de línea si no esta ya en un salto de línea
        if (caretPosition < textLength)
        {
            if (inputField.text[caretPosition] != '\n')
            {
                inputField.text = inputField.text.Insert(caretPosition, "\n");
                inputField.caretPosition = caretPosition + 1;
            }
        }
    }
    void BackspacePressed()
    {
        int caretPosition = inputField.caretPosition;

        // COmprueba si se ha eliminado una línea
        if (caretPosition > 0 && inputField.text[caretPosition - 1] == '\n')
        {
            inputField.text = inputField.text.Remove(caretPosition - 1, 1);
            inputField.caretPosition = caretPosition - 1;
        }
    }

    void OnInputFieldChanged(string text)
    {
        int lineCount = text.Split('\n').Length;

        if (lineCount != currentLine)
        {
            // Actualizamos el texto del contador de líneas
            lineNumberText.text = "";
            for (int i = 1; i <= lineCount; i++)
            {
                lineNumberText.text += i + "\n";
            }

            currentLine = lineCount;
        }

    }
}
