using Engine;
using UnityEngine;
public class CardFactory : MonoBehaviour
{
    public CardData CreateCardData(Engine.Card card)
    {
        var cardData = ScriptableObject.CreateInstance<CardData>();
        cardData.cardName = card.name;
        cardData.points = card.points;
        cardData.owner = card.player;
        cardData.Card = card; // que cada cardData sepa quien es su carta gemela en el engine
        // Configurar aquí más propiedades si son necesarias
        if (cardData.Card.hability == Habilities.Personalized)//*!si es una carta creada por el usuario
        {
            cardData.cardImage = Resources.Load<Sprite>("CardImages/cardImage");
            cardData.description = "Carta construida por el usuario con mucha creatividad a base de codigo :) ";
            return cardData;
        }
        cardData.description = card.description;
        cardData.cardImage = Resources.Load<Sprite>("CardImages/" + card.name.Replace(" ", "_"));
        return cardData;
    }
}
