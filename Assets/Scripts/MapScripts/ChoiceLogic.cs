using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceLogic : MonoBehaviour
{
    public TMP_Text text;
    public Button button;
    public MapEventData.Choices choices;

    public void OnPressed()
    {
        //Change Max Hp
        if (PlayerStatic.instance.maxHealth + choices.maxHpChange <= 0)
        {
            PlayerStatic.instance.maxHealth = 1;
        }
        else
        {
            PlayerStatic.instance.maxHealth += choices.maxHpChange;
        }
        //Change Current Hp
        if (PlayerStatic.instance.currentHealth + choices.currentHpChange <= 0)
        {
            PlayerStatic.instance.currentHealth = 1;
        }
        else
        {
            PlayerStatic.instance.currentHealth += choices.currentHpChange;
        }
        //Change Money
        if (PlayerStatic.instance.money + choices.moneyChange <= 0)
        {
            PlayerStatic.instance.money = 0;
        }
        else
        {
            PlayerStatic.instance.money += choices.moneyChange;
        }
        
        //Give cards
        if (choices.giveRandomReward)
        {
            MapGenerator.instance.rewardPending = true;
        }

        if (choices.cardReward != null)
        {
            PlayerStatic.instance.deck.Add(choices.cardReward);
        }
        
        MapEvents.instance.OnClick();
    }
}
