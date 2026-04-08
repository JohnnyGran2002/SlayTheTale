using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

//Creates a sine-wave vertical motion animation for a section of text
[System.Serializable]
public class TextWaveEffect : TextEffect
{
    private float amplitude;
    private float frequency;
    private float speed;

    public TextWaveEffect(TextRange range, float amplitude, float frequency, float speed) : base(range)
    {
        this.amplitude = amplitude;
        this.frequency = frequency;
        this.speed = speed;
    }

    //Moves each vertex of characters in the range up and down using a sine function(As if the text is “floating” on waves)
    public override void ApplyTextEffect(TMP_Text textMesh, TMP_TextInfo textInfo, float time)
    {
        for (int i = range.StartIndex; i <= range.EndIndex && i < textInfo.characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            int matIndex = charInfo.materialReferenceIndex;
            int vertIndex = charInfo.vertexIndex;
            Vector3[] verts = textInfo.meshInfo[matIndex].vertices;

            float offsetY = Mathf.Sin((time * speed) + i * frequency) * amplitude;

            verts[vertIndex + 0].y += offsetY;
            verts[vertIndex + 1].y += offsetY;
            verts[vertIndex + 2].y += offsetY;
            verts[vertIndex + 3].y += offsetY;
        }
    }
}
