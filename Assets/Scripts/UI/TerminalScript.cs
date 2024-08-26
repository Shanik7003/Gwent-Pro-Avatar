using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TerminalScript : MonoBehaviour
{
    public TMP_Text errorReport;  // El TMP_Text que contiene los errores
    public Scrollbar errorScrollbar;  // El Scrollbar para la terminal

    public void DisplayError(string error)
    {
        errorReport.text += error + "\n";  // Añade el error al reporte de errores
        UpdateScrollbar();
    }

    public void ClearTerminal()
    {
        errorReport.text = "";  // Limpia el texto actual de la terminal
    }

    void UpdateScrollbar()
    {
        float contentHeight = errorReport.rectTransform.rect.height;
        float visibleHeight = ((RectTransform)errorReport.transform.parent).rect.height;

        if (contentHeight > visibleHeight)
        {
            errorScrollbar.size = visibleHeight / contentHeight;
            errorScrollbar.value = 1.0f;  // Desplaza el scroll hacia la parte superior
        }
        else
        {
            errorScrollbar.size = 1.0f;
            errorScrollbar.value = 1.0f;
        }
    }
}
