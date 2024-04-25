using Engine;
using UnityEngine;
public class CardFactory : MonoBehaviour
{
    public CardData CreateCardData(Engine.Card card)
    {
        var cardData = ScriptableObject.CreateInstance<CardData>();
        cardData.cardName = card.name;
        cardData.description = card.description;
        cardData.points = card.points;
        cardData.owner = card.player;
        cardData.Card = card; // que cada cardData sepa quien es su carta gemela en el engine
        // Configurar aquí más propiedades si son necesarias
         cardData.cardImage = Resources.Load<Sprite>("CardImages/" + card.name.Replace(" ", "_"));
        return cardData;
    }
}
