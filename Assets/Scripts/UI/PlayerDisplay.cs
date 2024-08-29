using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Engine;

public class PlayerDisplay : MonoBehaviour, IObserver
{
    public PlayerData playerData;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI points;
    public Player player;

    public void OnNotify(Engine.EventType eventType, object data)
    {
        if (eventType == Engine.EventType.PlayerPointsChanged)
        {
            UpdatePlayerPoints(); // Actualiza la UI con los nuevos puntos del jugador
        }
    }

    void Start()
    {
        UpdatePlayer();
        player.AddObserver(this);
    }
    private void OnDestroy()
    {
        player.RemoveObserver(this);
    }
    public void UpdatePlayer()
    {
        //Debug.Log(playerData.Name);
        //Debug.Log(playerData.Points);
        nameText.text = playerData.Name;
        points.text = playerData.Points.ToString();
        player = playerData.player;
        Canvas.ForceUpdateCanvases();
    }

    public void UpdatePlayerPoints()
    {
        points.text = player.Points.ToString();
    }
}
// using UnityEngine;
// using TMPro;

// public class PlayerDisplay : MonoBehaviour
// {
//     public PlayerData playerData;
//     public TextMeshProUGUI nameText;
//     public TextMeshProUGUI points;

//     public void UpdatePlayer()
//     {
//         if (playerData == null)
//         {
//             //Debug.LogError("Player data not set.");
//             return;
//         }

//         //Debug.Log($"Updating display for {playerData.Name} with {playerData.Points} points.");
//         nameText.text = playerData.Name;
//         points.text = playerData.Points.ToString();
//         Canvas.ForceUpdateCanvases();
//     }

//     void Start()
//     {
//         if (nameText == null || points == null)
//         {
//             //Debug.LogError("Text components are not assigned.");
//             return;
//         }
//         UpdatePlayer();
//     }
// }
