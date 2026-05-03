using System;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

[CreateAssetMenu(fileName = "Stats", menuName = "Scriptable Objects/Stats")]
public class Stats : ScriptableObject
{
    public List<Modifiable<float>> floatList;
    public List<Modifiable<int>> intList;
    private Dictionary<string, Modifiable<float>> floatDic;
    private Dictionary<string, Modifiable<int>> intDic;

    private void OnEnable()
    {
        floatDic = new Dictionary<string, Modifiable<float>>();
        intDic = new Dictionary<string, Modifiable<int>>();
        
        if (floatDic != null && intDic != null)
        {
            RefreshAdressing();
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
            target.modifier *= mod;
            return;
        }
        
        Debug.LogException(new Exception("did not find variable by name"));
    }

    public void AddFloatStat(string name, float mod)
    {
        Modifiable<float> target;
        if (floatDic.TryGetValue(name, out target))
        {
            target.modifier += mod;
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
            return output.value * output.modifier;
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
            return output.value * (int)output.modifier;
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
            mod.modifier = 0;
        }

        foreach (var mod in intList)
        {
            mod.modifier = 0;
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
