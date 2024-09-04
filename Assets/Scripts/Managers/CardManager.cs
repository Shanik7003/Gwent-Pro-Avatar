using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Engine;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    private static CardManager _instance;
    //Constructor privado para evitar instanciación externa
    private CardManager() 
    {
        cardFactory = new();
    }


    public static CardManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("CardManager").GetComponentInChildren<CardManager>();
            }
            return _instance;
        }
    }
    public int frame = 0;
    public GameObject cardPrefab;     // Referencia al prefab de la carta
    public CardFactory cardFactory;   // Referencia al factory que convierte datos del motor en CardData
    private List<CardData> cardDatas = new(); // Lista para almacenar CardData generados
    private List<CardDisplay> cardDisplays = new();
    private bool P1CardsGenerated = false;
    public Transform player1DeckHolder;
    public Transform player2DeckHolder;
    public Transform player1HandRow;
    public Transform player2HandRow;

    void Start()
    {
        //renderizando las cartas de las Hands
     
        GenerateCardData(Game.GameInstance.Player1.Hand);
        StartCoroutine(GenerateVisualCards(player1HandRow,player1DeckHolder));
        
        GenerateCardData(Game.GameInstance.Player1.Deck);
        StartCoroutine(GenerateVisualCards(player1HandRow,player1DeckHolder));

        GenerateCardData(Game.GameInstance.Player2.Hand);
        StartCoroutine(GenerateVisualCards(player2HandRow,player2DeckHolder));

        GenerateCardData(Game.GameInstance.Player2.Deck);
        StartCoroutine(GenerateVisualCards(player2HandRow,player2DeckHolder));
    }
    void Update()
    {
        frame++;
    }

    public List<CardData> GenerateCardData(List<Card> collection)
    {
        cardDatas = new List<CardData>();
        foreach (var card in collection)
        {
            if (cardFactory == null)
            {
                //Debug.LogError("CardFactory reference is not set in the inspector!");
                return null;
            }
            CardData newCardData = cardFactory.CreateCardData(card);
            cardDatas.Add(newCardData);
        }
        return cardDatas;
    }
    public IEnumerator GenerateVisualCards(Transform handTransform, Transform deckTransform)
    {
        foreach (CardData card in cardDatas)
        {
            GameObject newCard = Instantiate(cardPrefab, deckTransform.position, Quaternion.identity, handTransform);
            CardDisplay display = newCard.GetComponent<CardDisplay>();
            
            if (display != null && card != null )
            {
                display.cardData = card;
                display.FirstUpdateCard();
                if (!display.card.player.Hand.Contains(display.card)) //*!si no esta en al mano no la muestres 
                {
                    newCard.SetActive(false);
                }
                yield return StartCoroutine(MoveCard(newCard.transform, handTransform.position,handTransform));
            }
            else
            {
                Debug.LogError("Componente CardDisplay o datos de carta faltantes.");
            }
        }
    }
    public static void ActivateCard(CardDisplay cardDisplay)
    {
        if (cardDisplay != null && !cardDisplay.gameObject.activeSelf)
        {
            cardDisplay.gameObject.SetActive(true);
        }
    }
    public static void DeActivateCard(CardDisplay cardDisplay)
    {
        if (cardDisplay != null)
        {
            cardDisplay.gameObject.SetActive(false);
        }
    }

    public IEnumerator MoveCard(Transform cardTransform, Vector3 targetPosition,Transform parentTransform)
    {
        float timeToMove = 1.0f; // Duración de la animación en segundos
        float elapsedTime = 0;
        Vector3 startPosition = cardTransform.position;

        while (elapsedTime < timeToMove)
        {
            cardTransform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cardTransform.position = targetPosition;
        LayoutRebuilder.ForceRebuildLayoutImmediate(parentTransform.GetComponent<RectTransform>());
    }
    public IEnumerator AddCardToHandInRight(Transform cardTransform, Transform handTransform, float spacing = 50.0f)
    {
        int cardIndex = handTransform.childCount; // Número de cartas en la mano
        Vector3 targetPosition = handTransform.position + new Vector3(spacing * cardIndex, 0, 0); // Calcula la posición a la derecha
        cardTransform.SetParent(handTransform); // Establece la carta como hija del contenedor de la mano

        // Mueve la carta a la posición calculada
        float timeToMove = 1.0f; // Duración de la animación en segundos
        float elapsedTime = 0;
        Vector3 startPosition = cardTransform.position;

        while (elapsedTime < timeToMove)
        {
            cardTransform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cardTransform.position = targetPosition;
        LayoutRebuilder.ForceRebuildLayoutImmediate(handTransform.GetComponent<RectTransform>());
    }
public IEnumerator AddCardToHandLeft(Transform cardTransform, Transform handTransform, float spacing = 50.0f)
{
    // Desactivar temporalmente el HorizontalLayoutGroup
    HorizontalLayoutGroup layoutGroup = handTransform.GetComponent<HorizontalLayoutGroup>();
    layoutGroup.enabled = false;

    // Mueve todas las cartas actuales a la derecha para hacer espacio
    for (int i = 0; i < handTransform.childCount; i++)
    {
        Transform existingCard = handTransform.GetChild(i);
        existingCard.localPosition += new Vector3(spacing, 0, 0);
    }

    // Añadir la carta al principio de la mano
    cardTransform.SetParent(handTransform, false);
    cardTransform.SetSiblingIndex(0);

    // Mueve la nueva carta a la posición calculada
    Vector3 startPosition = cardTransform.position;
    Vector3 targetPosition = handTransform.position + new Vector3(-spacing * handTransform.childCount, 0, 0);

    float timeToMove = 1.0f; // Duración de la animación en segundos
    float elapsedTime = 0;

    while (elapsedTime < timeToMove)
    {
        cardTransform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / timeToMove));
        elapsedTime += Time.deltaTime;
        yield return null;
    }

    cardTransform.position = targetPosition;

    // Reactivar el HorizontalLayoutGroup
    layoutGroup.enabled = true;
    LayoutRebuilder.ForceRebuildLayoutImmediate(handTransform.GetComponent<RectTransform>());
}





    public static bool CanPlaceCard(Card card, BattleField battleField)
    {
        return card.player == TurnManager.Instance.GetCurrentPlayer() && card.player == battleField.Owner;
    }

    public void EliminateCard(CardDisplay cardDisplay)//este metoo es para ser usado por la habilidad de eliminar la carta mas o menos poderosa, ya que elimina la carta que le indiques del campo del jugador que no este jugando en este turno 
    {
        if (cardDisplay.card.player == Game.GameInstance.Player1)
        {
            // Game.GameInstance.Player1.Field.Remove(cardDisplay.card);
            // Game.GameInstance.Board.Remove(cardDisplay.card);
            StartCoroutine(MoveCardToCemetery(cardDisplay.transform, GameObject.Find("Cemetery1").transform));
            Destroy(cardDisplay.gameObject, 1.0f); // Asumiendo que hay un delay para ver la animación antes de destruir el objeto.
        }
        if ( cardDisplay.card.player == Game.GameInstance.Player2)
        {
            // Game.GameInstance.Player2.Field.Remove(cardDisplay.card);
            // Game.GameInstance.Board.Remove(cardDisplay.card);
            StartCoroutine(MoveCardToCemetery(cardDisplay.transform, GameObject.Find("Cemetery2").transform));
            Destroy(cardDisplay.gameObject, 1.0f); // Asumiendo que hay un delay para ver la animación antes de destruir el objeto.
        }
       
    }
    IEnumerator MoveCardToCemetery(Transform cardTransform, Transform cemeteryTransform)
    {
        float timeToMove = 1.0f; // Duración de la animación en segundos
        float elapsedTime = 0;
        Vector3 startPosition = cardTransform.position;

        while (elapsedTime < timeToMove)
        {
            cardTransform.position = Vector3.Lerp(startPosition, cemeteryTransform.position, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cardTransform.position = cemeteryTransform.position; // Asegúrate de que llegue a la posición exacta del cementerio
    }
}