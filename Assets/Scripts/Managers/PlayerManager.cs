
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Engine;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
public class PlayerManager : MonoBehaviour
{
    public GameObject playerPrefab;
    private PlayerData newPlayerData;
    public GameObject Player1;
    public GameObject Player2;
    public PlayerFactory playerFactory;
    public static PlayerManager Instance;

    void Start()
    {
        GeneratePlayerData(Game.GameInstance.Player1);
        GeneratePlayer(Player1.transform );
        GeneratePlayerData(Game.GameInstance.Player2);
        GeneratePlayer(Player2.transform);
        Instance = this;
    }
    void Update()
    {
       
    }
    void GeneratePlayerData(Player player)
    {
            if (playerFactory == null)
            {
                Debug.LogError("PlayerFactory reference is not set in the inspector!");
                return;
            }
            newPlayerData = playerFactory.CreatePlayerData(player);
        
    }
    void GeneratePlayer(Transform transform)//este metodo podria ser void 
    {
            GameObject newPlayer = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, transform);
            newPlayer.transform.localPosition = Vector3.zero; // Ajusta según necesites
            Debug.LogAssertion(newPlayer.name);
            PlayerDisplay display = newPlayer.GetComponent<PlayerDisplay>();
            if (display != null && newPlayer != null)
            {
                display.playerData = newPlayerData;
                display.UpdatePlayer();
                Debug.Log("Player creado: " + newPlayerData.Name);
            }
            else
            {
                Debug.LogError("Componente PlayerDisplay o datos de player faltantes.");
            }
      }   
}

// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;
// using Engine;  // Asegúrate de que este namespace tenga todas las clases necesarias.

// public class PlayerManager : MonoBehaviour
// {
//     public GameObject playerPrefab;
//     public Transform Player1;
//     public Transform Player2;
//     public PlayerFactory playerFactory;

//     private PlayerData player1Data;
//     private PlayerData player2Data;

//     void Start()
//     {
//         // Inicialización y generación de datos para cada jugador.
//         if (playerFactory == null)
//         {
//             Debug.LogError("PlayerFactory reference is not set in the inspector!");
//             return;
//         }

//         player1Data = GeneratePlayerData(Game.GameInstance.Player1);
//         GeneratePlayer(Player1Space, player1Data);

//         player2Data = GeneratePlayerData(Game.GameInstance.Player2);
//         GeneratePlayer(Player2Space, player2Data);
//     }

//     PlayerData GeneratePlayerData(Player player)
//     {
//         PlayerData newPlayerData = playerFactory.CreatePlayerData(player);
//         if (newPlayerData != null)
//         {
//             Debug.Log($"Generated PlayerData for {newPlayerData.Name}");
//         }
//         else
//         {
//             Debug.LogError("Failed to generate PlayerData.");
//         }
//         return newPlayerData;
//     }

//     void GeneratePlayer(Transform transform, PlayerData playerData)
//     {
//         if (playerPrefab == null)
//         {
//             Debug.LogError("Player prefab is not assigned in the inspector!");
//             return;
//         }

//         GameObject newPlayer = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, transform);
//         newPlayer.transform.localPosition = Vector3.zero;
//         Debug.Log($"Player instantiated at {transform.position}");

//         PlayerDisplay display = newPlayer.GetComponent<PlayerDisplay>();
//         if (display != null && playerData != null)
//         {
//             display.playerData = playerData;
//             display.UpdatePlayer();
//             Debug.Log("Player created: " + playerData.Name);
//         }
//         else
//         {
//             Debug.LogError("PlayerDisplay component or player data is missing.");
//         }
//     }
// }
