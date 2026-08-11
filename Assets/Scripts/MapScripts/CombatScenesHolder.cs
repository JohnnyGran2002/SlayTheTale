using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CombatScenesHolder : MonoBehaviour
{
    public static CombatScenesHolder combatScenesHolder;

    public int combatCounter;
    
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
            for (var j = 0; j < tierSettings[i].enemyData.Length; j++)
            {
                var currentTier = tierSettings[i];
                
                var randomIndex = Random.Range(0, j + 1);

                (currentTier.enemyData[j], currentTier.enemyData[randomIndex]) = (currentTier.enemyData[randomIndex], currentTier.enemyData[j]);
            }
        }
    }
}
