using UnityEngine;
using TMPro;

//Adds a random jitter/shake motion to characters
[System.Serializable]
public class TextShakeEffect : TextEffect
{
    private float intensity;
    private float speed;

    public TextShakeEffect(TextRange range, float intensity, float speed) : base(range)
    {
        this.intensity = intensity;
        this.speed = speed;
    }

    //Makes the characters in range look like they’re shaking or trembling(Like vibrating text when something explodes or is emphasized)
    public override void ApplyTextEffect(TMP_Text textMesh, TMP_TextInfo textInfo, float time)
    {
        for (int i = range.StartIndex; i <= range.EndIndex && i < textInfo.characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            int matIndex = charInfo.materialReferenceIndex;
            int vertIndex = charInfo.vertexIndex;
            Vector3[] verts = textInfo.meshInfo[matIndex].vertices;

            //Uses Perlin noise to offset vertices in X and Y directions
            float noiseX = Mathf.PerlinNoise(i * 0.1f, time * speed) * 2f - 1f;
            float noiseY = Mathf.PerlinNoise(i * 0.1f + 5f, time * speed) * 2f - 1f;

            Vector3 offset = new Vector3(noiseX, noiseY, 0f) * intensity;

            verts[vertIndex + 0] += offset;
            verts[vertIndex + 1] += offset;
            verts[vertIndex + 2] += offset;
            verts[vertIndex + 3] += offset;
        }
    }
}
