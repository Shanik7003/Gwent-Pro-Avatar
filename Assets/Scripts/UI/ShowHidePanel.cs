using UnityEngine;
using System.Collections;
using TMPro; // Asegúrate de incluir esto si usas TextMeshPro

public class ShowHidePanel : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI dynamicText; // Referencia al componente de texto
    public float displayTime = 20f; // Tiempo que el panel estará visible

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
        StartCoroutine(HidePanelAfterDelay()); // Inicia la coroutine para ocultar el panel
    }

    private IEnumerator HidePanelAfterDelay()
    {
        yield return new WaitForSeconds(displayTime);
        panel.SetActive(false); // Oculta el panel
    }
}


