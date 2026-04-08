using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//defines which characters in the text an effect applies to
[System.Serializable]
public struct TextRange
{
    public int StartIndex;
    public int EndIndex;
    //start and endIndex is used to store the range of characters being affected(like a "highlight" region inside the text)
    public TextRange(int start, int end)
    {
        StartIndex = start;
        EndIndex = end;
    }
}
