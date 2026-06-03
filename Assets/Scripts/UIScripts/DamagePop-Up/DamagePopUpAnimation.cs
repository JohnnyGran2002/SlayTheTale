using Unity.VisualScripting;
using UnityEngine;

public class DamagePopUpAnimation : MonoBehaviour
{
    [SerializeField] private AnimationCurve _heightCurve;
    [SerializeField] private AnimationCurve _sideCurve;
    private Vector3 _origin;
    private float _time;

    private void Awake()
    {
        _origin = transform.position;
    }

    private void Update()
    {
        _time += Time.deltaTime;
        transform.position = _origin + new Vector3(0 + _sideCurve.Evaluate(_time), 2 + _heightCurve.Evaluate(_time), 0);
    }
}
