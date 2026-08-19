using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class RewardCanvas : MonoBehaviour
{
    public static RewardCanvas instance;
    
    [FormerlySerializedAs("spaceBetweenChoices")] [Header("Settings"), Space(7), SerializeField]
    private float spaceBetweenRewards = 20;

    [SerializeField] private Transform target;
    [SerializeField] private GameObject rewardPrefab;

    private int _amountOfCards;

    public void DoCardReward()
    {
        /*for (var i = 0; i < _amountOfCards; i++)
        {
            var currentReward = Instantiate(rewardPrefab, new Vector3(target.position.x, target.position.y + (i * -spaceBetweenRewards), choiceTarget.transform.position.z), Quaternion.identity, this.transform);
            currentReward.SetActive(true);
            var currentLogic = currentReward.GetComponent<ChoiceLogic>();
        }*/
    }

    public List<CardData> GetCards()
    {
        List<CardData> chosenCards = new List<CardData>();

        var rarity = GetRarity();

        var matchingCards = PlayerStatic.i.deck.Where(card => card.Rarity == rarity).ToList();

        if (matchingCards.Count == 0 && rarity != Rarity.Common)
        {
            matchingCards = PlayerStatic.i.deck.Where(card => card.Rarity == Rarity.Common).ToList();
        }

        if (matchingCards.Count > 0)
        {
            var randomIndex = Random.Range(0, matchingCards.Count);
            chosenCards.Add(matchingCards[randomIndex]);
        }

        return chosenCards;
    }

    private Rarity GetRarity()
    {
        var randomRange = Random.Range(0f, 100f);

        if (randomRange < CombatScenesHolder.i.commonOdds) return Rarity.Common;
        
        if (randomRange < CombatScenesHolder.i.commonOdds + CombatScenesHolder.i.uncommonOdds) return Rarity.Uncommon;
        
        return Rarity.Rare;
    }

    
} 

