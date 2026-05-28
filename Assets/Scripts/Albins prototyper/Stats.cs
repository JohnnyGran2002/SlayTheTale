using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Stats", menuName = "Scriptable Objects/Stats")]
public class Stats : ScriptableObject
{
    public List<Modifiable<float>> floatList;
    public List<Modifiable<int>> intList;
    //[DoNotSerialize] public Dictionary<string, Modifiable<float>> floatDic { get; private set; }
    private Dictionary<string, Modifiable<float>> floatDic;
    //[DoNotSerialize] public Dictionary<string, Modifiable<int>> intDic { get; private set; }
    private Dictionary<string, Modifiable<int>> intDic;

    private void OnEnable()
    {
        floatDic = new Dictionary<string, Modifiable<float>>();
        intDic = new Dictionary<string, Modifiable<int>>();
        MasterStatSingleton.onUpdate += UpdateLists;
        if (floatDic != null && intDic != null)
        {
            RefreshAdressing();
        }
    }

    private void OnDisable()
    {
        MasterStatSingleton.onUpdate -= UpdateLists;
    }

    private void UpdateLists(List<Modifiable<float>> floatL, List<Modifiable<int>> intL) //add so it adds to the dictionary first then list
    {
        List<string> names = new List<string>();
        foreach (var floatMod in floatL)
        {
            names.Add(floatMod.name);
            if (!floatDic.TryGetValue(floatMod.name, out _))
            {
                floatList.Add(new Modifiable<float>(floatMod.name,floatMod.value,floatMod.addModifier,floatMod.multModifier));
            }
             
        }
/*
        foreach (var pair in floatDic)
        {
            
        }
        */
        foreach (var intMod in intL)
        {
            if (intDic.TryGetValue(intMod.name, out _))
            {
                intList.Add(new Modifiable<int>(intMod.name,intMod.value,intMod.addModifier,intMod.multModifier)); 
            }
        }
        
        
        if (floatDic != null && intDic != null)
        {
            RefreshAdressing();
        }
        else
        {
         // i really should schedule another update but the liklyhood of this being a problem is so small i don't feel it is necessary **yet**    
        }
    }
    
    private void OnValidate()
    {
        if (floatDic != null && intDic != null)
        {
            RefreshAdressing();
        }
        
    }
    
    /// <summary>
    /// returns a given modifiable.
    /// </summary>
    /// <param name="name">the name of the stat.</param>
    /// <returns>the modifiable object of type float.</returns>
    public Modifiable<float> GetFloatModifiable(string name)
    {
        Modifiable<float> output;
        if (floatDic.TryGetValue(name, out output))
        {
            return output;
        }

        Debug.LogException(new Exception("did not find variable by name"));
        return null;
    }
    
    /// <summary>
    /// returns a given modifiable.
    /// </summary>
    /// <param name="name">the name of the stat.</param>
    /// <returns>the modifiable object of type float.</returns>
    public Modifiable<int> GetIntModifiable(string name)
    {
        Modifiable<int> output;
        if (intDic.TryGetValue(name, out output))
        {
            return output;
        }

        Debug.LogException(new Exception("did not find variable by name"));
        return null;
    }
    
    /// <summary>
    /// returns a given value before any modifier is added.
    /// </summary>
    /// <param name="name">the name of the stat.</param>
    /// <returns>value before modifier is added.</returns>
    public float Getinitialfloat(string name)
    {
        Modifiable<float> output;
        if (floatDic.TryGetValue(name, out output))
        {
            return output.value;
        }

        Debug.LogException(new Exception("did not find variable by name"));
        return 0;
    }
    
    /// <summary>
    /// returns a given value before any modifier is added.
    /// </summary>
    /// <param name="name">the name of the stat.</param>
    /// <returns>value before modifier is added.</returns>
    public int Getinitialint(string name)
    {
        Modifiable<int> output;
        if (intDic.TryGetValue(name, out output))
        {
            return output.value;
        }

        Debug.LogException(new Exception("did not find variable by name"));
        return 0;
    }

    public void MultiplyFloatStat(string name, float mod)
    {
        Modifiable<float> target;
        if (floatDic.TryGetValue(name, out target))
        {
            target.multModifier *= mod;
            return;
        }
        
        Debug.LogException(new Exception("did not find variable by name"));
    }

    public void AddFloatStat(string name, float mod)
    {
        Modifiable<float> target;
        if (floatDic.TryGetValue(name, out target))
        {
            target.addModifier += mod;
            return;
        }
        
        Debug.LogException(new Exception("did not find variable by name"));
    }
    
    /// <summary>
    /// returns a list of all float stat names.
    /// </summary>
    /// <returns>list of names.</returns>
    public List<string> GetFLoatNames()
    {
        List<string>  output= new List<string>();
        foreach (var mod in floatList)
        {
            output.Add(mod.name);
        }
        
        return output;
    }
    
    /// <summary>
    /// returns a list of all int stat names.
    /// </summary>
    /// <returns>list of names.</returns>
    public List<string> GetIntNames()
    {
        List<string>  output= new List<string>();
        foreach (var mod in intList)
        {
            output.Add(mod.name);
        }

        return output;
    }
    
    /// <summary>
    /// returns a given value after modifier is added.
    /// </summary>
    /// <param name="name">the name of the stat.</param>
    /// <returns>value after modifier is added.</returns>
    public float GetFinalFloat(string name)
    {
        Modifiable<float> output;
        if (floatDic.TryGetValue(name, out output))
        {
            return (output.value + output.addModifier) * output.multModifier;
        }
        Debug.LogException(new Exception("did not find variable by name"));
        return 0;
    }
    /// <summary>
    /// returns a given value after modifier is added.
    /// </summary>
    /// <param name="name">the name of the stat.</param>
    /// <returns>value after modifier is added.</returns>
    public int GetFinalInt(string name)
    {
        Modifiable<int> output;
        if (intDic.TryGetValue(name, out output))
        {
            return (int)((output.value + output.addModifier) * output.multModifier);
        }
        
        return 0;
    }

    /// <summary>
    /// clears all modifiers should only be done when run is over.
    /// </summary>
    public void ResetModifiers()
    {
        foreach (var mod in floatList)
        {
            mod.addModifier = 0;
            mod.multModifier = 0;
        }

        foreach (var mod in intList)
        {
            mod.addModifier = 0;
            mod.multModifier = 0;
        }
    }
    public void RefreshAdressing()
    {
        floatDic.Clear();
        intDic.Clear();
        foreach (Modifiable<float> mod in floatList)
        {
            floatDic.TryAdd(mod.name, mod);
        }
        foreach (Modifiable<int> mod in intList)
        {
            intDic.TryAdd(mod.name, mod);
        }
    }
}
