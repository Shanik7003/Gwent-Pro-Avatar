using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Engine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
public enum Owner {
    Player1,
    Player2
}
public enum CombatRow{
    M=0,
    R=1,
    S=2
}
public class BattleRow : MonoBehaviour, IDropHandler
{
    public List<CardDisplay> row;
    public Owner rowOwner;  // Propietario de la fila (Player1 o Player2)
    public CombatRow CombatRow;
    public bool IncreasePlace;
    
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
                ExistPasiveIncrease(cardDisplay);//comprueba si existe alguna aumento pasivo en esa fila y si existe lo aplica
                ExistPasiveWheather(cardDisplay);
                row.Add(cardDisplay);//añade la carta visual (CardDisplay) a la battlerow
                PlaceCardinBoardEngine(card);//coloca tambien en el board del engine su carta gemela del engine para asi llevar los dos tableros a la par  
                UpdatePlayerDisplay(card: card);//actualiza los punto sde los jugadores visuales 
                FreeHability(card);
                card.isDraggable = false;//para que el usuarioa no la pueda mover mas 
                // GetComponent<CanvasGroup>().blocksRaycasts = false;  // Desactiva el raycast
               
            }
            else
            {
                card.dropSuccess = false;
                Debug.LogError("Drop no permitido en " + transform.name);
            }
        }
    }
    public void  ExistPasiveIncrease(CardDisplay cardDisplay)//comprueba si existe alguna aumento pasivo en esa fila y si existe lo aplica
    {
        foreach (var item in cardDisplay.cardData.Card.player.Board.rows[(int)cardDisplay.GetComponentInParent<BattleRow>().CombatRow])
        {
            if (item.CardType == CardType.IncreaseCard && item.ID != cardDisplay.cardData.Card.ID)//verifica en el engine si la fila posee alguna carta de aumento
            {
                int SumPoints = item.points;
                cardDisplay.cardData.points += SumPoints ;//sumale los puntos de la carta que esta efectuando el incremento
                cardDisplay.UpdateCard();//actualiza los puntos visuales de la carta
                cardDisplay.cardData.Card.points += SumPoints;//actualiza la carta del engine 
            }
        }
    }
    public void  ExistPasiveWheather(CardDisplay cardDisplay)//comprueba si existe alguna aumento pasivo en esa fila y si existe lo aplica
    {
        if (Game.GameInstance.WheatherSpace.Spaces[(int)cardDisplay.GetComponentInParent<BattleRow>().CombatRow] != null)//si el espacio de clima correspondiente a la carta no esta vacio
        {
            //actualiza la carta y el jugador del engine 
            cardDisplay.cardData.Card.points = cardDisplay.cardData.Card.points/2;
            cardDisplay.cardData.Card.player.Points -= cardDisplay.cardData.Card.points/2;
            //actualiza los puntos de los jugadores visuales
            PlayerManager.Instance.Player1.GetComponentInChildren<PlayerDisplay>().points.text = Game.GameInstance.Player1.Points.ToString();
            PlayerManager.Instance.Player2.GetComponentInChildren<PlayerDisplay>().points.text = Game.GameInstance.Player2.Points.ToString();
            //actualiza la carta visual
            cardDisplay.cardData.points = cardDisplay.cardData.Card.points;
            cardDisplay.UpdateCard();
        } 
        else return;
        
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
        if (IncreasePlace)//si es un espacio de cartas de aumento
        {
            if (row.Count == 1)
            {
                return false;
            }
        }
        if (cardDisplay.cardData.Card.CardType == CardType.WheatherCard)
        {
            return false;
        }
        if (cardDisplay.cardData.Card.CardType == CardType.IncreaseCard)//es una carta de aumento?
        {
           if (!IncreasePlace)//no la estas colocando en un lugar de aumneto?
           {
             return false;
           }
           //si la estas colocando en un lugar de aumneto continua y verifica todo lo demas 
        }

        Player cardOwner = cardDisplay.cardData.owner;
        bool isPlayerTurn = TurnManager.Instance.IsPlayerTurn(cardOwner);
        // Chequeo de que el propietario de la carta y el propietario de la fila coinciden
        bool correctOwner = (rowOwner == Owner.Player1 && cardOwner == Game.GameInstance.Player1) || (rowOwner == Owner.Player2 && cardOwner == Game.GameInstance.Player2);
        bool positionAllowed = cardDisplay.cardData.Card.position.ToString().Contains(CombatRow.ToString());//verifica si la string de la posicion de la carta contiene la letra de la fila en que se esta queriendo colocar
        //bool CardType = 
        return isPlayerTurn && correctOwner && positionAllowed;
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
    public void PlaceCardinBoardEngine(Draggable card)// coloca la carta que el usuario coloco en una fila de batalla en el board del engine y ademas le suma lso puntos al jugador del engine  
    {
        CardDisplay cardDisplay = card.GetComponent<CardDisplay>();
        cardDisplay.cardData.Card.player.Board.rows[(int)CombatRow].Add(cardDisplay.cardData.Card); // añade la carta al board del jugador del engine
        cardDisplay.cardData.Card.player.Points += cardDisplay.cardData.Card.points; //incrementa los puntos del jugador
   
    }
    public void UpdatePlayerDisplay(Draggable card)//actualiza los puntos de los jugadores visuales
    {
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
            if(cardDisplay.cardData.Card.player == Game.GameInstance.Player1)
            {
                Debug.Log(" imprimiendocardFactory .................. => "+CardManager.Instance.cardFactory);
                CardManager.Instance.UICardTheft(Game.GameInstance.Player1);
            }
            if(cardDisplay.cardData.Card.player == Game.GameInstance.Player2)
            {
                Debug.Log(" imprimiendocardFactory .................. => "+CardManager.Instance.cardFactory);
                CardManager.Instance.UICardTheft(Game.GameInstance.Player2);
            }
        }
        if (cardDisplay.cardData.Card.hability == Habilities.IncreaseMyRow)
        {
            CardManager.Instance.UIIncreaseMyRow(card);
        }
        if (cardDisplay.cardData.Card.hability == Habilities.EliminateMostPowerful)
        {
            if (TurnManager.Instance.GetCurrentPlayer() == Game.GameInstance.Player1)
            {
                CardManager.Instance.UIEliminateMostPowerful(Game.GameInstance.Player2);//que seria el enemy
            }
             if (TurnManager.Instance.GetCurrentPlayer() == Game.GameInstance.Player2)
            {
                CardManager.Instance.UIEliminateMostPowerful(Game.GameInstance.Player1);//que seria el enemy 
            }
            
        }
        if (cardDisplay.cardData.Card.hability == Habilities.EliminateLeastPowerful)
        {
            if (TurnManager.Instance.GetCurrentPlayer() == Game.GameInstance.Player1)
            {
                CardManager.Instance.UIEliminateLeastPowerful(Game.GameInstance.Player2);//que seria el enemy
            }
            if (TurnManager.Instance.GetCurrentPlayer() == Game.GameInstance.Player2)
            {
                CardManager.Instance.UIEliminateLeastPowerful(Game.GameInstance.Player1);//que seria el enemy 
            }
            
        }
        if (cardDisplay.cardData.Card.hability == Habilities.MultiPoints)
        {
            CardManager.Instance.UIMultiPoints(cardDisplay);
        }
        // if(cardDisplay.cardData.Card.hability == Habilities.Eclipse)
        // {
        //     CardManager.Instance.UIDecreaseMyRow(card);
        // }
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
