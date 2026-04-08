using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveWordEffect : WordEffects
{
    public float amplitude = 3f;
    public float wavelength = 1f;
    public float speed = 3f;
    //Expose easy-to-edit parameters in the Inspector
    public override TextEffect CreateRuntimeEffect(TextRange range)
    {
        return new TextWaveEffect(range, amplitude, wavelength, speed);
    }
}
