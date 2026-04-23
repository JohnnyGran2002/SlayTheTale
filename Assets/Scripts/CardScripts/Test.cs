using UnityEngine;
[CreateAssetMenu(fileName = "New Card", menuName = "Test", order = 1)] // switch "Test" to "Card" later
public class Test : ScriptableObject
{
    public Rarity rarity;
    public CardType cardType;
    public Sprite artWork;
    public Sprite icon;
    public string Name;
    public string description;
    public int cost;
}
