using System;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    private bool _hidden;
    public void UpdateElementPositions(Component sender, object data)
    {
        _hidden = data is TurnManager.CurrentTurn.EnemyTurn;
        foreach (Transform child in gameObject.transform)
        {
            var comp = child.GetComponent<HUDElement>();
            if (comp != null) comp.LerpElement(_hidden);
        }
    }
}
