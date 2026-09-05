using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CombatScenesHolder : MonoBehaviour
{
    public static CombatScenesHolder instance;

    
    
    [Serializable]
    public struct IntRange
    { 
        public int from; 
        public int to;
    }
    
    [Serializable]
    public struct ColumnSettings
    {
        public IntRange tierRange;
        public IntRange moneyRewardRange;
        public EnemyData[] enemyData;
        public string[] scenesToUse;
        public MapEventData[] mapEventData;
        [HideInInspector] public int combatCounter, eventCounter;
    }

    public CardData[] cardPool;
    
    [Tooltip("Must equal 100"), Range(0,100)]
    public float commonOdds = 60, uncommonOdds = 37, rareOdds = 3;
    
    [Header("Settings per Tier")]
    public ColumnSettings[] tierSettings;

    public string bossScene;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        for (var i = 0; i < tierSettings.Length; i++)
        {
            var currentTier = tierSettings[i];
            
            for (var j = 0; j < tierSettings[i].enemyData.Length; j++)
            {
                var randomEnemyIndex = Random.Range(0, j + 1);

                (currentTier.enemyData[j], currentTier.enemyData[randomEnemyIndex]) = (currentTier.enemyData[randomEnemyIndex], currentTier.enemyData[j]);
            }

            for (var j = 0; j < tierSettings[i].mapEventData.Length; j++)
            {
                var randomEventIndex = Random.Range(0, j + 1);

                (currentTier.mapEventData[j], currentTier.mapEventData[randomEventIndex]) = (currentTier.mapEventData[randomEventIndex], currentTier.mapEventData[j]);
            }
        }
    }
}
