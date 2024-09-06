using System.Collections.Generic;
using System.Linq.Expressions;
using Engine;
using Unity.VisualScripting;
using UnityEngine;

public class GameManagerWrapper : MonoBehaviour
{
    void Start()
    {
        Debug.Log("GameManagerWrapper attached and running");
    }
    public static GameManagerWrapper Instance { get; private set; }
    void Awake()
    {
        //Debug.Log("Awake called in GameManagerWrapper");
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            //Debug.Log("GameManagerWrapper instance set and marked as DontDestroyOnLoad");
        }
        else
        {
            //Debug.Log("Instance already exists, destroying duplicate");
            Destroy(gameObject);
        }
    }

    public void SetPlayersInfo(string Player1Name,Faction Faction1,string Player2Name, Faction Faction2)
    {
        if (Game.GameInstance.Player1 == null || Game.GameInstance.Player2 == null)
        {

            return;
        }
        Game.GameInstance.Player1.Name = Player1Name;
        Game.GameInstance.Player1.Faction = Faction1;
        Game.GameInstance.Player2.Name = Player2Name;
        Game.GameInstance.Player2.Faction = Faction2;

        //*!este es el codigo que hay que hacerlo despues de que ya se hallan asignado los nombres 
        //*! semantico, si no se construyeron cartas entonces sigue como siempre 
        if (RunButtonScript.ast != null)
        {
            Debug.Log("estoy entrando a el execute donde se añade la carta al diccionario grande");
            ExecutionVisitor executionVisitor = new(RunButtonScript.ast);
            executionVisitor.Visit(RunButtonScript.ast);
        }
        int count = 1;
          
        Debug.Log("Cartas que se añaden al el Deck del player1 " + Game.GameInstance.Player1.Name);
        foreach (var card in Game.AllCards.Values)
        {
            if(card.name == "Beluga")
            {
                Debug.Log("");
            }
            if (card.faction == Faction1)
            {
                card.player = Game.GameInstance.Player1;
                card.MoveCard(Game.GameInstance.Player1.Deck);
                Debug.Log(count + " " +card.name);
                count++;
            }
        }

        foreach (var card in Game.AllCards.Values)
        {
            if (card.faction == Faction2)
            {
                card.player = Game.GameInstance.Player2;
                card.MoveCard(Game.GameInstance.Player2.Deck);
            }
        }
       
    }
}
