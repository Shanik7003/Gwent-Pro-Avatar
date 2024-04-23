using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Engine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
public enum Owner {
    Player1,
    Player2
}
public class BattleRow : MonoBehaviour, IDropHandler
{
    public Owner rowOwner;  // Propietario de la fila (Player1 o Player2)
    public Position position;
    public void OnDrop(PointerEventData eventData)
    { 
        Draggable card = eventData.pointerDrag.GetComponent<Draggable>();
        CardDisplay cardDisplay = card.GetComponent<CardDisplay>();
        if (card != null)
        {
            Debug.Log("Intentando colocar la carta en BattleRow");
            if (IsDropAllowed(card))
            {
                card.GetComponent<CanvasGroup>().blocksRaycasts = true;// poniendo el componente canvas group y el blockraycast a true para que no inetrfiera con la visualizacion
                card.transform.SetParent(transform);
                card.transform.localPosition = Vector3.zero;
                card.dropSuccess = true; // si la carta fue colocada en el tablero 
                PlaceCardinBoardEngine(card);//coloca tambien en el board del engine su carta gemela del engine para asi llevar los dos tableros a la par 
                ActivateCard(card);
                FreeHability(card);
                card.isDraggable = false;//para que el usuarioa no la pueda mover mas 
                Debug.Log($"Carta colocada en el board del Engine del jugador {cardDisplay.cardData.owner.Name} en la fila {position}");
                Debug.Log("Carta colocada correctamente en el BattleField de la UI" + transform.name);
                Debug.Log($"puntos de {cardDisplay.cardData.Card.player.Name}con ID={cardDisplay.cardData.Card.player.Id} ======== {cardDisplay.cardData.Card.player.Points}");
            }
            else
            {
                card.dropSuccess = false;
                Debug.LogError("Drop no permitido en " + transform.name);
            }
        }
    }

    public bool IsDropAllowed(Draggable card)
    {
        Debug.Log("entre a IsDropAllowed");
        CardDisplay cardDisplay = card.GetComponent<CardDisplay>();
        if (cardDisplay == null)
        {
            Debug.LogError("El objeto arrastrado no tiene un componente CardDisplay asociado.");
            return false;
        }

        Player cardOwner = cardDisplay.cardData.owner;
        Debug.Log("NOMBRE DEL DUEÑO DE LA  CARTA ------------- " + cardDisplay.cardData.owner.Name);
        bool isPlayerTurn = TurnManager.Instance.IsPlayerTurn(cardOwner);
        Debug.Log("es el turno de este player??????????????----------" + isPlayerTurn);
        // Chequeo de que el propietario de la carta y el propietario de la fila coinciden
        bool correctOwner = (rowOwner == Owner.Player1 && cardOwner == Game.GameInstance.Player1) || (rowOwner == Owner.Player2 && cardOwner == Game.GameInstance.Player2);
        Debug.Log("es el owner correcto?---------" + correctOwner);
        return isPlayerTurn && correctOwner;
    }
     // Método para obtener todas las cartas en esta fila específica
    public List<Draggable> GetCardsInRow()
    {
        List<Draggable> cardsInRow = new List<Draggable>();
        foreach (Transform child in transform)
        {
            Draggable card = child.GetComponent<Draggable>();
            if (card != null)
            {
                cardsInRow.Add(card);
            }
        }
        return cardsInRow;
    }
    // private void PlaceCardinBoardEngine(Draggable card)//se encarga de poner la carta gemela de la carta visual colocada en una fila en el board del engine para que los dos boards vayan a la par
    // {
    //     CardDisplay cardDisplay = card.GetComponent<CardDisplay>();
    //     Debug.Log("nombre de la carta del engine que moviste:    " + cardDisplay.cardData.name);
    //     cardDisplay.cardData.Card.player.Board.rows[(int)position].Add( cardDisplay.cardData.Card); //recordar la def  (public List<Card>[] rows = new List<Card>[3];)     
    // }
    public void PlaceCardinBoardEngine(Draggable card)// coloca la carta que el usuario coloco en una fila de batalla en el board del engine y ademas le suma lso puntos al jugador del engine  
    {
        CardDisplay cardDisplay = card.GetComponent<CardDisplay>();
        Debug.Log($"puntos de {cardDisplay.cardData.Card.player.Name}con ID={cardDisplay.cardData.Card.player.Id} ANTES DE SUMARLE LOS PUNTOS DE ESTA CARTA ======== {cardDisplay.cardData.Card.player.Points}");
        #region Debug
        if (cardDisplay == null)
        {
            Debug.LogError("cardDisplay es null");
            return;
        }
        if (cardDisplay.cardData == null)
        {
            Debug.LogError("cardData es null");
            return;
        }
        if (cardDisplay.cardData.Card == null)
        {
            Debug.LogError("Card es null");
            return;
        }
        if (cardDisplay.cardData.Card.player == null)
        {
            Debug.LogError("Player es null");
            return;
        }
        if (cardDisplay.cardData.Card.player.Board == null)
        {
            Debug.LogError("Board es null");
            return;
        }
        if (cardDisplay.cardData.Card.player.Board.rows == null)
        {
            Debug.LogError("rows es null");
            return;
        }
        if (cardDisplay.cardData.Card.player.Board.rows.Length <= (int)position)
        {
            Debug.LogError("El arreglo rows no tiene un índice para 'position'");
            return;
        }
        if (cardDisplay.cardData.Card.player.Board.rows[(int)position] == null)
        {
            Debug.LogError("La lista en la posición indicada es null");
            return;
        }   
        #endregion
        cardDisplay.cardData.Card.player.Board.rows[(int)position].Add(cardDisplay.cardData.Card); // añade la carta al board del jugador del engine
        cardDisplay.cardData.Card.player.Points += cardDisplay.cardData.Card.points; //incrementa los puntos del jugador
        Debug.Log($"Carta colocada en el board del Engine del jugador {cardDisplay.cardData.owner.Name} en la fila {position}");
        Debug.Log("Carta colocada correctamente en el BattleField de la UI" + transform.name);
        Debug.Log($"puntos de {cardDisplay.cardData.Card.player.Name} con ID={cardDisplay.cardData.Card.player.Id} ======== {cardDisplay.cardData.Card.points}");
        Debug.Log("nombre de la carta del engine que moviste:    " + cardDisplay.cardData.name);
        //Debug.Log($"puntos de {cardDisplay.cardData.owner.Name} ======== {cardDisplay.cardData.Card.points}");
    }
    public void ActivateCard(Draggable card)
    {
        //aqui podria llamr a PlaceCardinBoardEngine para que todo se haga en una sola funcion 
        //CardDisplay cardDisplay = card.GetComponent<CardDisplay>();
        if(TurnManager.Instance.GetCurrentPlayer() == Game.GameInstance.Player1)
        {
            PlayerManager.Instance.Player1.GetComponentInChildren<PlayerDisplay>().points.text = Game.GameInstance.Player1.Points.ToString();
        }
        if(TurnManager.Instance.GetCurrentPlayer() == Game.GameInstance.Player2)
        {
            PlayerManager.Instance.Player2.GetComponentInChildren<PlayerDisplay>().points.text = Game.GameInstance.Player2.Points.ToString();
        }
    }
    public void FreeHability(Draggable card)
    {
        Debug.Log("Entre a FreeHability!!!!!!!!!!!!!!!!!!!!!!!!");
        CardDisplay cardDisplay = card.GetComponent<CardDisplay>();
        if (cardDisplay.cardData.Card.hability == Habilities.CardTheft)
        {
            Debug.Log("Entre al if de FreeHability");
            if(CardManager.Instance == null)
            {
                Debug.Log("CardManager.Instance es null");
            }
            Debug.Log( "el nombre de la carta que puse en el board es:        "+ cardDisplay.cardData.Card.name);
            if(cardDisplay.cardData.Card.player == Game.GameInstance.Player1)
            {
                Debug.Log(" imprimiendocardFactory .................. => "+CardManager.Instance.cardFactory);
                CardManager.Instance.UICardTheft(Game.GameInstance.Player1);
            }
            
          
        }
    }


    private bool IsCardInHand(Draggable card)//esta funcion ya no la uso al no instanciar el deck completo en el boardpero la dejo aqui por si acaso
    {
        CardDisplay cardDisplay = card.GetComponent<CardDisplay>();
        if (cardDisplay == null)
        {
            Debug.LogError("El objeto arrastrado no tiene un componente CardDisplay asociado.");
            return false;
        }
        Debug.Log("El NOMBRE DEL DUEÑO DE LA CARTA DE ESTE CARDDISPLAY ES " + cardDisplay.cardData.Card.player.Name);
        Debug.Log("REPETIMOS EL NOMBRE DE DUEÑO DE LA CARTA ES ..................  " +cardDisplay.cardData.owner.Name);
        Player cardOwner  = cardDisplay.cardData.owner;
        foreach (var item in cardOwner.Hand)
        {
            if (cardDisplay.cardData.Card == item)//si la carta que paso es iagul a alguna de las de la mano 
            {
                return true;
            }
        }
        return false;
    }
}
