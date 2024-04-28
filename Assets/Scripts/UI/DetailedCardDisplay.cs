using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Engine;

public class DetailedCardDisplay : MonoBehaviour
{
    public Image artworkImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI position;

    public void UpdateDisplay(CardData cardData)
    {
        artworkImage.sprite = cardData.cardImage;
        nameText.text = cardData.cardName;
        descriptionText.text = cardData.description;
        pointsText.text = cardData.points.ToString();
        position.text = cardData.Card.position.ToString();
    }
}
