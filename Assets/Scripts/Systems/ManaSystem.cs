using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class ManaSystem : Singleton<ManaSystem>
{
    [SerializeField] private ManaUI manaUI;

    private int _maxMana = 3;

    private int _currentMana;

    public GameEvent UpdateManaUI;

    public void UpdateManaEvent(Component sender, object data)
    {
        if (data is int)
        {
            int manaChange = (int)data;
            StartCoroutine(UpdateMana(manaChange));
        }
    }

    public bool HaveEnoughMana(int manaCost)
    {
        return _currentMana >= manaCost;
    }

    private IEnumerator UpdateMana(int manaChange)
    {
        _currentMana += manaChange;
        UpdateManaUI.Raise(this, _currentMana);
        yield return null;
    }

    public void RefillManaEvent(Component sender, object data)
    {
        StartCoroutine(RefillMana());
    }

    private IEnumerator RefillMana()
    {
        _currentMana = _maxMana;
        UpdateManaUI.Raise(this, _currentMana);
        yield return null;
    }
}
