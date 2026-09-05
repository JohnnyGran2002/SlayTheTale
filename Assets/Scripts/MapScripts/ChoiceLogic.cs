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
        var randomInt = Random.Range(0, choices.consequences.Length - 1);
        
        //Change Max Hp
        if (PlayerStatic.instance.maxHealth + choices.consequences[randomInt].maxHpChange <= 0)
        {
            PlayerStatic.instance.maxHealth = 1;
        }
        else
        {
            PlayerStatic.instance.maxHealth += choices.consequences[randomInt].maxHpChange;
        }
        //Change Current Hp
        if (PlayerStatic.instance.currentHealth + choices.consequences[randomInt].currentHpChange <= 0)
        {
            PlayerStatic.instance.currentHealth = 1;
        }
        else
        {
            PlayerStatic.instance.currentHealth += choices.consequences[randomInt].currentHpChange;
        }
        //Change Money
        if (PlayerStatic.instance.money + choices.consequences[randomInt].moneyChange <= 0)
        {
            PlayerStatic.instance.money = 0;
        }
        else
        {
            PlayerStatic.instance.money += choices.consequences[randomInt].moneyChange;
        }
        
        //Give cards
        if (choices.consequences[randomInt].giveRandomReward)
        {
            MapGenerator.instance.rewardPending = true;
        }

        if (choices.consequences[randomInt].cardReward != null)
        {
            PlayerStatic.instance.deck.Add(choices.consequences[randomInt].cardReward);
        }
        
        MapEvents.instance.OnClick();
    }
}
