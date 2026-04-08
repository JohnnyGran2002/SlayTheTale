using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDefenseMoves : Singleton<PlayerDefenseMoves>
{
    [SerializeField] private MeshRenderer _body;
    private bool _isDodging = false, _isParrying = false;
    [SerializeField] private float _cooldown;
    private float _currentCooldown;
    [SerializeField] private float _dodgeDuration, _parryDuration;
    private float _remainingDodgeDuratiuon, _remainingParryDuratiuon;
    private Animator _animator;
    public bool IsDodgning
    {
        get { return _isDodging; }
    }

    public bool IsParrying
    {
        get { return _isParrying; }
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _currentCooldown <= 0)
        {
            TryDodging();
        }

        if (_currentCooldown > 0)
        {
            _currentCooldown -= Time.deltaTime;
        }

        if (IsDodgning == true)
        {
            _remainingDodgeDuratiuon -= Time.deltaTime;
        }

        if (_remainingDodgeDuratiuon <= 0)
        {
            _isDodging = false;
        }
    }
    private void TryDodging()
    {
        if (TurnManager.Instance.currentTurnStatus != TurnManager.turnStatus.enemyTurn)
        {
            Debug.Log("can't dodge on" + TurnManager.Instance.currentTurnStatus.ToString());
        }
        else
        {
            _isDodging = true;
            _animator.Play("Dodge");
            _remainingDodgeDuratiuon = _dodgeDuration;
            _currentCooldown = _cooldown;
        }
    }
}
