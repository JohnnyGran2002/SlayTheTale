using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardButton : MonoBehaviour
{
    public TMP_Text cost, name, description, type;
    public Image artwork;
    public CardData card;

    public void GiveCard()
    {
        PlayerStatic.instance.deck.Add(card);
        Debug.Log("Player now has "+ card.ToString());
    }
}
