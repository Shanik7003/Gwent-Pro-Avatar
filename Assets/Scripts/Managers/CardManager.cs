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
                // // Debug.Log("imprimirndo _instance.cardFactory LA DEFINITIVA =====> "+_instance.cardFactory);
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
            // Debug.Log("entre a InstanciateCard");
            StartCoroutine(GenerateCards(player1HandRow,player1DeckHolder));
        }
        if (card.player == Game.GameInstance.Player2)
        {
            StartCoroutine(GenerateCards(player2HandRow,player2DeckHolder));
        }
    }
    public  void GenerateCardData(List<Card> collection)
    {
        // Debug.Log(frame);
        cardDatas = new List<CardData>();
        foreach (var card in collection)
        {
            // Debug.Log("estoy generando CardDatas");
            // Debug.Log("Imprimiendo this.............." + cardFactory);
            if (cardFactory == null)
            {
                // Debug.LogError("CardFactory reference is not set in the inspector!");
                return;
            }
            CardData newCardData = cardFactory.CreateCardData(card);
            cardDatas.Add(newCardData);
        }
    }
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
                // Debug.LogError("Componente CardDisplay o datos de carta faltantes.");
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

    // Método para obtener cartas desde el motor, esto es un ejemplo genérico
    private List<Card> GetCardsFromEngine()
    {
        if (P1CardsGenerated)
        {
            // Aquí debes implementar la lógica para obtener las cartas del deck del jugador desde el motor
            // Debug.Log("entré a GetCardFromEngine");
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
        // Debug.Log("Entre al Metodo CanPlaceCard");
        // Verify if the card's player is the owner of the battlefield and it's their turn
        return card.player == TurnManager.Instance.GetCurrentPlayer() && card.player == battleField.Owner;
    }

    public void PlaceCard(Card card, Transform row, BattleField battleField)
    {
        if (CanPlaceCard(card, battleField))
        {
            // Logic to add card to the row
            // Debug.Log("Card placed successfully.");
        }
        else
        {
            // Debug.Log("You cannot place this card now or here.");
        }
    }
    public void EliminateCard(CardDisplay cardDisplay)//este metoo es para ser usado por la habilidad de eliminar la carta mas o menos poderosa, ya que elimina la carta que le indiques del campo del jugador que no este jugando en este turno 
    {
        if (cardDisplay.cardData.Card.player == Game.GameInstance.Player1)
        {
            StartCoroutine(MoveCardToCemetery(cardDisplay.transform, GameObject.Find("Cemetery1").transform));
            Destroy(cardDisplay.gameObject, 1.0f); // Asumiendo que hay un delay para ver la animación antes de destruir el objeto.
        }
        if ( cardDisplay.cardData.Card.player == Game.GameInstance.Player2)
        {
            StartCoroutine(MoveCardToCemetery(cardDisplay.transform, GameObject.Find("Cemetery2").transform));
            Destroy(cardDisplay.gameObject, 1.0f); // Asumiendo que hay un delay para ver la animación antes de destruir el objeto.
        }
       
    }
    IEnumerator MoveCardToCemetery(Transform cardTransform, Transform cemeteryTransform)
    {
        float timeToMove = 0.5f; // Duración de la animación en segundos
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

    public void UICardTheft(Player player) //robar una carta 
    {
        Card stolenCard = Card.CardTheft(player);
        if (player==Game.GameInstance.Player1)//hay que hacer algo para que maneje el momento en el que se quede sin cartas el deck
        {
            InstanciateCard( stolenCard); //instancia la carta en la mano visualmente
        }
         if (player==Game.GameInstance.Player2)//hay que hacer algo para que maneje el momento en el que se quede sin cartas el deck
        {
            InstanciateCard( stolenCard); //instancia la carta en la mano visualmente
        }
    }
    public void UIIncreaseMyRow(Draggable card)
    {
        CardDisplay newCardDisplay = card.GetComponent<CardDisplay>();
        newCardDisplay.cardData.Card.IncreaseMyRow(newCardDisplay.cardData.Card,(int)card.GetComponentInParent<BattleRow>().CombatRow);//llamando a esta funcion para que haga lo que tiene que hacer en el engine
        if(TurnManager.Instance.GetCurrentPlayer() == Game.GameInstance.Player1)//actualiza los puntos de los jugadores 
        {    
            PlayerManager.Instance.Player1.GetComponentInChildren<PlayerDisplay>().points.text = Game.GameInstance.Player1.Points.ToString();
        }
        else
        {
            PlayerManager.Instance.Player2.GetComponentInChildren<PlayerDisplay>().points.text = Game.GameInstance.Player2.Points.ToString();
        }
        if (!card.GetComponentInParent<BattleRow>().IncreasePlace )//si no es un increaseplace
        {
            foreach (var item in card.GetComponentInParent<BattleRow>().row)//por cada carta de la lista de CardDisplay de la fila de la interfaz, actualiza sus puntos 
            {
            item.cardData.points = item.cardData.Card.points;
            item.UpdateCard();//muestra los puntos actualizados en la interfaz
            }
            
        }
        //si la carta si esta en un IncreasePlace como iterar por la lista row de la fila ?????
        if (card.GetComponentInParent<BattleRow>().IncreasePlace)
        {
            if (TurnManager.Instance.GetCurrentPlayer() == Game.GameInstance.Player1)
            {
                if (card.GetComponentInParent<BattleRow>().CombatRow == CombatRow.M)
                {
                    foreach(var item in GameObject.Find("M1").GetComponent<BattleRow>().row)
                    {
                        item.cardData.points = item.cardData.Card.points;
                        item.UpdateCard();//muestra los puntos actualizados en la interfaz
                    }
                }
                if (card.GetComponentInParent<BattleRow>().CombatRow == CombatRow.R)
                {
                    foreach(var item in GameObject.Find("R1").GetComponent<BattleRow>().row)
                    {
                        item.cardData.points = item.cardData.Card.points;
                        item.UpdateCard();//muestra los puntos actualizados en la interfaz
                    }
                }
                 if (card.GetComponentInParent<BattleRow>().CombatRow == CombatRow.S)
                {
                    foreach(var item in GameObject.Find("S1").GetComponent<BattleRow>().row)
                    {
                        item.cardData.points = item.cardData.Card.points;
                        item.UpdateCard();//muestra los puntos actualizados en la interfaz
                    }
                }
            }  
              if (TurnManager.Instance.GetCurrentPlayer() == Game.GameInstance.Player2)
            {
                if (card.GetComponentInParent<BattleRow>().CombatRow == CombatRow.M)
                {
                    foreach(var item in GameObject.Find("M2").GetComponent<BattleRow>().row)
                    {
                        item.cardData.points = item.cardData.Card.points;
                        item.UpdateCard();//muestra los puntos actualizados en la interfaz
                    }
                }
                if (card.GetComponentInParent<BattleRow>().CombatRow == CombatRow.R)
                {
                    foreach(var item in GameObject.Find("R2").GetComponent<BattleRow>().row)
                    {
                        item.cardData.points = item.cardData.Card.points;
                        item.UpdateCard();//muestra los puntos actualizados en la interfaz
                    }
                }
                 if (card.GetComponentInParent<BattleRow>().CombatRow == CombatRow.S)
                {
                    foreach(var item in GameObject.Find("S2").GetComponent<BattleRow>().row)
                    {
                        item.cardData.points = item.cardData.Card.points;
                        item.UpdateCard();//muestra los puntos actualizados en la interfaz
                    }
                }
            } 
        }
        if (TurnManager.Instance.GetCurrentPlayer() == Game.GameInstance.Player1)
        {
            if (card.GetComponentInParent<BattleRow>().CombatRow == CombatRow.M)
            {
                foreach(var item in GameObject.Find("M1").GetComponent<BattleRow>().row)
                {
                    item.cardData.points = item.cardData.Card.points;
                    item.UpdateCard();//muestra los puntos actualizados en la interfaz
                }
            }
              if (card.GetComponentInParent<BattleRow>().CombatRow == CombatRow.M)
            {
                foreach(var item in GameObject.Find("M1").GetComponent<BattleRow>().row)
                {
                    item.cardData.points = item.cardData.Card.points;
                    item.UpdateCard();//muestra los puntos actualizados en la interfaz
                }
            }
        }
      
    }
    public void UIEliminateMostPowerful(Player enemy)
    {
        int MostPowerfulID = Card.EliminateMostPowerful(enemy);
        if (Game.GameInstance.Player1 == TurnManager.Instance.GetCurrentPlayer())//el enemigo va a ser el jugador contrario al que esta jugando ahora 
        {
            for (int i = 0; i < GameObject.Find("FIELD2").GetComponent<BattleField>().battleRows.Length; i++)
            {
                 foreach (var item in GameObject.Find("FIELD2").GetComponent<BattleField>().battleRows[i].row)
                {
                    if (item.cardData.Card.ID == MostPowerfulID)
                    {
                        // Debug.Log ("entre al if para llamar al metodo de eliminar la carta ");
                        //saca la carta de las filas de combate y ponla en el cementerio 
                        EliminateCard(item);
                        GameObject.Find("FIELD2").GetComponent<BattleField>().battleRows[i].row.Remove(item);

                        PlayerManager.Instance.Player2.GetComponentInChildren<PlayerDisplay>().points.text = Game.GameInstance.Player2.Points.ToString();
                        break;
                    }
                }
            }
           
        }
         if (Game.GameInstance.Player2 == TurnManager.Instance.GetCurrentPlayer())//el enemigo va a ser el jugador contrario al que esta jugando ahora 
        {
            for (int i = 0; i < GameObject.Find("FIELD1").GetComponent<BattleField>().battleRows.Length; i++)
            {
                foreach (var item in GameObject.Find("FIELD1").GetComponent<BattleField>().battleRows[i].row)
                {
                    if (item.cardData.Card.ID == MostPowerfulID)
                    {
                        // Debug.Log ("entre al if para llamar al metodo de eliminar la carta ");
                        //saca la carta de las filas de combate y ponla en el cementerio 
                        EliminateCard(item);
                        GameObject.Find("FIELD1").GetComponent<BattleField>().battleRows[i].row.Remove(item);
                        PlayerManager.Instance.Player1.GetComponentInChildren<PlayerDisplay>().points.text = Game.GameInstance.Player1.Points.ToString();
                        break;
                    }
                }
            }
           
        }
    }
    public void UIEliminateLeastPowerful(Player enemy)
    {
        int MostPowerfulID = Card.EliminateLeastPowerful(enemy);
        if (Game.GameInstance.Player1 == TurnManager.Instance.GetCurrentPlayer())//el enemigo va a ser el jugador contrario al que esta jugando ahora 
        {
            for (int i = 0; i < GameObject.Find("FIELD2").GetComponent<BattleField>().battleRows.Length; i++)
            {
                 foreach (var item in GameObject.Find("FIELD2").GetComponent<BattleField>().battleRows[i].row)
                {
                    if (item.cardData.Card.ID == MostPowerfulID)
                    {
                        // Debug.Log ("entre al if para llamar al metodo de eliminar la carta ");
                        //saca la carta de las filas de combate y ponla en el cementerio 
                        EliminateCard(item);
                        PlayerManager.Instance.Player2.GetComponentInChildren<PlayerDisplay>().points.text = Game.GameInstance.Player2.Points.ToString();
                        break;
                    }
                }
            } 
        }

        else if (Game.GameInstance.Player2 == TurnManager.Instance.GetCurrentPlayer())//el enemigo va a ser el jugador contrario al que esta jugando ahora 
        {
            for (int i = 0; i < GameObject.Find("FIELD1").GetComponent<BattleField>().battleRows.Length; i++)
            {
                 foreach (var item in GameObject.Find("FIELD1").GetComponent<BattleField>().battleRows[i].row)
                {
                    if (item.cardData.Card.ID == MostPowerfulID)
                    {
                        // Debug.Log ("entre al if para llamar al metodo de eliminar la carta ");
                        //saca la carta de las filas de combate y ponla en el cementerio 
                        EliminateCard(item);
                        PlayerManager.Instance.Player1.GetComponentInChildren<PlayerDisplay>().points.text = Game.GameInstance.Player1.Points.ToString();
                        break;
                    }
                }
            }
           
        }
    }
    public void UIMultiPoints(CardDisplay card)
    {
        double points = Card.Multipoints(card.cardData.Card);
        card.cardData.points = points;
        card.UpdateCard();
        if(TurnManager.Instance.GetCurrentPlayer() == Game.GameInstance.Player1)//actualiza los puntos de los jugadores 
        {    
            PlayerManager.Instance.Player1.GetComponentInChildren<PlayerDisplay>().points.text = Game.GameInstance.Player1.Points.ToString();
        }
        else
        {
            PlayerManager.Instance.Player2.GetComponentInChildren<PlayerDisplay>().points.text = Game.GameInstance.Player2.Points.ToString();
        }

    }
    public void UIDecreaseMyRow(Draggable card)
    {
        // Debug.Log("Entre a UIDecreaseMyRow");
        CardDisplay newCardDisplay = card.GetComponent<CardDisplay>();
        newCardDisplay.cardData.Card.Eclipse(newCardDisplay.cardData.Card,(int)card.GetComponentInParent<WheatherSpace>().CombatRow,TurnManager.Instance.GetCurrentEnemy());//llamando a esta funcion para que haga lo que tiene que hacer en el engine
       //actualiza los puntos visuales de los jugadores 
        PlayerManager.Instance.Player1.GetComponentInChildren<PlayerDisplay>().points.text = Game.GameInstance.Player1.Points.ToString();
        PlayerManager.Instance.Player2.GetComponentInChildren<PlayerDisplay>().points.text = Game.GameInstance.Player2.Points.ToString();
        foreach (var item in card.GetComponentInParent<WheatherSpace>().BattleRowPlayer1.row)
        {
            if(item.cardData.Card.CardType == CardType.IncreaseCard)
            {
                continue;
            }
            item.cardData.points = item.cardData.Card.points;
            item.UpdateCard();//muestra los puntos actualizados en la interfaz
        }
        foreach (var item in card.GetComponentInParent<WheatherSpace>().BattleRowPlayer2.row)
        {
            if(item.cardData.Card.CardType == CardType.IncreaseCard)
            {
                continue;
            }
            item.cardData.points = item.cardData.Card.points;
            item.UpdateCard();//muestra los puntos actualizados en la interfaz
        }
        card.GetComponentInParent<WheatherSpace>().BattleRowPlayer1.GetComponent<Image>().color = new Color(1, 0, 0, 0.5f); // Semi-transparent red
        card.GetComponentInParent<WheatherSpace>().BattleRowPlayer2.GetComponent<Image>().color = new Color(1, 0, 0, 0.5f); // Semi-transparent red
    }
    public void UIClearence(Draggable card ,Player owner)//habilidad para las cartas de despeje
    {
        CardDisplay newCardDisplay = card.GetComponent<CardDisplay>();
        newCardDisplay.cardData.Card.Clearence(card.GetComponent<CardDisplay>().cardData.Card,(int)card.GetComponentInParent<BattleRow>().CombatRow,TurnManager.Instance.GetCurrentEnemy());
      
        if (card.GetComponentInParent<BattleRow>().CombatRow == CombatRow.M)
        {
            if (GameObject.Find("WM").GetComponent<WheatherSpace>().space.Count != 0)//si en ese espeacio hay una carta de clima 
            {
                if (GameObject.Find("WM").GetComponent<WheatherSpace>().space[0] == null)
                {
                    return;
                }
                //eliminala y quita su efecto en todas las cartas de esa fila y la fila corespondiente en el campo del jugador 
                EliminateCard(GameObject.Find("WM").GetComponent<WheatherSpace>().space[0]);//elimina la carta de clima visualmente
                GameObject.Find("WM").GetComponent<WheatherSpace>().space.Remove(GameObject.Find("WM").GetComponent<WheatherSpace>().space[0]);

                foreach (var item in card.GetComponentInParent<BattleRow>().row)
                {
                    item.cardData.points = item.cardData.Card.points;
                    item.UpdateCard();//muestra los puntos actualizados en la interfaz
                }
                if (TurnManager.Instance.GetCurrentEnemy() == Game.GameInstance.Player1)
                {
                    foreach (var item in GameObject.Find("M1").GetComponent<BattleRow>().row)
                    {
                        item.cardData.points = item.cardData.Card.points;
                        item.UpdateCard();//muestra los puntos actualizados en la interfaz
                    }
                }
                else
                {
                    foreach (var item in GameObject.Find("M2").GetComponent<BattleRow>().row)
                    {
                        item.cardData.points = item.cardData.Card.points;
                        item.UpdateCard();//muestra los puntos actualizados en la interfaz
                    }
                }

            }
            GameObject.Find("WM").GetComponent<WheatherSpace>().BattleRowPlayer1.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f); // Semi-transparent black
            GameObject.Find("WM").GetComponent<WheatherSpace>().BattleRowPlayer2.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f); // Semi-transparent black
        }
        if (card.GetComponentInParent<BattleRow>().CombatRow == CombatRow.R)
        {

            if (GameObject.Find("WR").GetComponent<WheatherSpace>().space.Count != 0)//si en ese espeacio hay una carta de clima 
            {
                if (GameObject.Find("WR").GetComponent<WheatherSpace>().space[0] == null)
                {
                    return;
                }
                //eliminala y quita su efecto en todas las cartas de esa fila y la fila corespondiente en el campo del jugador 
                EliminateCard(GameObject.Find("WR").GetComponent<WheatherSpace>().space[0]);//elimina la carta de clima 
                 GameObject.Find("WR").GetComponent<WheatherSpace>().space.Remove(GameObject.Find("WR").GetComponent<WheatherSpace>().space[0]);
                foreach (var item in card.GetComponentInParent<BattleRow>().row)
                {
                    item.cardData.points = item.cardData.Card.points;
                    item.UpdateCard();//muestra los puntos actualizados en la interfaz
                    
                }
                if (TurnManager.Instance.GetCurrentEnemy() == Game.GameInstance.Player1)
                {
                    foreach (var item in GameObject.Find("R1").GetComponent<BattleRow>().row)
                    {
                        item.cardData.points = item.cardData.Card.points;
                        item.UpdateCard();//muestra los puntos actualizados en la interfaz
                        
                    }
                }
                else
                {
                    foreach (var item in GameObject.Find("R2").GetComponent<BattleRow>().row)
                    {
                        item.cardData.points = item.cardData.Card.points;
                        item.UpdateCard();//muestra los puntos actualizados en la interfaz
                        
                    }
                }
            }
            GameObject.Find("WR").GetComponent<WheatherSpace>().BattleRowPlayer1.GetComponent<Image>().color = new Color(0, 0, 0, 0.8f); // Semi-transparent black
            GameObject.Find("WR").GetComponent<WheatherSpace>().BattleRowPlayer2.GetComponent<Image>().color = new Color(0, 0, 0, 0.8f); // Semi-transparent black
        }
        if (card.GetComponentInParent<BattleRow>().CombatRow == CombatRow.S)
        {
            if (GameObject.Find("WS").GetComponent<WheatherSpace>().space.Count != 0)//si en ese espeacio hay una carta de clima 
            {
                if (GameObject.Find("WS").GetComponent<WheatherSpace>().space[0] == null)
                {
                    return;
                }
                //eliminala y quita su efecto en todas las cartas de esa fila y la fila corespondiente en el campo del jugador 
                EliminateCard(GameObject.Find("WS").GetComponent<WheatherSpace>().space[0]);//elimina la carta de clima 
                GameObject.Find("WS").GetComponent<WheatherSpace>().space.Remove(GameObject.Find("WS").GetComponent<WheatherSpace>().space[0]);
                foreach (var item in card.GetComponentInParent<BattleRow>().row)
                {
                    item.cardData.points = item.cardData.Card.points;
                    item.UpdateCard();//muestra los puntos actualizados en la interfaz
                    Game.GameInstance.WheatherSpace.Spaces[(int)CombatRow.S] = null;
                    
                    
                }
                if (TurnManager.Instance.GetCurrentEnemy() == Game.GameInstance.Player1)
                {
                    foreach (var item in GameObject.Find("S1").GetComponent<BattleRow>().row)
                    {
                        item.cardData.points = item.cardData.Card.points;
                        item.UpdateCard();//muestra los puntos actualizados en la interfaz
                        
                    }
                }
                else
                {
                    foreach (var item in GameObject.Find("S2").GetComponent<BattleRow>().row)
                    {
                        item.cardData.points = item.cardData.Card.points;
                        item.UpdateCard();//muestra los puntos actualizados en la interfaz
                        
                    }
                }
            }
            GameObject.Find("WS").GetComponent<WheatherSpace>().BattleRowPlayer1.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f); // Semi-transparent black
            GameObject.Find("WS").GetComponent<WheatherSpace>().BattleRowPlayer2.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f); // Semi-transparent black
        }
        PlayerManager.Instance.Player1.GetComponentInChildren<PlayerDisplay>().points.text = Game.GameInstance.Player1.Points.ToString();
        PlayerManager.Instance.Player2.GetComponentInChildren<PlayerDisplay>().points.text = Game.GameInstance.Player2.Points.ToString();
    }
    public void UICleanRow(Draggable card)
    {

        CardDisplay newCardDisplay = card.GetComponent<CardDisplay>();
        (Position,Player) rowAndPlayer = Card.CleanRow(newCardDisplay.cardData.Card,TurnManager.Instance.GetCurrentEnemy());
        
        //(Position,Player) rowAndPlayer = (Position.M, Game.GameInstance.Player1);
        if (rowAndPlayer.Item2 == Game.GameInstance.Player1)
        {
            if (rowAndPlayer.Item1 == Position.M)
            {
                foreach (var item in GameObject.Find("M1").GetComponent<BattleRow>().row)
                {
                    if(item == null)
                    {
                        continue;
                    }
                    EliminateCard(item);
                    
                }
                GameObject.Find("M1").GetComponent<BattleRow>().row = new List<CardDisplay>();
            }
            if (rowAndPlayer.Item1 == Position.R)
            {
                foreach (var item in GameObject.Find("R1").GetComponent<BattleRow>().row)
                {
                    if(item == null)
                    {
                        continue;
                    }
                    EliminateCard(item);
                }
                GameObject.Find("R1").GetComponent<BattleRow>().row = new List<CardDisplay>();
            }
            if (rowAndPlayer.Item1 == Position.S)
            {
                foreach (var item in GameObject.Find("S1").GetComponent<BattleRow>().row)
                {
                    if(item == null)
                    {
                        continue;
                    }
                    EliminateCard(item);
              
                }
                GameObject.Find("S1").GetComponent<BattleRow>().row = new List<CardDisplay>();
            }
            PlayerManager.Instance.Player1.GetComponentInChildren<PlayerDisplay>().points.text = Game.GameInstance.Player1.Points.ToString();
        }
        if (rowAndPlayer.Item2 == Game.GameInstance.Player2)
        {
            if (rowAndPlayer.Item1 == Position.M)
            {
                foreach (var item in GameObject.Find("M2").GetComponent<BattleRow>().row)
                {
                    if(item == null)
                    {
                        continue;
                    }
                    EliminateCard(item);
                    
                }
                GameObject.Find("M2").GetComponent<BattleRow>().row = new List<CardDisplay>();
            }
            if (rowAndPlayer.Item1 == Position.R)
            {
                foreach (var item in GameObject.Find("R2").GetComponent<BattleRow>().row)
                {
                    if(item == null)
                    {
                        continue;
                    }
                    EliminateCard(item);
                  
                }
                GameObject.Find("R2").GetComponent<BattleRow>().row = new List<CardDisplay>();
            }
            if (rowAndPlayer.Item1 == Position.S)
            {
                foreach (var item in GameObject.Find("S2").GetComponent<BattleRow>().row)
                {
                    if(item == null)
                    {
                        continue;
                    }
                  
                    EliminateCard(item);
                   
                }
                GameObject.Find("S2").GetComponent<BattleRow>().row = new List<CardDisplay>();
            }
            PlayerManager.Instance.Player2.GetComponentInChildren<PlayerDisplay>().points.text = Game.GameInstance.Player2.Points.ToString();
        }
        

    }
}