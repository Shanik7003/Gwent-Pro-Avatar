
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


