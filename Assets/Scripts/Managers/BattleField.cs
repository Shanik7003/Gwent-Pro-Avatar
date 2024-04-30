using System.Collections.Generic;
using Engine;
using UnityEngine;
public class BattleField : MonoBehaviour
{
    public Player Owner; // Owner of this battlefield(esta propiedad creo que no esta haciendo ninguna funcion aqui pero la vpy a dejar por unos metodos ahi que no se si estoy usando)
    public Owner  FieldOwner;
    [SerializeField] public BattleRow[] battleRows = new BattleRow[3]; // Filas de batalla
    [SerializeField] private Transform[] handSpaces = new Transform[1]; // Espacios para las manos de las cartas
    [SerializeField] private Transform[] deckSpaces = new Transform[1]; // Espacios para los decks
    [SerializeField] private Transform[] graveyardSpaces = new Transform[1]; // Espacios para los cementerios
    [SerializeField] private Transform[] leaderCardSpaces = new Transform[1]; // Espacios para las cartas de líder


    void Start()
    {
        // ValidateComponents();
    }
      // Método para obtener todas las cartas en el campo de batalla
    public List<Draggable> GetAllCardsOnBattleField()
    {
        List<Draggable> allCards = new List<Draggable>();
        foreach (BattleRow row in battleRows)
        {
            allCards.AddRange(row.GetCardsInRow());
        }
        return allCards;
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
