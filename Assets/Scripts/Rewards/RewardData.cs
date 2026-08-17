using UnityEngine;

[CreateAssetMenu(fileName = "New Reward", menuName = "RewardData", order = 1)]

public class RewardData : ScriptableObject
{
    [Tooltip("Cards that will be given to the player")]public CardData[] card;
    public int money, maxHp;
}
