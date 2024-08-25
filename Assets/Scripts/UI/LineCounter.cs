using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
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
        scrollRect.onValueChanged.AddListener(OnScrollChanged); // Sincroniza el scroll
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
            lineNumberText.text = "";
            for (int i = 1; i <= lineCount; i++)
            {
                lineNumberText.text += i + "\n";
            }

            currentLine = lineCount;
        }
    }

    void OnScrollChanged(Vector2 pos)
    {
        // Sincroniza el scroll del inputField con el de los números de línea
        scrollRect.verticalNormalizedPosition = pos.y;
    }
}
