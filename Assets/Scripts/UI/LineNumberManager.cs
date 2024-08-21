using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LineNumberSynchronizer : MonoBehaviour
{
    public TMP_InputField textInputField;
    public TMP_Text lineNumberText;
    public ScrollRect textScrollRect;
    public ScrollRect lineNumberScrollRect;

    private void Start()
    {
        UpdateLineNumbers();
        textInputField.onValueChanged.AddListener(delegate { UpdateLineNumbers(); });
        textScrollRect.onValueChanged.AddListener(delegate { SyncScrollPositions(); });
    }

    void UpdateLineNumbers()
    {
        // Contar las líneas en el texto
        string[] lines = textInputField.text.Split('\n');
        string lineNumbers = "";

        for (int i = 1; i <= lines.Length; i++)
        {
            lineNumbers += i + "\n";
        }

        // Asignar los números de línea al TMP_Text
        lineNumberText.text = lineNumbers;

        // // Ajustar la altura del RectTransform de los números de línea
        // float textHeight = textInputField.textComponent.preferredHeight;
        // RectTransform lineNumberRect = lineNumberText.GetComponent<RectTransform>();
        // lineNumberRect.sizeDelta = new Vector2(lineNumberRect.sizeDelta.x, textHeight);

        // Sincronizar las posiciones de scroll
        SyncScrollPositions();
    }

    void SyncScrollPositions()
    {
        // Sincronizar la posición vertical del ScrollRect de los números de línea con el del texto
        lineNumberScrollRect.verticalNormalizedPosition = textScrollRect.verticalNormalizedPosition;
    }
}
