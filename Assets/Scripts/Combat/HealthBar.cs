using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Health))]
public class HealthBar : MonoBehaviour
{
    [Header("Atributes")]
    [Tooltip("speed for delayed health bar")]
    [SerializeField] private float _lerpDuration;

    [Header("Refrences")]
    [SerializeField] Slider _delayedHealthSlider;
    [SerializeField] Slider _realHealthSlider;
    private Health _health;
    private Transform _mainCamera;
    [SerializeField] private GameObject _canvas;
    private bool _shouldDisplay;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    void Start()
    {
        _realHealthSlider.maxValue = _health.MaxHealth;
        _delayedHealthSlider.maxValue = _realHealthSlider.maxValue;

        _realHealthSlider.value = _health.CurrentHealth;
        _delayedHealthSlider.value = _realHealthSlider.value;

        if (_health.CurrentHealth > 0)
        {
            _shouldDisplay = true;
        }
        else
        {
            _shouldDisplay = false;
        }
    }

    public void UpdateHealthBarEvent(Component sender, object data)
    {
        _realHealthSlider.value = _health.CurrentHealth;

        if (_delayedHealthSlider.value > _realHealthSlider.value)
        {
            StartCoroutine(UpdateHealthBar());
        }
        else
        {
            _delayedHealthSlider.value = _realHealthSlider.value;
        }
    }

    private IEnumerator UpdateHealthBar()
    {
        yield return new WaitForSeconds(0.5f);
        Tween tween = _delayedHealthSlider.DOValue(_realHealthSlider.value, _lerpDuration);
        yield return tween.WaitForCompletion();

        if (_delayedHealthSlider.value <= 0)
        {
            _shouldDisplay = false;
        }


    }

    private void HealthBarVisible()
    {
        if (_shouldDisplay)
        {
            if (!_canvas.activeSelf)
            { _canvas.SetActive(true); }
        }
        else
        {
            if (_canvas.activeSelf)
            { _canvas.SetActive(false); }
        }
    }
}
