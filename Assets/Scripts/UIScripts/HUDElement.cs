using System;
using UnityEngine;
using DG.Tweening;

public class HUDElement : MonoBehaviour
{
    [SerializeField] private Vector2 hiddenPos, visiblePos;
    
    private RectTransform _rect;
    private bool _hidden = false;

    private void Awake()
    {
        transform.position = hiddenPos;
    }

    public void ToggleVisibility()
    {
        _hidden = !_hidden;
        LerpElement(_hidden);
    }

    public void LerpElement(bool hidden)
    {
        if (_rect == null) _rect = GetComponent<RectTransform>();
        if (hidden)
        {
            _rect.DOAnchorPos(hiddenPos, 0.4f).SetEase(Ease.InBack);
        }
        else
        {
            _rect.DOAnchorPos(visiblePos, 0.4f).SetEase(Ease.OutBack);
        }
    }
    
    private void OnDrawGizmos()
    {
        var canvas = transform.GetComponentInParent<Canvas>();
        if (canvas == null) return;
        Gizmos.matrix = canvas.transform.localToWorldMatrix;
        visiblePos = transform.localPosition;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(hiddenPos, 10f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(visiblePos, 10f);
        Gizmos.color = Color.white;
        Gizmos.DrawLine(hiddenPos, visiblePos);
    }
}
