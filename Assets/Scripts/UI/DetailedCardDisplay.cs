using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DetailedCardDisplay : MonoBehaviour
{
    public Image artworkImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI pointsText;

    public void UpdateDisplay(CardData cardData)
    {
        artworkImage.sprite = cardData.cardImage;
        nameText.text = cardData.cardName;
        descriptionText.text = cardData.description;
        pointsText.text = cardData.points.ToString();
    }
}
