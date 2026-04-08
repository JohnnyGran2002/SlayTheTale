using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Defines configurable effect templates for use in the Inspector, doesn’t directly modify text
[System.Serializable]
public abstract class WordEffects
{
    public abstract TextEffect CreateRuntimeEffect(TextRange range);
}
