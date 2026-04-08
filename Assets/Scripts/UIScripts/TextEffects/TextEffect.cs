using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.TextCore.Text;

//Base class that defines the common structure for all runtime effects(the “interface” for all actual effects)
[System.Serializable]
public abstract class TextEffect
{
    //to know which characters to affect
    protected TextRange range;

    protected TextEffect(TextRange range)
    {
        this.range = range;
    }

    //each effect implements its own visual effect
    public abstract void ApplyTextEffect(TMP_Text textMesh, TMP_TextInfo textInfo, float time);
}
