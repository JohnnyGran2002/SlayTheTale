using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class RewardCanvas : MonoBehaviour
{
    //public static RewardCanvas instance;
    
    [FormerlySerializedAs("spaceBetweenChoices")] [Header("Settings"), Space(7), SerializeField]
    private float spaceBetweenRewards = 20;

    [SerializeField] private RewardButton[] buttons;
    //[SerializeField] private GameObject rewardPrefab;

    private int _amountOfCards;
    
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(!MapGenerator.instance.rewardPending) return;
        MapGenerator.instance.rewardPending = false;
        DoCardReward();
    }

    [ContextMenu("DoReward")]

    public void DoCardReward()
    {
        gameObject.SetActive(true);
        
        List<CardData> currentCards = GetCards();

        for (var i = 0; i < buttons.Length; i++)
        {
            buttons[i].cost.text = currentCards[i].Cost.ToString();
            buttons[i].name.text = currentCards[i].CardName;
            buttons[i].description.text = currentCards[i].Description;
            //buttons[i].type.text = currentCards[i].CardType.ToString();
            buttons[i].artwork.sprite = currentCards[i].Artwork;
        }
    }

    private List<CardData> GetCards()
    {
        List<CardData> chosenCards = new List<CardData>();
        
        while (chosenCards.Count < buttons.Length)
        {
            var rarity = GetRarity();

            var matchingCards = CombatScenesHolder.instance.cardPool.Where(card => card.Rarity == rarity && !chosenCards.Contains(card)).ToList();

            if (matchingCards.Count == 0)
            {
                matchingCards = CombatScenesHolder.instance.cardPool.Where(card => !chosenCards.Contains(card)).ToList();
            }

            if (matchingCards.Count > 0)
            {
                var randomIndex = Random.Range(0, matchingCards.Count);
                chosenCards.Add(matchingCards[randomIndex]);
            }
        }
        
        return chosenCards;
    }

    private Rarity GetRarity()
    {
        var randomRange = Random.Range(0f, 100f);

        if (randomRange < CombatScenesHolder.instance.commonOdds) return Rarity.Common;
        
        if (randomRange < CombatScenesHolder.instance.commonOdds + CombatScenesHolder.instance.uncommonOdds) return Rarity.Uncommon;
        
        return Rarity.Rare;
    }

    
} 

