using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject Enemies;
    public CanvasGroup Group;
    private bool _playerAlive;
    private Health _playerHealth;
    private Health[] _enemiesHealth;
    private List<Health> _deadEnemies = new List<Health>();

    private void Awake()
    {
        _playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        _enemiesHealth = Enemies.GetComponentsInChildren<Health>();
        _deadEnemies.Clear();
    }

    private void Update()
    {
        for (int i = 0; i < _enemiesHealth.Length; i++)
        {
            if (!_enemiesHealth[i].IsAlive && !_deadEnemies.Contains(_enemiesHealth[i]))
            {
                _deadEnemies.Add(_enemiesHealth[i]);
            }
        }
        if (_deadEnemies.Count >= _enemiesHealth.Length) StartCoroutine(SceneChange(_playerHealth.IsAlive));
        else if (!_playerHealth.IsAlive) StartCoroutine(SceneChange(_playerHealth.IsAlive));
    }

    private IEnumerator SceneChange(bool isAlive)
    {
        Group.GetComponent<DeathScreenController>().SetText(isAlive);
        Group.DOFade(1, 0.15f);
        yield return new WaitForSeconds(2.5f);
        LoadScene(isAlive);
    }

    void LoadScene(bool playerAlive)
    {
        if (playerAlive)
        {
            if (SceneManager.GetActiveScene().buildIndex + 1 >= SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene("MainMenuDEMO");
                return;
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            return;
        }
        SceneManager.LoadScene("MainMenuDEMO");
    }
}
