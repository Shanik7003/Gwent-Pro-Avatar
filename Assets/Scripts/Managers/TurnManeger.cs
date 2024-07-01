using Engine;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    public int turnCount = 0; // Contador de turnos
    // string name1 = Game.GameInstance.Player1.Name;
    // string name2 = Game.GameInstance.Player2.Name;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Opcional: Hace que el objeto no se destruya al cargar nuevas escenas.
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartTurn();
    }

    public void StartTurn()
    {
        Player currentPlayer = GetCurrentPlayer();
        // if(currentPlayer == Game.GameInstance.Player1)
        // {
        //     Debug.Log("ESTOY AQUI, QUERIENDOTE ");
        //     currentPlayer.Name = name1 + " X"; 
        //     Game.GameInstance.Player2.Name = name2;

        //     GameObject.Find("Player1").GetComponentInChildren<PlayerData>().Name = Game.GameInstance.Player1.Name;
        //     GameObject.Find("Player1").GetComponentInChildren<PlayerDisplay>().UpdatePlayer();
        //     GameObject.Find("Player2").GetComponentInChildren<PlayerData>().Name = Game.GameInstance.Player2.Name;
        //     GameObject.Find("Player2").GetComponentInChildren<PlayerDisplay>().UpdatePlayer();
        // }
        // else if (currentPlayer == Game.GameInstance.Player2)
        // {
        //     currentPlayer.Name = name2 + " X"; 
        //     Game.GameInstance.Player1.Name = name1;
        //     GameObject.Find("Player1").GetComponentInChildren<PlayerDisplay>().UpdatePlayer();
        //     GameObject.Find("Player2").GetComponentInChildren<PlayerDisplay>().UpdatePlayer();
        // }
        
    }
    public void EndTurn()
    {

        // Debug.Log($"Turno {turnCount}: Termina.");
        turnCount++; // Incrementa el contador de turnos
        StartTurn(); // Inicia el siguiente turno automáticamente
    }

    public Player GetCurrentPlayer()
    {
        // Determina el jugador actual basado en si el número de turno es par o impar
        // Debug.Log("entre al metodo GetCurrentPlayer");
        return turnCount % 2 == 0 ? Game.GameInstance.Player1 : Game.GameInstance.Player2;//La estructura es condición ? resultado_si_verdadero : resultado_si_falso. (Par:Player1)
    }
    public Player GetCurrentEnemy()
    {
        if(TurnManager.Instance.GetCurrentPlayer() == Game.GameInstance.Player1)
        {
            return Game.GameInstance.Player2;
        }
        if(TurnManager.Instance.GetCurrentPlayer() == Game.GameInstance.Player2)
        {
            return Game.GameInstance.Player1;
        }
        return Game.GameInstance.Player1;//por defecto 
    }
      public bool IsPlayerTurn(Player player)
    {
        return GetCurrentPlayer() == player;
    }
}
