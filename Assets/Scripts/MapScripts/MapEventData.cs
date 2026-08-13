using UnityEngine;

[CreateAssetMenu(fileName = "New Map Event", menuName = "MapData", order = 1)]

public class MapEventData : ScriptableObject
{
    [System.Serializable]
    public struct Choices
    {
        public string Text;
        public RewardData Reward;
    }
    
    public string textBox;
    public Choices[] choices;
    public Sprite image;
}