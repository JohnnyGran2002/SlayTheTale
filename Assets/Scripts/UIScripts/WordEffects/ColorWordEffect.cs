using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorWordEffect : WordEffects
{
    public Color color = Color.white;
    //Expose easy-to-edit parameters in the Inspector
    public override TextEffect CreateRuntimeEffect(TextRange range)
    {
        return new TextColorEffect(range, color);
    }
}
