using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Engine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
public class WheatherSpace : MonoBehaviour, IDropHandler
{
    public List<CardDisplay> space = new();
    public CombatRow CombatRow;
    public BattleRow BattleRowPlayer1;
    public BattleRow BattleRowPlayer2;

    public void OnDrop(PointerEventData eventData)
    {
        Draggable card = eventData.pointerDrag.GetComponent<Draggable>();
        CardDisplay cardDisplay = card.GetComponent<CardDisplay>();
        if (IsDropAllowed(card)) 
        {  
            card.GetComponent<CanvasGroup>().blocksRaycasts = true;// poniendo el componente canvas group y el blockraycast a true para que no inetrfiera con la visualizacion
            card.transform.SetParent(transform);
            card.transform.localPosition = Vector3.zero;
            card.dropSuccess = true; // si la carta fue colocada en el tablero 
            //ExistsPasiveWheather(cardDisplay);
            space.Add(cardDisplay);//añade la carta visual (CardDisplay) a el espacio de wheather
            PlaceCardInWheatherSpace(card);
            FreeWheather(card);
            UpdatePlayerDisplay(card: card);//actualiza los punto sde los jugadores visuales 
            card.isDraggable = false;//para que el usuarioa no la pueda mover mas 
            if (TurnManager.Instance.GetCurrentEnemy().AlreadyPass)
            {
                return;
            }
            TurnManager.Instance.EndTurn();//pasar el turno
            //GetComponent<CanvasGroup>().blocksRaycasts = false;  // Desactiva el raycast
        }   
        else
        {
            card.dropSuccess = false;
            // Debug.LogError("Drop no permitido en " + transform.name);
        }
    }
    public bool IsDropAllowed(Draggable card)
    {
        // Debug.Log("entre a IsDropAllowed");
        CardDisplay cardDisplay = card.GetComponent<CardDisplay>();
     
        if (space.Count == 1)//para no poder poner mas de una carta en un espacio de clima 
        {
            return false;
        }
        
        if (cardDisplay == null)
        {
            // Debug.LogError("El objeto arrastrado no tiene un componente CardDisplay asociado.");
            return false;
        }
        if (cardDisplay.cardData.Card.CardType == CardType.WheatherCard)//si es una carta de clima?
        {
            Player cardOwner = cardDisplay.cardData.owner;
            bool isPlayerTurn = TurnManager.Instance.IsPlayerTurn(cardOwner);
            bool positionAllowed = cardDisplay.cardData.Card.position.ToString().Contains(CombatRow.ToString());//verifica si la string de la posicion de la carta contiene la letra de la fila en que se esta queriendo colocar
            //bool CardType = 
            return isPlayerTurn && positionAllowed;
        }
       return false;//si no es una carta d clima no va a entrar al if y siempre retirna falso
    }
    public void PlaceCardInWheatherSpace(Draggable card)
    {
        CardDisplay cardDisplay = card.GetComponent<CardDisplay>();
        //coloca la carta en el espcio de clima correspondiente 
        Game.GameInstance.WheatherSpace.Spaces[(int)CombatRow] = cardDisplay.cardData.Card;
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
    public void FreeWheather (Draggable card)
    {
        // Debug.Log("Entre a FreeWheather");
        CardDisplay cardDisplay = card.GetComponent<CardDisplay>();
        if(cardDisplay.cardData.Card.hability == Habilities.Eclipse)
        {
            CardManager.Instance.UIDecreaseMyRow(card);
        }
    }
    
}