using UnityEngine;
using System.Collections;
using TMPro; // Asegúrate de incluir esto si usas TextMeshPro

public class ShowPanel : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI dynamicText; // Referencia al componente de texto
    private void Awake()
    {
        panel.SetActive(false); // Asegurarse de que el panel esté oculto al inicio
    }

    // Método público para mostrar el panel con un texto específico
    public void ShowPanelWithMessage(string message)
    {
        // Debug.Log("Attempting to show panel with message: " + message);
        dynamicText.text = message; // Establece el texto
        panel.SetActive(true); // Muestra el panel
    }
}


