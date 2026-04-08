using SerializeReferenceEditor;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using static UnityEngine.UI.Image;

[ExecuteAlways]
public class TextEffectTool : MonoBehaviour
{
    [SerializeField] private TMP_Text textMeshPro;

    // The raw text shown in TMP (no TMP markup here)
    [TextArea(3, 10)] public string plainText;
    //list of words, each word can have one or more effects
    public List<WordEffectEditor> wordEffects = new List<WordEffectEditor>();
    //list of active effects being applied every frame
    private readonly List<TextEffect> activeEffects = new List<TextEffect>();
    //saves the original vertex data of the text(able to reset it before reapplying effects)
    private TMP_MeshInfo[] originalMeshInfo;
    //to track when text or effects change
    private string lastText;
    private int lastEffectHash;

    private void OnEnable()
    {
        if (textMeshPro == null)
        {
            Debug.LogWarning("Please reference a TextMeshPro in " + gameObject.name);
        }


        if (textMeshPro != null)
        {
            textMeshPro.text = plainText;
            //forces TMP to rebuild its geometry
            textMeshPro.ForceMeshUpdate();
            //save the original shape
            originalMeshInfo = textMeshPro.textInfo.CopyMeshInfoVertexData();
            //apply all word-based effects
            ApplyConfiguredEffects();
        }


#if UNITY_EDITOR
        //update effects in edit mode so they can be previewed live
        EditorApplication.update += EditorUpdate;
#endif
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        //clean up when disabled (important for editor updates)
        EditorApplication.update -= EditorUpdate;
#endif
    }

    private void Update()
    {
        //only animate in play mode — edit mode uses a separate function
        if (Application.isPlaying)
        {
            ApplyEffects(Time.time);
        }
    }

#if UNITY_EDITOR
    //called constantly while in edit mode(keeps animations moving and rebuilds effects when settings gets edited)
    private void EditorUpdate()
    {
        if (Application.isPlaying || textMeshPro == null) return;
        //check if anything in text or effect changed
        int currentEffectHash = ComputeEffectHash();

        //rebuild everything when text or effects are modified
        if (plainText != lastText || currentEffectHash != lastEffectHash)
        {
            ApplyConfiguredEffects();
            lastText = plainText;
            lastEffectHash = currentEffectHash;
        }

        //Animate effects in editor using editor time
        ApplyEffects((float)EditorApplication.timeSinceStartup);

        SceneView.RepaintAll();
        EditorApplication.QueuePlayerLoopUpdate();
    }

    private int lastWordCount = 0;
    private List<int> lastEffectsCounts = new();
    private bool initialized = false;
    private void OnValidate()
    {
        //Skip entirely during play mode or playmode transition
        if (Application.isPlaying || EditorApplication.isPlayingOrWillChangePlaymode)
            return;

        //Skip first validation right after script recompile
        if (!initialized)
        {
            initialized = true;
            lastWordCount = wordEffects?.Count ?? 0;
            if (wordEffects == null)
                wordEffects = new List<WordEffectEditor>();
            return;
        }

        //Keep size trackers aligned
        while (lastEffectsCounts.Count < wordEffects.Count)
        {
            lastEffectsCounts.Add(0);
        }

        for (int i = 0; i < wordEffects.Count; i++)
        {
            WordEffectEditor mapping = wordEffects[i] ?? (wordEffects[i] = new WordEffectEditor());
            mapping.effects ??= new List<WordEffects>();

            //If a new word entry was added, start empty
            if (i >= lastWordCount)
            {
                mapping.word = string.Empty;
                mapping.effects.Clear();
            }

            lastEffectsCounts[i] = mapping.effects.Count;
        }

        lastWordCount = wordEffects.Count;

        //Mark dirty so Unity saves the edits
        EditorUtility.SetDirty(this);

        ApplyConfiguredEffects();

    }
#endif

    //apply all active text effects (color, wave and shake) to the TMP mesh
    private void ApplyEffects(float time)
    {
        if (textMeshPro == null || originalMeshInfo == null) return;

        //make TMP to refresh its geometry data
        textMeshPro.ForceMeshUpdate();

        //start from the original vertex positions/colors before adding effects
        ResetToOriginalMeshData();

        TMP_TextInfo textInfo = textMeshPro.textInfo;

        //go through every effect and apply it
        foreach (TextEffect effect in activeEffects)
        {
            effect.ApplyTextEffect(textMeshPro, textInfo, time);
        }

        //update TMP with the new vertex data (after effects applied)
        textMeshPro.UpdateVertexData();
    }

    //reset all text mesh vertices/colors to their original state before applying new effects each frame
    private void ResetToOriginalMeshData()
    {
        if (originalMeshInfo == null) return;

        TMP_TextInfo textInfo = textMeshPro.textInfo;
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            //original copy
            TMP_MeshInfo src = originalMeshInfo[i];
            //current mesh
            TMP_MeshInfo dst = textInfo.meshInfo[i];

            //copy vertex positions and colors back to default
            System.Array.Copy(src.vertices, dst.vertices, src.vertices.Length);
            System.Array.Copy(src.colors32, dst.colors32, src.colors32.Length);
        }
    }

    //builds the list of runtime effects based on the words and settings configured in the inspector
    private void ApplyConfiguredEffects()
    {
        //clear old effects
        activeEffects.Clear();

        if (string.IsNullOrEmpty(plainText) || textMeshPro == null) return;

        if (textMeshPro == null) return;

        //set the text
        textMeshPro.text = plainText;
        //make TMP update its character data
        textMeshPro.ForceMeshUpdate();

        if (textMeshPro.textInfo == null || textMeshPro.textInfo.characterCount == 0) return;

        originalMeshInfo = textMeshPro.textInfo.CopyMeshInfoVertexData();

        //for each word configuration in the list
        foreach (WordEffectEditor mapping in wordEffects)
        {
            if (mapping == null || string.IsNullOrEmpty(mapping.word)) continue;

            //find every matching word in the text
            int index = 0;
            while ((index = plainText.IndexOf(mapping.word, index, System.StringComparison.OrdinalIgnoreCase)) != -1)
            {
                //define which characters this word covers
                TextRange range = new TextRange(index, index + mapping.word.Length - 1);

                //change each configured effect into a real runtime effect
                if (mapping.effects == null) continue;
                foreach (WordEffects config in mapping.effects)
                {
                    if (config != null)
                    {
                        activeEffects.Add(config.CreateRuntimeEffect(range));
                    }
                }

                //move past this word to find the next
                index += mapping.word.Length;
            }
        }

        //store these values to detect changes later
        lastText = plainText;
        lastEffectHash = ComputeEffectHash();
    }

    //creates a quick "signature" of the current effect setup, Used to detect if the configuration changed (word added or color changed, etc.)
    private int ComputeEffectHash()
    {
        unchecked
        {
            int hash = 17;
            foreach (WordEffectEditor mapping in wordEffects)
            {
                if (mapping == null) continue;

                //include word and each effect’s hash in the calculation
                hash = hash * 31 + (mapping.word?.GetHashCode() ?? 0);
                if (mapping.effects != null)
                {
                    foreach (WordEffects effect in mapping.effects)
                    {
                        hash = hash * 31 + (effect?.GetHashCode() ?? 0);
                    }
                }
            }
            return hash;
        }
    }
}
