using UnityEngine;
using Engine;
public class UIManager : MonoBehaviour
{
    public void OnEndTurnButtonClicked()
    {
        TurnManager.Instance.EndTurn();
    }
}
