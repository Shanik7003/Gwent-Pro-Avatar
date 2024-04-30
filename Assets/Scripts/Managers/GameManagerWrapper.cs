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

    public void SetPlayersInfo(string Player1Name,string Player2Name,Engine.Faction Faction1,Engine.Faction Faction2)
    {
        if (Game.GameInstance.Player1 == null || Game.GameInstance.Player2 == null)
        {
            // Debug.LogError("Players are not initialized.");
            return;
        }
        Game.GameInstance.Player1.Name = Player1Name;
        Game.GameInstance.Player1.Faction = Faction1;
        foreach (var card in Faction1.Deck)
        {
            card.player = Game.GameInstance.Player1;
        }
        Game.GameInstance.Player2.Name = Player2Name;
        Game.GameInstance.Player2.Faction = Faction2;
        foreach (var card in Faction2.Deck)
        {
            card.player = Game.GameInstance.Player2;
        }
    }
   

    
    public void StartGame()
    {
        //gameInstance.StartNewGame();
    }

    // Puedes añadir más métodos aquí para exponer más funcionalidades de Game
}
