using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CombatScenesHolder : MonoBehaviour
{
    public static CombatScenesHolder combatScenesHolder;

    public int combatCounter, eventCounter;
    
    [System.Serializable]
    public struct TierRange
    { 
        public int from; 
        public int to;
    }
    
    [System.Serializable]
    public struct ColumnSettings
    {
        public EnemyData[] enemyData;
        public string[] scenesToUse;
        public MapEventData[] mapEventData;
        public TierRange tierRange;
    }
    
    public ColumnSettings[] tierSettings;
    
    private void Awake()
    {
        if (combatScenesHolder != null && combatScenesHolder != this)
        {
            Destroy(this);
        }
        else
        {
            combatScenesHolder = this;
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
