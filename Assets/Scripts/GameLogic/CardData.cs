using Engine;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardData : ScriptableObject
{
    public string cardName;
    public string description;
    public int points;
    public Sprite cardImage;
    public Player owner;
    public Card Card;
    
}
