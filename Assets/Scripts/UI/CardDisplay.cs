using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public CardData cardData;
    public Image artworkImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI points;

public void UpdateCard()
{
    nameText.text = cardData.cardName;
    descriptionText.text = cardData.description;
    points.text = cardData.points.ToString();
    artworkImage.sprite = cardData.cardImage; 
}
    void Start()
    {
        UpdateCard();
    }
}
