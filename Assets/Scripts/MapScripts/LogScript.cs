using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum Result
{
    GainedMaxHp,
    LostMaxHp,
    GainedGold,
    LostGold,
    GainedReward,
}

public class LogScript : MonoBehaviour
{
    [SerializeField] private TMP_Text textBox;
    private string _text;
    
    private List<string> actionLog = new List<string>();

    public void GetInfo(Result result, int amount)
    {
        switch (result)
        {
            case Result.GainedMaxHp:
                _text = "\n" + "Player gained " + amount + " extra max hp!";
                PrintLog(_text);
                break;
            case Result.LostMaxHp:
                _text = "\n" + "Player lost " + amount + " max hp...";
                PrintLog(_text);
                break;
            case Result.GainedGold:
                _text = "\n" + "Player gaind " + amount + " gold!";
                PrintLog(_text);
                break;
            case Result.LostGold:
                _text = "\n" + "Player lost " + amount + " gold...";
                PrintLog(_text);
                break;
            case Result.GainedReward:
                _text = "\n" + "Player got a card!";
                PrintLog(_text);
                break;
        }
    }

    private void PrintLog(string text)
    {
        actionLog.Add(text);
        string logAsText = string.Join("\n", actionLog.ToArray());
        textBox.text = logAsText;
    }
}
