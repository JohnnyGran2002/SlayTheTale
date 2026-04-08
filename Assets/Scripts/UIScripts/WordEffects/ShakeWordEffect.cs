using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShakeWordEffect : WordEffects
{
    public float amplitude = 3f;
    public float frequency = 8f;
    //Expose easy-to-edit parameters in the Inspector
    public override TextEffect CreateRuntimeEffect(TextRange range)
    {
        return new TextShakeEffect(range, amplitude, frequency);
    }
}
