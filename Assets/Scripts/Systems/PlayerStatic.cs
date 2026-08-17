using System.Collections.Generic;
using UnityEngine;

public class PlayerStatic : MonoBehaviour
{
    public static PlayerStatic i;

    public int currentHealth, maxHealth, money;

    public List<CardData> deck = new List<CardData>();
    
    private void Awake()
    {
        if (i != null && i != this)
        {
            Destroy(this);
        }
        else
        {
            i = this;
            DontDestroyOnLoad(this);
        }
    }
}
