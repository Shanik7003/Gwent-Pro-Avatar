using System.Collections.Generic;
using Engine;
using UnityEngine;
public class BoardManager : MonoBehaviour
{
    [SerializeField] private Transform[] battleRows = new Transform[6]; // Filas de batalla
    [SerializeField] private Transform[] handSpaces = new Transform[2]; // Espacios para las manos de las cartas
    [SerializeField] private Transform[] deckSpaces = new Transform[2]; // Espacios para los decks
    [SerializeField] private Transform[] graveyardSpaces = new Transform[2]; // Espacios para los cementerios
    [SerializeField] private Transform[] leaderCardSpaces = new Transform[2]; // Espacios para las cartas de líder

    void Start()
    {
       
    }
}
