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
    //public List<CardDisplay> row;
    public Owner rowOwner;  // Propietario de la fila (Player1 o Player2)
    public CombatRow CombatRow;
    public bool IncreasePlace;
    
    public void OnDrop(PointerEventData eventData)
    { 
        Draggable card = eventData.pointerDrag.GetComponent<Draggable>();
        CardDisplay cardDisplay = card.GetComponent<CardDisplay>();
        if (card != null)
        {
            //Debug.Log("Intentando colocar la carta en BattleRow");
            if (IsDropAllowed(card))
            {
                card.GetComponent<CanvasGroup>().blocksRaycasts = true;// poniendo el componente canvas group y el blockraycast a true para que no inetrfiera con la visualizacion

                card.transform.SetParent(transform);
                card.transform.localPosition = Vector3.zero;
                card.dropSuccess = true; // si la carta fue colocada en el tablero 

                // PlaceCardinBoardEngine(cardDisplay);//coloca tambien en el board del engine su carta gemela del engine para asi llevar los dos tableros a la par  
     
                // ExistsPasiveIncrease(cardDisplay);//comprueba si existe alguna aumento pasivo en esa fila y si existe lo aplica
                // ExistsPasiveWheather(cardDisplay);

                // FreeHability(card);

                // card.isDraggable = false;//para que el usuario no la pueda mover mas
                // card.GetComponent<CanvasGroup>().interactable = false;
                ActivateCard(cardDisplay);
       
                if (TurnManager.Instance.GetCurrentEnemy().AlreadyPass)//si tu enemigo paso :juega todas las cartas que quieres hasta que decidas pasar
                {
                    return;//y no entras al EndTurn de abajo
                }
                TurnManager.Instance.EndTurn();//pasar de turno
                // GetComponent<CanvasGroup>().blocksRaycasts = false;  // Desactiva el raycast
               
            }
            else
            {
                card.dropSuccess = false;
                //Debug.LogError("Drop no permitido en " + transform.name);
            }
        }
    }
    public void ActivateCard(CardDisplay cardDisplay)
    {
        //CardDisplay cardDisplay = card.GetComponent<CardDisplay>();
        Draggable card = cardDisplay.GetComponent<Draggable>();
        PlaceCardinBoardEngine(cardDisplay);//coloca tambien en el board del engine su carta gemela del engine para asi llevar los dos tableros a la par  

        ExistsPasiveIncrease(cardDisplay);//comprueba si existe alguna aumento pasivo en esa fila y si existe lo aplica
        ExistsPasiveWheather(cardDisplay);

        FreeHability(card);

        card.isDraggable = false;//para que el usuario no la pueda mover mas
        card.GetComponent<CanvasGroup>().interactable = false;
    }

    public void  ExistsPasiveIncrease(CardDisplay cardDisplay)//comprueba si existe alguna aumento pasivo en esa fila y si existe lo aplica
    {
        foreach (var item in cardDisplay.cardData.Card.player.Board.rows[(int)cardDisplay.GetComponentInParent<BattleRow>().CombatRow])
        {
            if (item.CardType == CardType.IncreaseCard && item.Id != cardDisplay.cardData.Card.Id)//verifica en el engine si la fila posee alguna carta de aumento
            {
                double SumPoints = item.points;
                cardDisplay.card.points += SumPoints;
                cardDisplay.card.player.Points += SumPoints; //actualiza los puntos del jugador del engine;  
            }
        }
    }
    public void  ExistsPasiveWheather(CardDisplay cardDisplay)//comprueba si existe alguna aumento pasivo en esa fila y si existe lo aplica
    {
        //cuando pregunats si existe un pasiveWheather ya la pusiste en el tablero del engine , es decir que ya sumaste los puntos que tenia la carta al jugador, ahora tienes que quitarle al jugador la mitad 
        if (Game.GameInstance.WheatherSpace.Spaces[(int)cardDisplay.GetComponentInParent<BattleRow>().CombatRow] != null)//si el espacio de clima correspondiente a la carta no esta vacio
        {
            cardDisplay.card.points /= 2;//actualiza los puntos de la carta del engine
            cardDisplay.card.player.Points -= cardDisplay.card.points;//actualiza los del player
        } 
        else return;  
    }

    public bool IsDropAllowed(Draggable card)
    {
        //Debug.Log("entre a IsDropAllowed");
        CardDisplay cardDisplay = card.GetComponent<CardDisplay>();
        if (cardDisplay == null)
        {
            //Debug.LogError("El objeto arrastrado no tiene un componente CardDisplay asociado.");
            return false;
        }
        if (IncreasePlace)//si es un espacio de cartas de aumento
        {
            
            foreach (var item in cardDisplay.card.player.Board.rows[(int)CombatRow])
            {
                if (item.CardType == CardType.IncreaseCard)//*!ya existe una carta de aumento en esa fila 
                {
                    return false;
                }
            }
        }
        if (cardDisplay.card.CardType == CardType.WheatherCard)
        {
            return false;
        }
        if (cardDisplay.card.CardType == CardType.IncreaseCard)//es una carta de aumento?
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
        return isPlayerTurn && correctOwner && positionAllowed && !TurnManager.Instance.GetCurrentPlayer().AlreadyPass;
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
    public void PlaceCardinBoardEngine(CardDisplay cardDisplay)// coloca la carta que el usuario coloco en una fila de batalla en el board del engine y ademas le suma lso puntos al jugador del engine  
    {
        List<Card> Destino = cardDisplay.card.player.Board.rows[(int)CombatRow];
        
        cardDisplay.card.Ubication.Remove( cardDisplay.card);
        Destino.Add( cardDisplay.card);
        cardDisplay.card.Ubication = Destino;
        
        cardDisplay.card.player.Field.Add(cardDisplay.card);//añadela siempre a  el field del player 
        Game.GameInstance.Board.Add(cardDisplay.card);

        cardDisplay.card.player.Points += cardDisplay.card.points; //incrementa los puntos del jugador
   
    }

    public void FreeHability(Draggable card)
    {

        CardDisplay cardDisplay = card.GetComponent<CardDisplay>();
        Player player = cardDisplay.cardData.Card.player;
        
        if (cardDisplay.card.hability == Habilities.Personalized)
        {
            ExecutionVisitor executionVisitor = new(RunButtonScript.ast);//*!esto hay que cambiarlo despues 
            // //executionVisitor.Visit(RunButtonScript.ast);

            
            foreach (var effectInvocation in cardDisplay.card.CardNode.EffectList)
            {
                executionVisitor.Visit(effectInvocation);
            }
            
            return;
        }
            
            

        if (cardDisplay.card.hability == Habilities.CardTheft)
        {
           Card.CardTheft(player);
           return;
        }

        if (cardDisplay.cardData.Card.hability == Habilities.IncreaseMyRow)
        {
            cardDisplay.card.IncreaseMyRow(cardDisplay.card,(int)card.GetComponentInParent<BattleRow>().CombatRow);//llamando a esta funcion para que haga lo que tiene que hacer en el engine
            return;
        }


        if (cardDisplay.cardData.Card.hability == Habilities.EliminateMostPowerful)
        {
            //*! en esta los jugadores estan invertidos, arreglar la logica 
            if (TurnManager.Instance.GetCurrentPlayer() == Game.GameInstance.Player1)
            {
                Card.EliminateMostPowerful(Game.GameInstance.Player2);
            }
            if (TurnManager.Instance.GetCurrentPlayer() == Game.GameInstance.Player2)
            {
                Card.EliminateMostPowerful(Game.GameInstance.Player1);
            }
            return;
        }

        if (cardDisplay.cardData.Card.hability == Habilities.EliminateLeastPowerful)
        {
            if (TurnManager.Instance.GetCurrentPlayer() == Game.GameInstance.Player1)
            {
                Card.EliminateLeastPowerful(Game.GameInstance.Player2);
            }
            if (TurnManager.Instance.GetCurrentPlayer() == Game.GameInstance.Player2)
            {
                Card.EliminateLeastPowerful(Game.GameInstance.Player1);
            }
            return;
        }

        if (cardDisplay.cardData.Card.hability == Habilities.MultiPoints)
        {
            Card.Multipoints(cardDisplay.card);
            return;
        }

        if (cardDisplay.cardData.Card.hability == Habilities.Clearence)
        {
            cardDisplay.card.Clearence(cardDisplay.card,(int)card.GetComponentInParent<BattleRow>().CombatRow,TurnManager.Instance.GetCurrentEnemy());
            return;
        }

        if (cardDisplay.cardData.Card.hability == Habilities.CleanRow)
        {
            Card.CleanRow(cardDisplay.card);
            return;
        }
     
    }


    private bool IsCardInHand(Draggable card)//esta funcion ya no la uso al no instanciar el deck completo en el boardpero la dejo aqui por si acaso
    {
        CardDisplay cardDisplay = card.GetComponent<CardDisplay>();
        if (cardDisplay == null)
        {
            //Debug.LogError("El objeto arrastrado no tiene un componente CardDisplay asociado.");
            return false;
        }
  
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
