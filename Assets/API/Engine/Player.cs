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
        public Faction Faction;
        public List<Card> Graveyard;
        public List<Card> Deck;
        public Board Board;
        public List<Card> Field;
        public List<Card> Hand{get;set;}
        public int Id;
        public List<int> Gems;
        public bool AlreadyPass;
        private List<IObserver> observers = new List<IObserver>();
        private double points;
        public double Points
        {
            get => points;
            set
            {
                points = value;
                NotifyObservers(EventType.PlayerPointsChanged, this); // Notifica que los puntos del jugador han cambiado
            }
        }

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

        public void AddObserver(IObserver observer)
        {
            observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            observers.Remove(observer);
        }

        private void NotifyObservers(EventType eventType, object data)
        {
            foreach (var observer in observers)
            {
                observer.OnNotify(eventType, data);
            }
        }
        public void GetHand()
        {
            System.Random random = new();
            //List<Card> container = new();
            for (int i = 0; i < 10; i++)
            {
                Card card = Deck[random. Next(0,Deck.Count)];
               // StartCoroutine(card.MoveCard(Hand)); 
                card.MoveCard(Hand);
                // Hand.Add(card);
                // card.Ubication = Hand;
                // Deck.Remove(card);
            }
        }

       
    }
}