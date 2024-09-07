using UnityEngine;
using TMPro; // Asegúrate de incluir el espacio de nombres de TextMeshPro
using UnityEngine.UI;

public class ErrorManager : MonoBehaviour
{
    public static ErrorManager Instance;

    public GameObject errorPanel;
    public TextMeshProUGUI errorMessage; // Cambiado a TextMeshProUGUI
    public Button closeButton; // Cambiado a TMP_Button

    void Awake()
    {    
        // Implementar el patrón Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Opcional, si deseas que persista entre escenas
        }
        else
        {
            Destroy(gameObject);
        }

        // Asegurarse de que el panel esté desactivado al iniciar
        errorPanel.SetActive(false);

        // Asignar la función ClosePanel al botón de cerrar
        closeButton.onClick.AddListener(ClosePanel);
    }

    public void ShowError(string message)
    {
        errorPanel.SetActive(true);
        errorMessage.text = message;
        errorPanel.SetActive(true);
        Time.timeScale = 0; // Pausar el juego si es necesario
    }

    public void ClosePanel()
    {
        errorPanel.SetActive(false);
        Time.timeScale = 1; // Reanudar el juego
    }
}
