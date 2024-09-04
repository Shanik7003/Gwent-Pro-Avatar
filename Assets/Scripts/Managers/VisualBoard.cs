using System;
using UnityEngine;
using Unity;
using UnityEngine.UI;
using System.Collections.Generic;
using Engine;

public class VisualBoard : MonoBehaviour
{
    public GameObject Player1Deck ;
    public  GameObject Player2Deck;
    public  GameObject Player1Graveyard;
    public  GameObject Player2Graveyard;
    public  GameObject Player1Hand;
    public  GameObject Player2Hand;
    public GameObject M1;
    public GameObject R1;
    public GameObject S1;
    public GameObject M2;
    public GameObject R2;
    public GameObject S2;



    public static Dictionary< List<Card> ,Transform > UbicationsMapping { get; private set; }= new Dictionary<List<Card>, Transform>();

    void Start()
    {
        UbicationsMapping = new Dictionary<List<Card>, Transform>
        {
            { Game.GameInstance.Player1.Deck, Player1Deck.transform },
            { Game.GameInstance.Player2.Deck, Player2Deck.transform },
            { Game.GameInstance.Player1.Graveyard, Player1Graveyard.transform },
            { Game.GameInstance.Player2.Graveyard, Player2Graveyard.transform },
            { Game.GameInstance.Player1.Hand, Player1Hand.transform },
            { Game.GameInstance.Player2.Hand, Player2Hand.transform },
            {Game.GameInstance.Player1.Board.rows[0],M1.transform},
            {Game.GameInstance.Player1.Board.rows[1],R1.transform},
            {Game.GameInstance.Player1.Board.rows[2],S1.transform},
            {Game.GameInstance.Player2.Board.rows[0],M2.transform},
            {Game.GameInstance.Player2.Board.rows[1],R2.transform},
            {Game.GameInstance.Player2.Board.rows[2],S2.transform},

        };

    }
}