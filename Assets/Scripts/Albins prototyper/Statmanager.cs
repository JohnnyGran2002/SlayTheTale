using System;
using System.Collections.Generic;
using UnityEngine;

public class Statmanager : MonoBehaviour
{
    [SerializeField] private Stats stat;
    [SerializeField] private List<string> damageMultipliers;
    [SerializeField] private List<string> defenseMultipliers;
    private MasterStatSingleton masterStat;
    private Damageable damageable;
    private Dictionary<string, Modifiable<float>> floatDic;
    private Dictionary<string, Modifiable<int>> intDic;
    void Start()
    {
        floatDic = new Dictionary<string, Modifiable<float>>();
        intDic = new Dictionary<string, Modifiable<int>>();
    }

    private void SetupStats()
    {
        
        List<string> listBufFloat =  stat.GetFLoatNames();
        foreach (var name in listBufFloat)
        {
            if (!floatDic.TryAdd(name, new Modifiable<float>(name, stat.GetFinalFloat(name))))
            {
                throw new Exception("name was somehow wrong");
            }
        }

        List<string> listBuffInt = stat.GetIntNames();
        foreach (var name in listBuffInt)
        {
            if (intDic.TryAdd(name, new Modifiable<int>(name, stat.GetFinalInt(name))))
            {
                throw new Exception("name was somehow wrong");
            }
        }
    }
    private void RefreshInitialValues()
    {
        
    }
    public float ModifyAttack(float InitialDamage)
    {
        float add = 0;
        float mult = 0;
        Modifiable<float> buffer;
        foreach (var name in damageMultipliers)
        {
            if (floatDic.TryGetValue(name, out buffer))
            {
                add += buffer.addModifier;
                mult += buffer.multModifier;
            }
        }

        return (InitialDamage + add) * mult;
    }

    public int ModifyAttack(int InitialDamage)
    {
        int add = 0;
        float mult = 0;
        Modifiable<int> buffer;
        foreach (var name in damageMultipliers)
        {
            if (intDic.TryGetValue(name, out buffer))
            {
                add += buffer.addModifier;
                mult += buffer.multModifier;
            }
        }

        return (int)((InitialDamage + add) * mult);
    }
    public float GetDefenseMod()
    {
        return 0;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
