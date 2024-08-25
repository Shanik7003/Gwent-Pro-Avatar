using System.Linq.Expressions;
using Engine;
using Unity.VisualScripting;
using UnityEngine;

public class GameManagerWrapper : MonoBehaviour
{
    void Start()
    {
        // Debug.Log("GameManagerWrapper attached and running");
    }
    public static GameManagerWrapper Instance { get; private set; }
    void Awake()
    {
        // Debug.Log("Awake called in GameManagerWrapper");
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            // Debug.Log("GameManagerWrapper instance set and marked as DontDestroyOnLoad");
        }
        else
        {
            // Debug.Log("Instance already exists, destroying duplicate");
            Destroy(gameObject);
        }
    }

    public void SetPlayersInfo1(string PlayerName,Faction Faction)
    {
        if (Game.GameInstance.Player1 == null || Game.GameInstance.Player2 == null)
        {
            // Debug.LogError("Players are not initialized.");
            return;
        }
        Game.GameInstance.Player1.Name = PlayerName;
        Game.GameInstance.Player1.Faction = Faction;

        foreach (var card in Game.AllCards.Values)
        {
            if (card.faction == Faction)
            {
                card.player = Game.GameInstance.Player1;
                Game.GameInstance.Player1.Deck.Add(card);
                Game.GameInstance.Board.Add(card);
            }
        }
       
    }
    public void SetPlayersInfo2(string PlayerName,Engine.Faction Faction)
    {
        if (Game.GameInstance.Player1 == null || Game.GameInstance.Player2 == null)
        {
            // Debug.LogError("Players are not initialized.");
            return;
        }
        Game.GameInstance.Player2.Name = PlayerName;
        Game.GameInstance.Player2.Faction = Faction;
        
        foreach (var card in Game.AllCards.Values)
        {
            if (card.faction == Faction)
            {
                card.player = Game.GameInstance.Player2;
                Game.GameInstance.Player2.Deck.Add(card);
                Game.GameInstance.Board.Add(card);
            }
        }
       
    }

    public void StartGame()
    {
        //gameInstance.StartNewGame();
    }
}
