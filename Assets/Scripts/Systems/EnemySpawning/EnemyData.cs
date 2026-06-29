using UnityEngine;

[CreateAssetMenu(fileName = "New Combat", menuName = "EnemyData", order = 1)]

public class EnemyData : ScriptableObject
{
    [Tooltip("Enemies will spawn left to right")]
    public GameObject[] enemies;
}
