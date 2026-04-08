using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManaUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _manaText;

    public void UpdateManaTextEvent(Component sender, object data)
    {
        if (data is int)
        {
            int currentMana = (int)data;
            _manaText.text = currentMana.ToString() + "/3";
        }
    }
}
