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

    private void Start()
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
        if (_deadEnemies.Count >= _enemiesHealth.Length || Input.GetKeyDown(KeyCode.P)) StartCoroutine(SceneChange(_playerHealth.IsAlive));
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
            MapGenerator.instance.rewardPending = true;
            Cursor.lockState = CursorLockMode.Confined;
            MapGenerator.instance.Move(true);
            if (SceneManager.GetActiveScene().name == "Act_1_BossScene")
            {
                SceneManager.LoadScene("MainMenu");
            }
            else
            {
                SceneManager.LoadScene("Map");
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            SceneManager.LoadScene("Map");
        }
    }
}
