using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerDisplay : MonoBehaviour
{
    public PlayerData playerData;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI points;

public void UpdatePlayer()
{
    Debug.Log(playerData.Name);
    Debug.Log(playerData.Points);
    nameText.text = playerData.Name;
    points.text = playerData.Points.ToString();
    Canvas.ForceUpdateCanvases();
}
    void Start()
    {
        UpdatePlayer();
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
//             Debug.LogError("Player data not set.");
//             return;
//         }

//         Debug.Log($"Updating display for {playerData.Name} with {playerData.Points} points.");
//         nameText.text = playerData.Name;
//         points.text = playerData.Points.ToString();
//         Canvas.ForceUpdateCanvases();
//     }

//     void Start()
//     {
//         if (nameText == null || points == null)
//         {
//             Debug.LogError("Text components are not assigned.");
//             return;
//         }
//         UpdatePlayer();
//     }
// }
