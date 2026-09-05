using UnityEngine;

[CreateAssetMenu(fileName = "New Map Event", menuName = "MapData", order = 1)]

public class MapEventData : ScriptableObject
{
    [System.Serializable]
    public struct Consequences
    {
        public CardData cardReward;
        public int maxHpChange,currentHpChange,
            moneyChange;

        public bool giveRandomReward;
    }
    
    [System.Serializable]
    public struct Choices
    {
        [TextArea] public string Text;
        public Consequences[] consequences;
    }
    
    [TextArea] public string textBox;
    public Choices[] choices;
    public Sprite image;
}