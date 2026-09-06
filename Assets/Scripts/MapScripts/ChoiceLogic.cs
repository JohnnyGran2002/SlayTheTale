using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ChoiceLogic : MonoBehaviour
{
    public TMP_Text text;
    public Button button;
    public MapEventData.Choices choices;

    private LogScript _logScript;

    public void OnPressed()
    {
        var randomInt = Random.Range(0, choices.consequences.Length - 1);

        _logScript = MapGenerator.instance.log;
        
        //Change Max Hp
        if (PlayerStatic.instance.maxHealth + choices.consequences[randomInt].maxHpChange <= 0)
        {
            PlayerStatic.instance.maxHealth = 1;
            switch (choices.consequences[randomInt].maxHpChange)
            {
                case > 0:
                    _logScript.GetInfo(Result.GainedMaxHp, choices.consequences[randomInt].maxHpChange);
                    break;
                case < 0:
                    _logScript.GetInfo(Result.LostMaxHp, choices.consequences[randomInt].maxHpChange * -1);
                    break;
            }
        }
        else if (PlayerStatic.instance.maxHealth + choices.consequences[randomInt].maxHpChange != 0)
        {
            PlayerStatic.instance.maxHealth += choices.consequences[randomInt].maxHpChange;
            switch (choices.consequences[randomInt].maxHpChange)
            {
                case > 0:
                    _logScript.GetInfo(Result.GainedMaxHp, choices.consequences[randomInt].maxHpChange);
                    break;
                case < 0:
                    _logScript.GetInfo(Result.LostMaxHp, choices.consequences[randomInt].maxHpChange * -1);
                    break;
            }
        }
        //Change Current Hp
        if (PlayerStatic.instance.currentHealth + choices.consequences[randomInt].currentHpChange <= 0)
        {
            PlayerStatic.instance.currentHealth = 1;
        }
        else if (PlayerStatic.instance.currentHealth + choices.consequences[randomInt].currentHpChange != 0)
        {
            PlayerStatic.instance.currentHealth += choices.consequences[randomInt].currentHpChange;
        }
        //Change Money
        if (PlayerStatic.instance.money + choices.consequences[randomInt].moneyChange <= 0)
        {
            PlayerStatic.instance.money = 0;
            switch (choices.consequences[randomInt].moneyChange)
            {
                case > 0:
                    _logScript.GetInfo(Result.GainedGold, choices.consequences[randomInt].moneyChange);
                    break;
                case < 0:
                    _logScript.GetInfo(Result.LostGold, choices.consequences[randomInt].moneyChange * -1);
                    break;
            }
        }
        else if (PlayerStatic.instance.money + choices.consequences[randomInt].moneyChange != 0)
        {
            PlayerStatic.instance.money += choices.consequences[randomInt].moneyChange;
            switch (choices.consequences[randomInt].moneyChange)
            {
                case > 0:
                    _logScript.GetInfo(Result.GainedGold, choices.consequences[randomInt].moneyChange);
                    break;
                case < 0:
                    _logScript.GetInfo(Result.LostGold, choices.consequences[randomInt].moneyChange * -1);
                    break;
            }
        }
        
        //Give cards
        if (choices.consequences[randomInt].giveRandomReward)
        {
            MapGenerator.instance.rewardPending = true;
            _logScript.GetInfo(Result.GainedReward, 0);
        }

        if (choices.consequences[randomInt].cardReward != null)
        {
            PlayerStatic.instance.deck.Add(choices.consequences[randomInt].cardReward);
        }
        
        MapEvents.instance.OnClick();
    }
}
