using SerializeReferenceEditor;
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

[CreateAssetMenu]
public class CardData : ScriptableObject
{
    //get art sprite but not set
    [field: SerializeField] public Sprite Art { get; private set; }
    //get mana value but not set
    [field: SerializeField] public int Mana { get; private set; }
    //get description text but not set
    [field: SerializeField] public string Description {  get; private set; }
    //uses Serialize Reference Editor package because unity currently unable to serialize abstract classes
    [field: SerializeReference, SR] public List<Effect> Effects { get; private set; }
 }
