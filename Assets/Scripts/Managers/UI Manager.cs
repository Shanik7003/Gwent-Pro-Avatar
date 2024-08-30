using UnityEngine;
using Engine;
using System.Timers;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.UI;
using System.Linq;
using System;
using System.ComponentModel;
public class UIManager : MonoBehaviour
{
    public ShowHidePanel PanelEndRound; // Referencia al script ShowHidePanel
    public ShowPanel PanelGameOver;
    public BattleRow[] Rows = new BattleRow[12];
  
    public void OnPassButtonClicked()
    {
        if (TurnManager.Instance.GetCurrentEnemy().AlreadyPass)//si el otro jugados ya paso
        {
            string WinnerName = RoundWinner();//llamando lo antes de verificar si el juego se acabo para que le sume la gema 
            if(IsGameOver())
            {
                return;
            }
            Debug.Log("cartas en el board "+ Game.GameInstance.Board.Count);
            //resetear la escena de tablero para la segunda ronda 
            PanelEndRound.ShowPanelWithMessage($"This Round is over, the winner of the round is {WinnerName} continue to next round");
            ResetBoard();
            Debug.Log("cartas en el board "+ Game.GameInstance.Board.Count);
            ResetWheatherSpaces();
            Game.GameInstance.Player1.AlreadyPass = false;
            Game.GameInstance.Player2.AlreadyPass = false; 
            TurnManager.Instance.GetCurrentPlayer().AlreadyPass = false;
            TurnManager.Instance.GetCurrentEnemy().AlreadyPass = false;
            
            return;
        }

        TurnManager.Instance.GetCurrentPlayer().AlreadyPass = true;
        TurnManager.Instance.EndTurn();

    }
    public string RoundWinner()
    {
        //Debug.Log("Entre a RoundWinner");
        //Debug.Log("el nombre es >>>>>>>>>>>>>>>>>>>>>>" + Game.GameInstance.Player1.Name);
        if (Game.GameInstance.Player1.Points > Game.GameInstance.Player2.Points)
        {
            Game.GameInstance.Player1.Gems.Add(1);//sumale una gema 
            //pon una gema visual para despues 
            return Game.GameInstance.Player1.Name;
        }
        if(Game.GameInstance.Player1.Points == Game.GameInstance.Player2.Points)
        {
            return "NoBody";
        }
        else
        {
            Game.GameInstance.Player2.Gems.Add(1);//sumale una gema
            //pon una gema visual
            return Game.GameInstance.Player2.Name;
        }
    }
    public bool IsGameOver()
    {
        if(Game.GameInstance.Player1.Gems.Count >= 2 )
        {
            //hacer las cosas para terminar el juego
            PanelGameOver.ShowPanelWithMessage("GAME OVER, El ganador es " + Game.GameInstance.Player1.Name);
            //Debug.Log("el ganador del juego es el player1");
            return true;
        }
        if(Game.GameInstance.Player2.Gems.Count >= 2 )
        {
            //hacer las cosas para terminar el juego 
            PanelGameOver.ShowPanelWithMessage("GAME OVER, El ganador es " + Game.GameInstance.Player2.Name);
            //Debug.Log("el ganador del juego es el player2");
            return true;
        }
        return false;
    }
    public void ResetBoard()//mueve al cementerio todas las cartas de las filas 
    {
        List<Card> container = new();
        for (int i = 0; i < Game.GameInstance.Board.Count; i++)
        {
            if(Game.GameInstance.Board[i] != null)
            {
                Debug.Log("elimine una ");
                //Game.GameInstance.Board[i].RemoveCard();
                container.Add(Game.GameInstance.Board[i]);
            }
        }
        foreach (var item in container)
        {
            if (Game.GameInstance.Board.Contains(item))
            {
                item.player.Points -= item.points;
                item.RemoveCard();
            }
        }
    }
    public void ResetWheatherSpaces()
    {
        for (int i = 0; i < Game.GameInstance.WheatherSpace.Spaces.Length; i++)
        {
            if (Game.GameInstance.WheatherSpace.Spaces[i] != null)
            {
                Game.GameInstance.WheatherSpace.Spaces[i].RemoveCard();
            }
        }
        Game.GameInstance.WheatherSpace.Spaces = new Card[3];
    }

}
