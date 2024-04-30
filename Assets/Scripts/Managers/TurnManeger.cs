using Engine;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    public int turnCount = 0; // Contador de turnos

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
        // Debug.Log($"Turno {turnCount}: Empieza.");
        Player currentPlayer = GetCurrentPlayer();
        // Debug.Log($"Es el turno de {currentPlayer.Name}.");

        // Aquí puedes añadir cualquier lógica de inicio de turno, como robar cartas, etc.
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
