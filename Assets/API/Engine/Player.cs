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
    public class Player
    {
        public string Name;
        public double Points;
        public Faction Faction;
        public List<Card> Graveyard;
        public List<Card> Deck;
        public Board Board;
        public List<Card> Field;
        public List<Card> Hand{get;set;}
        public int Id;
        public List<int> Gems;
        public bool AlreadyPass;

        public Player()//nuevo constructor para poder inicializar los players sin tener todos sus datos
        {
            System.Random random = new();
            Name = "DefaultName";
            Points = 0;
            Graveyard = new List<Card>();
            Hand = new List<Card>();
            Deck = new List<Card>();
            Gems = new();
            Board = new Board(this);
            Field = new List<Card>();
            Id = random.Next(0, 1000);
        }
        public List<Card> GetHand()
        {
            System.Random random = new();
            List<Card> container = new();
            for (int i = 0; i < 10; i++)
            {
                Card card = Deck[random. Next(0,Deck.Count)];
                Hand.Add(card);
                Deck.Remove(card);
            }
            
            return Hand;
        }
    }
}