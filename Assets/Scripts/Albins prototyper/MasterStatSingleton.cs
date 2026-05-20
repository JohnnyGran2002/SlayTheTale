using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "MasterStatSingleton", menuName = "Scriptable Objects/MasterStatSingleton")]
public class MasterStatSingleton : ScriptableObject
{
    public static  UnityAction<List<Modifiable<float>>, List<Modifiable<int>>> onUpdate = delegate { };
    public List<Modifiable<float>> floatList;
    public List<Modifiable<int>> intList;

    private void OnValidate()
    {
        onUpdate.Invoke(floatList,intList);
    }
}
