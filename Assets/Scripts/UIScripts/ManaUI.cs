using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ManaUI : MonoBehaviour
{
    [SerializeField] private List<Image> manaImages;
    [SerializeField] private float duration = 0.15f;
    // If more controlled delays than Tween sequence add this and [1] & [2]
    //[SerializeField] private float _fadeDelay = 0.1f;

    private int _previousMana = 3;
    
    public void UpdateManaAnimationEvent(Component sender, object data)
    {
        if (data is not int currentMana) return;
        Sequence seq = DOTween.Sequence();
        if (currentMana < _previousMana)
        {
            // [1] Replace with this to use _fadeDelay
            //int delayIndex = 0;
            // for (int i = _previousMana - 1; i >= currentMana; i--)
            // {
            //     manaImages[i].DOFade(0f, duration).SetDelay(delayIndex * _fadeDelay);
            //
            //     delayIndex++;
            // }
            for (int i = _previousMana - 1; i >= currentMana; i--)
            {
                seq.Append(manaImages[i].DOFade(0f, duration));
            }
        }
        else if (currentMana > _previousMana)
        {
            // [2] Replace with this to use _fadeDelay
            // int delayIndex = 0;
            //
            // for (int i = _previousMana; i < currentMana; i++)
            // {
            //     manaImages[i].DOFade(1f, duration).SetDelay(delayIndex * _fadeDelay);
            //
            //     delayIndex++;
            // }
            for (int i = _previousMana; i < currentMana; i++)
            {
                seq.Append(manaImages[i].DOFade(1f, duration));
            }
        }

        _previousMana = currentMana;
    }
}
