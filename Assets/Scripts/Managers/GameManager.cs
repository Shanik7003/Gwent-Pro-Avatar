using Engine;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public BattleField player1BattleField;
    public BattleField player2BattleField;
  
    public GameManager GameManagerInstance;
    void Start()
    {
        player1BattleField.Owner = Game.GameInstance.Player1;
        player2BattleField.Owner = Game.GameInstance.Player2;

        
    }
}
