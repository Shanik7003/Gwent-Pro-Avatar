using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Engine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "Player", menuName = "Player")]
public class PlayerData: ScriptableObject
{
    public Player player;
    public string Name;
    public double Points;
    public Faction Faction;

    public BattleField BattleField;
    public List<Card> Hand{get;set;}
}