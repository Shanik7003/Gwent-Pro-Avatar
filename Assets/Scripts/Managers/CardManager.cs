using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Engine;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    private static CardManager _instance;
    //Constructor privado para evitar instanciación externa
    private CardManager() 
    {
        cardFactory =new();
    }

    // // Propiedad pública para acceder a la instancia
    public static CardManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("CardManager").GetComponentInChildren<CardManager>();
                Debug.Log("imprimirndo _instance.cardFactory LA DEFINITIVA =====> "+_instance.cardFactory);
            }
            return _instance;
        }
    }
    public int frame = 0;
    public GameObject cardPrefab;     // Referencia al prefab de la carta
    public CardFactory cardFactory;   // Referencia al factory que convierte datos del motor en CardData
    private List<CardData> cardDatas = new(); // Lista para almacenar CardData generados
    private bool P1CardsGenerated = false;
    public Transform player1DeckHolder;
    public Transform player2DeckHolder;
    public Transform player1HandRow;
    public Transform player2HandRow;

    void Start()
    {
        // List<Card> EngineCards1 = GetCardsFromEngine(); // guarda las cartas del deck del jugador1
        // GenerateCardData(EngineCards1);//convierte las cards en cardData
        // //GenerateCards(player1DeckHolder); // no tengo que instanciar el Deck completo 
        // List<Card> EngineCards2 = GetCardsFromEngine(); // guarda las cartas del deck del jugador2
        // GenerateCardData(EngineCards2);//convierte las cards en cardData
        // //GenerateCards(player2DeckHolder); // no tengo que instanciar el deck completo

        //renderizando las cartas de las Hands
        GenerateCardData(Game.GameInstance.Player1.Hand);
        StartCoroutine(GenerateCards(player1HandRow,player1DeckHolder));
        GenerateCardData(Game.GameInstance.Player2.Hand);
        StartCoroutine(GenerateCards(player2HandRow,player2DeckHolder));
        

    }
    void Update()
    {
        frame++;
    }
    public void InstanciateCard(Card card)//metodo para instanciar una sola carta, tengo que hacer uno que instancie n cartas 
    {
        List<Card> SingleCard = new(){card};
        GenerateCardData(SingleCard);
        if (card.player == Game.GameInstance.Player1)
        {
            Debug.Log("entre a InstanciateCard");
            StartCoroutine(GenerateCards(player1HandRow,player1DeckHolder));
        }
        if (card.player == Game.GameInstance.Player2)
        {
            StartCoroutine(GenerateCards(player2HandRow,player2DeckHolder));
        }
    }


    

    public  void GenerateCardData(List<Card> collection)
    {
        Debug.Log(frame);
        cardDatas = new List<CardData>();
        foreach (var card in collection)
        {
             Debug.Log("estoy generando CardDatas");
            // Asigna una imagen apropiada aquí si es necesario
            //Sprite cardImage = Resources.Load<Sprite>("path/to/sprite/" + card.name);
            Debug.Log("Imprimiendo this.............." + cardFactory);

            if (cardFactory == null)
            {
                Debug.LogError("CardFactory reference is not set in the inspector!");
                return;
            }
            CardData newCardData = cardFactory.CreateCardData(card);//CARD IMAGE Como PARAMETRO
            cardDatas.Add(newCardData);
        }
    }
//original GenerateCards
    // public  List<CardDisplay> GenerateCards(Transform transform)//este metodo podria ser void 
    // {
    //     List<CardDisplay> deck = new List<CardDisplay>();
    //     foreach (CardData card in cardDatas)
    //     {
    //         GameObject newCard = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity, transform);
    //         newCard.transform.localPosition = Vector3.zero; // Ajusta según necesites
    //         CardDisplay display = newCard.GetComponent<CardDisplay>();
    //         if (display != null && card != null)
    //         {
    //             display.cardData = card;
    //             display.UpdateCard();
    //             deck.Add(display);
    //             // Debug.Log("Carta creada: " + card.cardName);
    //         }
    //         else
    //         {
    //             Debug.LogError("Componente CardDisplay o datos de carta faltantes.");
    //         }
    //     }
    //     return deck;
    // }

public IEnumerator GenerateCards(Transform handTransform, Transform deckTransform)
{
    foreach (CardData card in cardDatas)
    {
        GameObject newCard = Instantiate(cardPrefab, deckTransform.position, Quaternion.identity, handTransform);
        CardDisplay display = newCard.GetComponent<CardDisplay>();
        if (display != null && card != null)
        {
            display.cardData = card;
            display.UpdateCard();
            yield return StartCoroutine(MoveCard(newCard.transform, handTransform.position,handTransform));
        }
        else
        {
            Debug.LogError("Componente CardDisplay o datos de carta faltantes.");
        }
    }
}

IEnumerator MoveCard(Transform cardTransform, Vector3 targetPosition,Transform parentTransform)
{
    float timeToMove = 0.5f; // Duración de la animación en segundos
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
      // Asegúrate de que la carta está como hija del contenedor correcto


    // Método para obtener cartas desde el motor, esto es un ejemplo genérico
    private List<Card> GetCardsFromEngine()
    {
        if (P1CardsGenerated)
        {
            // Aquí debes implementar la lógica para obtener las cartas del deck del jugador desde el motor
            Debug.Log("entré a GetCardFromEngine");
            return Game.GameInstance.Player2.Faction.Deck;

        }
        else
        {
            P1CardsGenerated = true;
            return Game.GameInstance.Player1.Faction.Deck;
        }
    }
    public static bool CanPlaceCard(Card card, BattleField battleField)
    {
        Debug.Log("Entre al Metodo CanPlaceCard");
        // Verify if the card's player is the owner of the battlefield and it's their turn
        return card.player == TurnManager.Instance.GetCurrentPlayer() && card.player == battleField.Owner;
    }

    public void PlaceCard(Card card, Transform row, BattleField battleField)
    {
        if (CanPlaceCard(card, battleField))
        {
            // Logic to add card to the row
            Debug.Log("Card placed successfully.");
        }
        else
        {
            Debug.Log("You cannot place this card now or here.");
        }
    }

    public void UICardTheft(Player player) //robar una carta 
    {
        Card stolenCard = Card.CardTheft(player);
        if (player==Game.GameInstance.Player1)//hay que hacer algo para que maneje el momento en el que se quede sin cartas el deck
        {
           
            InstanciateCard( stolenCard); //instancia la carta en la mano visualmente
            // Game.GameInstance.Player1.Hand.Add(Game.GameInstance.Player1.Faction.Deck[index]);//añade la carta a la mano del engine
            // Game.GameInstance.Player1.Faction.Deck.Remove(Game.GameInstance.Player1.Faction.Deck[index]);//saca la carta del deck
        }
         if (player==Game.GameInstance.Player2)//hay que hacer algo para que maneje el momento en el que se quede sin cartas el deck
        {
            // int index = random.Next(0,Game.GameInstance.Player2.Faction.Deck.Count);
            InstanciateCard( stolenCard); //instancia la carta en la mano visualmente
            // Game.GameInstance.Player2.Hand.Add(Game.GameInstance.Player2.Faction.Deck[index]);//añade la carta a la mano del engine
            // Game.GameInstance.Player2.Faction.Deck.Remove(Game.GameInstance.Player2.Faction.Deck[index]);//saca la carta del deck
        }
    }
    public void UIIncreaseMyRow(Draggable card)
    {
        CardDisplay newCardDisplay = card.GetComponent<CardDisplay>();
        Debug.Log("blalblablablablbalbla     " + newCardDisplay.cardData.Card.player.Board.rows[(int)newCardDisplay.cardData.Card.position].Count);
        Debug.Log("indice en UIIncreaseRow     " + (int)newCardDisplay.cardData.Card.position);
        newCardDisplay.cardData.Card.IncreaseMyRow(newCardDisplay.cardData.Card);//llamando a esta funcion para que haga lo que tiene que hacer en el engine
        Debug.Log("EL VACIO => "+ newCardDisplay.cardData.Card.player.Name);
        if(TurnManager.Instance.GetCurrentPlayer() == Game.GameInstance.Player1)//actualiza los puntos de los jugadores 
        {    
            PlayerManager.Instance.Player1.GetComponentInChildren<PlayerDisplay>().points.text = Game.GameInstance.Player1.Points.ToString();
        }
        else
        {
            PlayerManager.Instance.Player2.GetComponentInChildren<PlayerDisplay>().points.text = Game.GameInstance.Player2.Points.ToString();
        }
        foreach (var item in card.GetComponentInParent<BattleRow>().row)
        {
           item.cardData.points = item.cardData.Card.points;
           item.UpdateCard();//muestra los puntos actualizados en la interfaz
        }
    }


}
