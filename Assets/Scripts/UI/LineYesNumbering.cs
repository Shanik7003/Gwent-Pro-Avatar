using UnityEngine;
using TMPro;

public class LineNumbering : MonoBehaviour
{
    public TMP_InputField inputField;
    public TextMeshProUGUI lineNumberText;

    void Start()
    {
        UpdateLineNumbers();
        inputField.onValueChanged.AddListener(delegate { UpdateLineNumbers(); });
    }

    void UpdateLineNumbers()
    {
        string[] lines = inputField.text.Split('\n');
        lineNumberText.text = "";

        for (int i = 1; i <= lines.Length; i++)
        {
            lineNumberText.text += i + "\n";
        }
    }
}
