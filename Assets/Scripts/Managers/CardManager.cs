using System.Collections.Generic;
using System.Linq;
using Engine;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

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
        GenerateCards(player1HandRow);
        GenerateCardData(Game.GameInstance.Player2.Hand);
        GenerateCards(player2HandRow);
        

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
            GenerateCards(player1HandRow);
        }
        if (card.player == Game.GameInstance.Player2)
        {
            GenerateCards(player2HandRow);
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

    public  List<CardDisplay> GenerateCards(Transform transform)//este metodo podria ser void 
    {
        List<CardDisplay> deck = new List<CardDisplay>();
        foreach (CardData card in cardDatas)
        {
            GameObject newCard = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity, transform);
            newCard.transform.localPosition = Vector3.zero; // Ajusta según necesites
            CardDisplay display = newCard.GetComponent<CardDisplay>();
            if (display != null && card != null)
            {
                display.cardData = card;
                display.UpdateCard();
                deck.Add(display);
                // Debug.Log("Carta creada: " + card.cardName);
            }
            else
            {
                Debug.LogError("Componente CardDisplay o datos de carta faltantes.");
            }
        }
        return deck;
    }

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
        System.Random random = new();
       //preferiria poner aqui en random para que el indice de la crata que voy a instanciar fuese desde 0 a deck.Count pero no me deja crear una variable random no se porque???
        if (player==Game.GameInstance.Player1)//hay que hacer algo para que maneje el momento en el que se quede sin cartas el deck
        {
            int index = random.Next(0,Game.GameInstance.Player1.Faction.Deck.Count);
            InstanciateCard( Game.GameInstance.Player1.Faction.Deck[index]);
            Game.GameInstance.Player1.Hand.Add(Game.GameInstance.Player1.Faction.Deck[index]);
            Game.GameInstance.Player1.Faction.Deck.Remove(Game.GameInstance.Player1.Faction.Deck[index]);
            //recuerda remover la carta del deck cuando la instancies y ademas sacarla del deck y ponerla en la mano del engine del jugador 
        }
    }


}
