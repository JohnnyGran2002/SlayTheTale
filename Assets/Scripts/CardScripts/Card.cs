using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    private readonly CardData data;

    //reference values that dosen't change, instead return values of data
    public string NameText => data.name;
    public string DescriptionText => data.Description;
    public Sprite CardArt => data.Art;

    //refrence values tha can be changed
    public int Mana { get; private set; }
        
    public List<Effect> Effects => data.Effects;

    public Card(CardData cardData)
    {
        data = cardData;
        Mana = cardData.Mana;
    }
}
