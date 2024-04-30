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
        // ValidateComponents();
    }

//     private void ValidateComponents()
// {
//     bool allComponentsValid = true;
//     foreach (var row in battleRows)
//         if (row == null) { allComponentsValid = false; break; }
//     foreach (var space in handSpaces)
//         if (space == null) { allComponentsValid = false; break; }
//     foreach (var deck in deckSpaces)
//         if (deck == null) { allComponentsValid = false; break; }
//     foreach (var graveyard in graveyardSpaces)
//         if (graveyard == null) { allComponentsValid = false; break; }
//     foreach (var leader in leaderCardSpaces)
//         if (leader == null) { allComponentsValid = false; break; }

//     // Debug.Log(allComponentsValid ? "All components set up correctly!" : "Component setup error: Check your board assignments.");
// }
}
