using System.Collections.Generic;
using UnityEngine;

public class PlayerStatic : MonoBehaviour
{
    public static PlayerStatic instance;

    public int currentHealth, maxHealth, money;

    public List<CardData> deck = new List<CardData>();
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }
}
