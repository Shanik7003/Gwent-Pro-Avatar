using Engine;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    // private void Awake()
    // {
    //     if (Instance == null)
    //     {
    //         Instance = this;
    //         DontDestroyOnLoad(gameObject);
    //     }
    //     else
    //     {
    //         Destroy(gameObject);
    //     }
    // }
    //  public void SetPlayersInfo(string Player1Name,string Player2Name,Engine.Faction Faction1,Engine.Faction Faction2)
    // {
    //     if (Game.GameInstance.Player1 == null || Game.GameInstance.Player2 == null)
    //     {
    //         Debug.LogError("Players are not initialized.");
    //         return;
    //     }
    //     Game.GameInstance.Player1.Name = Player1Name;
    //     Game.GameInstance.Player1.Faction = Faction1;
    //     Game.GameInstance.Player2.Name = Player2Name;
    //     Game.GameInstance.Player2.Faction = Faction2;
    // }


    // void Start()
    // {
    //     // Inicialización del juego
    // }
}

