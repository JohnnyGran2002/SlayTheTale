using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CardType
{
    attack,
    spell
}

public enum Rarity
{
    common,
    uncommon,
    rare
}

[CreateAssetMenu(fileName = "New Card", menuName = "CardData", order = 1)]
public class CardData : ScriptableObject
{
    [SerializeField] private string id = Guid.NewGuid().ToString().ToUpper();
    [Header("General")] 
    [SerializeField] private string cardName = "New Card";
    [SerializeField] private string description;
    [SerializeField] private Sprite artwork;

    [Header("Gameplay")] 
    [SerializeField] private CardType cardType;
    [SerializeField] private Rarity rarity;
    [SerializeField] private int cost;

    [Header("Effects")]
    [SerializeReference] private List<Effect> effects;
    
    public string Id => id;
    public string CardName => cardName;
    public string Description => description;
    public Sprite Artwork => artwork;
    public CardType CardType => cardType;
    public Rarity Rarity => rarity;
    public int Cost => cost;
    public List<Effect> Effects => effects;

    public bool IsValid(out string error)
    {
        if (string.IsNullOrWhiteSpace(cardName))
        {
            error = "Card name is required";
            return false;
        }

        if (artwork == null)
        {
            error = "Artwork is required";
            return false;
        }

        if (cost < 0)
        {
            error = "Cost cannot be negative";
            return false;
        }
        error = null;
        return true;
    }

    // //get art sprite but not set
    // [field: SerializeField] public Sprite Art { get; private set; }
    // //get mana value but not set
    // [field: SerializeField] public int Mana { get; private set; }
    // //get description text but not set
    // [field: SerializeField] public string Description {  get; private set; }
    // //uses Serialize Reference Editor package because unity currently unable to serialize abstract classes
    // [field: SerializeReference] public List<Effect> Effects { get; private set; }
}
