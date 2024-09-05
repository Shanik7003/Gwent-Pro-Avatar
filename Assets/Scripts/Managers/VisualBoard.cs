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



    public static Dictionary< List<Card> ,List<Transform> > UbicationsMapping { get; private set; } = new Dictionary<List<Card>, List<Transform>>();

    void Awake()
    {
        Debug.Log("YA PUSE LAS UBICACIONES EN EL DICCIONARIO:");
        UbicationsMapping = new Dictionary<List<Card>, List<Transform>>
        {
            { Game.GameInstance.Player1.Deck,            new List<Transform>{Player1Deck.transform} },
            { Game.GameInstance.Player2.Deck,            new List<Transform>{Player2Deck.transform} },
            { Game.GameInstance.Player1.Graveyard,       new List<Transform>{Player1Graveyard.transform} },
            { Game.GameInstance.Player2.Graveyard,       new List<Transform>{Player2Graveyard.transform} },
            { Game.GameInstance.Player1.Hand,            new List<Transform>{Player1Hand.transform} },
            { Game.GameInstance.Player2.Hand,            new List<Transform>{Player2Hand.transform} },
            { Game.GameInstance.Player1.Board.rows[0],   new List<Transform>{M1.transform} },
            { Game.GameInstance.Player1.Board.rows[1],   new List<Transform>{R1.transform} },
            { Game.GameInstance.Player1.Board.rows[2],   new List<Transform>{S1.transform} },
            { Game.GameInstance.Player2.Board.rows[0],   new List<Transform>{M2.transform} },
            { Game.GameInstance.Player2.Board.rows[1],   new List<Transform>{R2.transform} },
            { Game.GameInstance.Player2.Board.rows[2],   new List<Transform> {S2.transform} },

            { Game.GameInstance.Player1.Field,           new List<Transform>{ M1.transform, R1.transform, S1.transform } },
            { Game.GameInstance.Player2.Field,           new List<Transform>{ M2.transform, R2.transform, S2.transform } },
            { Game.GameInstance.Board,                   new List<Transform>{ M1.transform, R1.transform, S1.transform } },

        };

    }
}