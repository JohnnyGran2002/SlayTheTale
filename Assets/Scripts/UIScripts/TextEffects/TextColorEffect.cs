
using UnityEngine;
using TMPro;

[System.Serializable]
public class TextColorEffect : TextEffect
{
    private Color32 _color;

    public TextColorEffect() : base(new TextRange(0, 0)) { }

    public TextColorEffect(TextRange range, Color32 color) : base(range)
    {
        this._color = color;
    }

    public override void ApplyTextEffect(TMP_Text textMesh, TMP_TextInfo textInfo, float time)
    {
        if (textInfo == null) return;
        //loop over all characters inside that range, checks each character and skips invisible ones
        for (int i = range.StartIndex; i <= range.EndIndex && i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            //Skip invisible characters (like spaces or line breaks)
            if (!charInfo.isVisible) continue;

            // The material index tells which mesh this character belongs to
            int matIndex = charInfo.materialReferenceIndex;
            int vertIndex = charInfo.vertexIndex;
            // Access the color array for this meshy
            Color32[] colors = textInfo.meshInfo[matIndex].colors32;

            // Apply the chosen color to all 4 vertices of this character’s quad
            colors[vertIndex + 0] = _color;
            colors[vertIndex + 1] = _color;
            colors[vertIndex + 2] = _color;
            colors[vertIndex + 3] = _color;
        }
    }
}
