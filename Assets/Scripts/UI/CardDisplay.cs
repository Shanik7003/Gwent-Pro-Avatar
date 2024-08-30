using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Engine;
using System.Collections.Generic;
using UnityEngine.Animations;

public class CardDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IObserver
{
    public CardData cardData;
    public Card card;
    public Image artworkImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI points;
    public static GameObject detailedCardDisplay; // Referencia al panel de visualización detallada
    public TextMeshProUGUI position;

    void Start()
    {
        //card.AddObserver(this);
        FirstUpdateCard();
        if (detailedCardDisplay == null)
            detailedCardDisplay = GameObject.Find("DetailedCardDisplay"); // Asegúrate de que el nombre coincide
            detailedCardDisplay.SetActive(false);
    }
    private void OnDestroy()
    {
        card.RemoveObserver(this);
    }

    public void UpdateCard()
    {
        points.text = card.points.ToString();
    }

    public void HandleCardMovement(List<Card> newPosition)
    {
        // Implementa la lógica para actualizar la UI con la nueva posición de la carta
        CardManager.ActivateCard(this);
        StartCoroutine(CardManager.Instance.MoveCard(this.transform,VisualBoard.UbicationsMapping[newPosition].position,VisualBoard.UbicationsMapping[newPosition]));

    }
    public void HandleCardElimination()
    {
        CardManager.Instance.EliminateCard(this);
    }
    public void OnNotify(Engine.EventType eventType, object data)
    {
        switch (eventType)
        {
            case Engine.EventType.CardPointsChanged:
                UpdateCard(); // Actualiza la interfaz visual de la carta
                break;

            case Engine.EventType.CardMoved:
                HandleCardMovement((List<Card>) data); // Actualiza la interfaz visual del movimiento de la carta
                break;
            case Engine.EventType.CardRemoved:
                HandleCardElimination();
                break;

        }
    }
    
    public void FirstUpdateCard()
    {
        CardManager.ActivateCard(this);
        nameText.text = cardData.cardName;
        descriptionText.text = cardData.description;
        points.text = cardData.points.ToString();
        artworkImage.sprite = cardData.cardImage;
        position.text = cardData.Card.position.ToString();
        card = cardData.Card;
        card.AddObserver(this);

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

