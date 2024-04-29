using UnityEngine;
using Engine;
using System.Timers;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using JetBrains.Annotations;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public ShowHidePanel PanelEndRound; // Referencia al script ShowHidePanel
    public BattleRow[] Rows = new BattleRow[12];
    public void OnEndTurnButtonClicked()
    {
        TurnManager.Instance.EndTurn();
    }
    public void OnPassButtonClicked()
    {
        if (TurnManager.Instance.GetCurrentEnemy().AlreadyPass)//si el otro jugados ya paso
        {
            if(IsGameOver())
            {
                //metodos para terminar el juego definitivamente
            }

            //resetear la escena de tablero para la segunda ronda 
            PanelEndRound.ShowPanelWithMessage($"This Round is over, the winner of the round is {RoundWinner().Name} continue to next round");
            ResetBoard();
            ResetWheatherSpaces();
            ResetEngineBoard();
            Game.GameInstance.Player1.AlreadyPass = false;
            Game.GameInstance.Player2.AlreadyPass = false;
            GameObject.Find("EndTurnPlayer").GetComponent<Button>().interactable = true;//activa de nuevo el boton de terminar el turno de la ronda 
           
            return;
        }
        TurnManager.Instance.GetCurrentPlayer().AlreadyPass = true;
        TurnManager.Instance.EndTurn();
        GameObject.Find("EndTurnPlayer").GetComponent<Button>().interactable = false;//desactiva el boton de terminar el turnopara que no se pueda usar mas 
    }
    public Player RoundWinner()
    {
        Debug.Log("Entre a RoundWinner");
        Debug.Log("el nombre es >>>>>>>>>>>>>>>>>>>>>>" + Game.GameInstance.Player1.Name);
        if (Game.GameInstance.Player1.Points > Game.GameInstance.Player2.Points)
        {
            Game.GameInstance.Player1.Gems.Add(1);//sumale una gema 
            //pon una gema visual para despues 
            return Game.GameInstance.Player1;
        }
        else
        {
            Game.GameInstance.Player2.Gems.Add(1);//sumale una gema
            //pon una gema visual
            return Game.GameInstance.Player2;
        }
    }
    public bool IsGameOver()
    {
        if(Game.GameInstance.Player1.Gems.Count >= 2 )
        {
            //hacer las cosas para terminar el juego
            Debug.Log("el ganador del juego es el player1");
            return true;
        }
        if(Game.GameInstance.Player2.Gems.Count >= 2 )
        {
            //hacer las cosas para terminar el juego 
            Debug.Log("el ganador del juego es el player2");
            return true;
        }
        return false;
    }
    public void ResetBoard()//mueve al cementerio todas las cartas de las filas 
    {
        for (int i = 0; i < Rows.Length; i++)
        {
            foreach (var item in Rows[i].GetComponent<BattleRow>().row)
            {
                CardManager.Instance.EliminateCard(item);//se moveran al cementerio todas las cartas en la interfaz
                //resetea los board del engine de cada juagador 
               
            }
        }
    }
    public void ResetWheatherSpaces()
    {
        foreach (var item in GameObject.Find("WM").GetComponent<WheatherSpace>().space)
        {
            GameObject.Find("WM").GetComponent<WheatherSpace>().space.Remove(item);
            //cambia los colores de las filas 
            GameObject.Find("WM").GetComponent<WheatherSpace>().BattleRowPlayer1.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f); // Semi-transparent black
            GameObject.Find("WM").GetComponent<WheatherSpace>().BattleRowPlayer2.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f); // Semi-transparent black
            CardManager.Instance.EliminateCard(item);//se moveran al cementerio todas las cartas en la interfaz
        }
        foreach (var item in GameObject.Find("WR").GetComponent<WheatherSpace>().space)
        {
            GameObject.Find("WR").GetComponent<WheatherSpace>().space.Remove(item);
            //cambia los colores de las filas 
            GameObject.Find("WR").GetComponent<WheatherSpace>().BattleRowPlayer1.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f); // Semi-transparent black
            GameObject.Find("WR").GetComponent<WheatherSpace>().BattleRowPlayer2.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f); // Semi-transparent black
            CardManager.Instance.EliminateCard(item);//se moveran al cementerio todas las cartas en la interfaz
            
        }
         foreach (var item in GameObject.Find("WS").GetComponent<WheatherSpace>().space)
        {
            GameObject.Find("WS").GetComponent<WheatherSpace>().space.Remove(item);
            //cambia los colores de las filas 
            GameObject.Find("WS").GetComponent<WheatherSpace>().BattleRowPlayer1.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f); // Semi-transparent black
            GameObject.Find("WS").GetComponent<WheatherSpace>().BattleRowPlayer2.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f); // Semi-transparent black
            CardManager.Instance.EliminateCard(item);//se moveran al cementerio todas las cartas en la interfaz
        }
    }
    public void ResetEngineBoard()
    {
        Debug.Log("<><><><><><><><><><>><><> entre al ultimo metodo <><><><><><><><><><><><><><><><><>");
        Game.GameInstance.Player1.Board.ResetEngineBoard();
        Game.GameInstance.Player2.Board.ResetEngineBoard();
    }
}
