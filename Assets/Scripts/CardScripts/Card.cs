using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    // public CardData Data {get; private set;}
    //
    // public bool IsPlayed {get; set;}
    // public int CurrentCost {get; set;}
    //
    // public Card(CardData data)
    // {
    //     Data = data;
    //     CurrentCost = data.Cost;
    // }
    
    private readonly CardData data;

    //reference values that dosen't change, instead return values of data
    public string NameText => data.CardName;
    public string DescriptionText => data.Description;
    public Sprite CardArt => data.Artwork;
    public Sprite Background => data.Background;
    public Sprite Shadow => data.Shadow;
    public CardType CardType => data.CardType;
    public Rarity Rarity => data.Rarity;
    public CardElement Element => data.Element;
    public Attack.AreaType AreaType => data.AreaType;

    //refrence values tha can be changed
    public int Mana { get; private set; }

    public Card(CardData cardData)
    {
        data = cardData;
        Mana = cardData.Cost;
    }
}
