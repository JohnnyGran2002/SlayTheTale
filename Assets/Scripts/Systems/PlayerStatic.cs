using System.Collections.Generic;
using UnityEngine;

public class PlayerStatic : MonoBehaviour
{
    public static PlayerStatic playerStatic;

    public int currentHealth, maxHealth;

    public List<CardData> deck = new List<CardData>();
    
    private void Awake()
    {
        if (playerStatic != null && playerStatic != this)
        {
            Destroy(this);
        }
        else
        {
            playerStatic = this;
            DontDestroyOnLoad(this);
        }
    }
}
