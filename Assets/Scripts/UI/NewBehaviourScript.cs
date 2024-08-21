using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextEditorWithLineNumbers : MonoBehaviour
{
    public TMP_InputField textInputField;
    public RectTransform lineNumberContainer;
    public GameObject lineNumberPrefab;
    public ScrollRect scrollRect;

    private void Start()
    {
        UpdateLineNumbers();
        textInputField.onValueChanged.AddListener(delegate { UpdateLineNumbers(); });
        scrollRect.onValueChanged.AddListener(delegate { SyncScrollPositions(); });
    }

    void UpdateLineNumbers()
    {
        // Limpiar números de línea previos
        foreach (Transform child in lineNumberContainer)
        {
            Destroy(child.gameObject);
        }

        // Contar las líneas en el texto
        string[] lines = textInputField.text.Split('\n');
        
        for (int i = 1; i <= lines.Length; i++)
        {
            // Instanciar un nuevo número de línea
            GameObject lineNumberObj = Instantiate(lineNumberPrefab, lineNumberContainer);
            lineNumberObj.GetComponent<TMP_Text>().text = i.ToString();
        }

        // Ajustar la altura del contenedor de los números de línea
        lineNumberContainer.sizeDelta = new Vector2(lineNumberContainer.sizeDelta.x, textInputField.textComponent.preferredHeight);
    }

    void SyncScrollPositions()
    {
        // Sincronizar la posición vertical del ScrollRect de los números de línea con el del texto
        lineNumberContainer.localPosition = new Vector3(lineNumberContainer.localPosition.x, textInputField.textComponent.rectTransform.localPosition.y, 0);
    }
}
