using SerializeReferenceEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Maps a specific word to one or more effects, Each matching word in the text gets converted to actual runtime TextEffect instances using those configs.
[System.Serializable]
public class WordEffectEditor
{
    //Stores the word to look for in the text
    public string word;

    // Stores a [SerializeReference] list of WordEffects
    [field: SerializeReference, SR] public List<WordEffects> effects = new List<WordEffects>();
}
