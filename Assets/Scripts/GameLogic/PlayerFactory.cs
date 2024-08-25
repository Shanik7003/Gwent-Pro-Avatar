using Engine;
using UnityEngine;
public class PlayerFactory : MonoBehaviour
{
    public PlayerData CreatePlayerData(Engine.Player player)
    {
        var playerData = ScriptableObject.CreateInstance<PlayerData>();
        playerData.Name = player.Name;
        playerData.Faction =  player.Faction;
        playerData.Points = player.Points;
        // Configurar aquí más propiedades si son necesarias
        return playerData;
    }
}
