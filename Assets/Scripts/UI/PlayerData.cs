using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Engine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "Player", menuName = "Player")]
public class PlayerData: ScriptableObject
{
    public string Name;
    public int Points;
    public Faction Faction;
    // public Board Board;
    public BattleField BattleField;
    public List<UnitCard> Hand{get;set;}
}