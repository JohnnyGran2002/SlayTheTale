using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RewardCanvas : MonoBehaviour
{
    public static RewardCanvas i;

    [SerializeField] private Transform target;
    [SerializeField] private GameObject rewardPrefab;

    private int _amountOfCards;

    public void DoCardReward()
    {
        
    }

    /*public List<CardData> GetCards()
    {
        List<CardData> chosenCards = new List<CardData>();
        
        for (var i = 0; i < _amountOfCards; i++)
        {
            var rarity = GetRarity();
            
        }
    }*/

    private Rarity GetRarity()
    {
        var randomIndex = Random.Range(0f, 100f);

        if (randomIndex < CombatScenesHolder.i.commonOdds)
        {
            return Rarity.Common;
        }
        else if (randomIndex < CombatScenesHolder.i.uncommonOdds)
        {
            return Rarity.Uncommon;
        }
        else
        {
            return Rarity.Rare;
        }
    }

    /*private CardData GetCard(Rarity rarity)
    {
        List<CardData> 
    }*/
} 

