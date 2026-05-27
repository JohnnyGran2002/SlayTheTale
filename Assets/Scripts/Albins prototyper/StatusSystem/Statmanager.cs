using System;
using System.Collections.Generic;
using UnityEngine;

public class Statmanager : MonoBehaviour
{
    [SerializeField] private Damageable dam;
    [SerializeField] private Stats stat;
    [SerializeField] private List<string> damageMultipliers;
    [SerializeField] private List<string> defenseMultipliers;
    //[SerializeField] private GameEventListener startTurnListener;
    //[SerializeField] private GameEventListener activeTurnListener;
    //[SerializeField] private GameEventListener EndTurnListener;
    private MasterStatSingleton masterStat;
    private Damageable damageable;
    private Dictionary<string, Modifiable<float>> floatDic;
    private Dictionary<string, Modifiable<int>> intDic;
    private List<Status> statusList;
    void Start()
    {
        floatDic = new Dictionary<string, Modifiable<float>>();
        intDic = new Dictionary<string, Modifiable<int>>();
        statusList = new List<Status>();
        SetupStats();
    }
    void Update()
    {
        
    }
    private void OnEnable()
    {
        dam.OnStatus += RecieveStat;
    }

    private void OnDisable()
    {
        dam.OnStatus -= RecieveStat;
    }

    public void TickStatuses(Component sender, object data)
    {
        
    }
    private void RecieveStat(Damageable damageable, Status inStat)
    {
        Debug.Log("recieved " + inStat.name);
        foreach (var status in statusList)
        {
            if (status.name == inStat.name)
            {
                status.Stack(inStat);
                return;
            }
        }
        inStat.ApplyStatus(this);
        statusList.Add(inStat);
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
            if (!intDic.TryAdd(name, new Modifiable<int>(name, stat.GetFinalInt(name))))
            {
                throw new Exception("name was somehow wrong");
            }
        }
    }
    private void RefreshInitialValues() // needs to be done soon
    {
        
    }
    
    public bool TryModifyStat(string name,float addative, float multiplicative)
    {
        Modifiable<int> bufferint;
        if (intDic.TryGetValue(name, out bufferint))
        {
            Debug.Log(bufferint.multModifier);
            bufferint.addModifier += (int)addative;
            bufferint.multModifier += multiplicative;
            Debug.Log(bufferint.multModifier);
            return true;
        }

        Modifiable<float> bufferfloat;
        if (floatDic.TryGetValue(name, out bufferfloat))
        {
            Debug.Log(bufferfloat.multModifier);
            bufferfloat.addModifier += addative;
            bufferfloat.multModifier += multiplicative;
            Debug.Log(bufferfloat.multModifier);
            return true;
        }
        return false;
    }
    
    
    
    
    
    
    public float ModifyAttack(float InitialDamage)
    {
        float add = 0;
        float mult = 0;
        float Base = 0;
        Modifiable<float> buffer;
        foreach (var name in damageMultipliers)
        {
            if (floatDic.TryGetValue(name, out buffer))
            {
                add += buffer.addModifier;
                mult += buffer.multModifier;
                Base += buffer.value;
            }
        }

        return (InitialDamage + add) * mult * Base;
    }

    public int ModifyAttack(int InitialDamage)
    {
        int add = 0;
        float mult = 0;
        int Base = 0; 
        Modifiable<int> buffer;
        foreach (var name in damageMultipliers)
        {
            if (intDic.TryGetValue(name, out buffer))
            {
                add += buffer.addModifier;
                mult += buffer.multModifier;
                Base += buffer.value;
            }
        }

        return (int)((InitialDamage + add) * mult * Base);
    }
    public float ModifyRecievedDamage(float recievedDamage)
    {
        float add = 0;
        float mult = 0;
        float Base = 0;
        Modifiable<float> buffer;
        foreach (var name in defenseMultipliers)
        {
            if (floatDic.TryGetValue(name, out buffer))
            {
                add += buffer.addModifier;
                mult += buffer.multModifier;
                Base += buffer.value;
            }
        }

        return (recievedDamage - add) / mult * Base;
    }

    public int ModifyRecievedDamage(int recievedDamage)
    {
        int add = 0;
        float mult = 0;
        int Base = 0;
        Modifiable<int> buffer;
        foreach (var nam in defenseMultipliers)
        {
            Debug.Log(defenseMultipliers[0]);
            if (intDic.TryGetValue(nam, out buffer))
            {
                Debug.Log("modified");
                add += buffer.addModifier;
                mult += buffer.multModifier;
                Base += buffer.value;
            }
        }

        return (int)((recievedDamage - add) / mult * Base);
    }
    

    
}
