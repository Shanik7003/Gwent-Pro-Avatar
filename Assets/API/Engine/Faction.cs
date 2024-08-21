using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Playables;
namespace Engine
{
    public class Faction
    {
        public Card leader;
        public List<Card> Deck = new();
        public Faction(List<Card> Deck, Card leader)
        {
            for (int i = 0; i < Deck.Count; i++)
            {
            this.Deck.Add(Deck[i]);   
            } 
            this.leader = leader;
        }
    }
    public class FireNation : Faction
    {
       public FireNation (List<Card> FireDeck, Card leader) : base (FireDeck, leader) {}
    }
    public class WaterTribe : Faction
    {
        public WaterTribe (List<Card> WaterDeck,Card leader) : base (WaterDeck,leader){}   
    }
    public class AirNomads : Faction
    {
        public AirNomads (List<Card> AirDeck,Card leader) : base(AirDeck,leader){}
    }
    public class EarthKingdom : Faction
    {
        public EarthKingdom(List<Card> EarthDeck, Card leader): base (EarthDeck,leader){}
    }
}