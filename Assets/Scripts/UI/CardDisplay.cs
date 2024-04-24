using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CardDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CardData cardData;
    public Image artworkImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI points;
    public static GameObject detailedCardDisplay; // Referencia al panel de visualización detallada

    void Start()
    {
        UpdateCard();
        if (detailedCardDisplay == null)
            detailedCardDisplay = GameObject.Find("DetailedCardDisplay"); // Asegúrate de que el nombre coincide
            detailedCardDisplay.SetActive(false);
    }

    public void UpdateCard()
    {
        Debug.Log("IMPRIMIENDO CARdDATA ... => " + cardData);
        nameText.text = cardData.cardName;
        descriptionText.text = cardData.description;
        points.text = cardData.points.ToString();
        artworkImage.sprite = cardData.cardImage;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
                       
        detailedCardDisplay.SetActive(true); // Muestra el panel
        UpdateDetailedDisplay(); // Actualiza los datos en el panel detallado
    }

    public void OnPointerExit(PointerEventData eventData)
    {
                      
        detailedCardDisplay.SetActive(false); // Oculta el panel
    }
    private void UpdateDetailedDisplay()
    {
        DetailedCardDisplay display = detailedCardDisplay.GetComponent<DetailedCardDisplay>();
        if (display != null)
        {
            display.UpdateDisplay(cardData);
        }
    }
}

