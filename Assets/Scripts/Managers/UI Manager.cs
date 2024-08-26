using UnityEngine;
using Engine;
using System.Timers;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.UI;
using System.Linq;
using System;
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

            //resetear la escena de tablero para la segunda ronda 
            PanelEndRound.ShowPanelWithMessage($"This Round is over, the winner of the round is {WinnerName} continue to next round");
            ResetBoard();
            ResetWheatherSpaces();
            ResetEngineBoard();
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
        Debug.Log("Entre a RoundWinner");
        Debug.Log("el nombre es >>>>>>>>>>>>>>>>>>>>>>" + Game.GameInstance.Player1.Name);
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
            Debug.Log("el ganador del juego es el player1");
            return true;
        }
        if(Game.GameInstance.Player2.Gems.Count >= 2 )
        {
            //hacer las cosas para terminar el juego 
            PanelGameOver.ShowPanelWithMessage("GAME OVER, El ganador es " + Game.GameInstance.Player2.Name);
            Debug.Log("el ganador del juego es el player2");
            return true;
        }
        return false;
    }
    public void ResetBoard()//mueve al cementerio todas las cartas de las filas 
    {
        for (int i = 0; i < Rows.Length; i++)
        {
            Debug.Log("No esta vacio Rows");
            foreach (var item in Rows[i].GetComponent<BattleRow>().row)
            {
                //Debug.Log("No esta vacio Rows[i]");
                if(item == null)
                {
                    continue;
                }
                CardManager.Instance.EliminateCard(item);//se moveran al cementerio todas las cartas en la interfaz
                //resetea los board del engine de cada juagador 
            }
            Rows[i].GetComponent<BattleRow>().row = new List<CardDisplay>();
        }
        //actualiza los puntos de los jugadores del engine
        Game.GameInstance.Player1.Points =0;
        Game.GameInstance.Player2.Points = 0;
        //actualiza los puntos de los jugadores visuales 
        PlayerManager.Instance.Player1.GetComponentInChildren<PlayerDisplay>().points.text = Game.GameInstance.Player1.Points.ToString();
        PlayerManager.Instance.Player2.GetComponentInChildren<PlayerDisplay>().points.text = Game.GameInstance.Player2.Points.ToString();
    }
    public void ResetWheatherSpaces()
    {
        foreach (var item in GameObject.Find("WM").GetComponent<WheatherSpace>().space)
        {
            //cambia los colores de las filas 
             if (item == null)
            {
               continue; 
            }
            GameObject.Find("WM").GetComponent<WheatherSpace>().BattleRowPlayer1.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f); // Semi-transparent black
            GameObject.Find("WM").GetComponent<WheatherSpace>().BattleRowPlayer2.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f); // Semi-transparent black
            CardManager.Instance.EliminateCard(item);//se moveran al cementerio todas las cartas en la interfaz
        }
        GameObject.Find("WM").GetComponent<WheatherSpace>().space = new List<CardDisplay>();
        foreach (var item in GameObject.Find("WR").GetComponent<WheatherSpace>().space)
        {
            //cambia los colores de las filas 
            if (item == null)
            {
               continue; 
            }
            GameObject.Find("WR").GetComponent<WheatherSpace>().BattleRowPlayer1.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f); // Semi-transparent black
            GameObject.Find("WR").GetComponent<WheatherSpace>().BattleRowPlayer2.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f); // Semi-transparent black
            CardManager.Instance.EliminateCard(item);//se moveran al cementerio todas las cartas en la interfaz
            
        }
        GameObject.Find("WR").GetComponent<WheatherSpace>().space=new List<CardDisplay>();
         foreach (var item in GameObject.Find("WS").GetComponent<WheatherSpace>().space)
        {
            //cambia los colores de las filas 
             if (item == null)
            {
               continue; 
            }
            GameObject.Find("WS").GetComponent<WheatherSpace>().BattleRowPlayer1.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f); // Semi-transparent black
            GameObject.Find("WS").GetComponent<WheatherSpace>().BattleRowPlayer2.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f); // Semi-transparent black
            CardManager.Instance.EliminateCard(item);//se moveran al cementerio todas las cartas en la interfaz
        }
        GameObject.Find("WS").GetComponent<WheatherSpace>().space = new List<CardDisplay>();
    }
    public void ResetEngineBoard()
    {
        Game.GameInstance.Player1.Board.ResetEngineBoard();
        Game.GameInstance.Player2.Board.ResetEngineBoard();
        Debug.Log("ME FUE BIEN EN RESET ENGINE BOARD");
    }
}
