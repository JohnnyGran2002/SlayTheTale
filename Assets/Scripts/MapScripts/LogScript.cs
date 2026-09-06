using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum Result
{
    GainedMaxHp,
    LostMaxHp,
    GainedHp,
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
                _text = "You gained " + amount + " extra max hp!";
                PrintLog(_text);
                break;
            case Result.LostMaxHp:
                _text = "You lost " + amount + " max hp...";
                PrintLog(_text);
                break;
            case Result.GainedGold:
                _text = "You gaind " + amount + " gold!";
                PrintLog(_text);
                break;
            case Result.LostGold:
                _text = "You lost " + amount + " gold...";
                PrintLog(_text);
                break;
            case Result.GainedReward:
                _text = "You got a card!";
                PrintLog(_text);
                break;
            case Result.GainedHp:
                _text = "You gained " + amount + " hp!";
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
