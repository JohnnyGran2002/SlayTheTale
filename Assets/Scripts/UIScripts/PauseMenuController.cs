using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;
    private Scene _menuScene;
    private Button[] _buttons;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _buttons = GetComponentsInChildren<Button>();
        _canvasGroup.alpha = 0;
        foreach (var button in _buttons)
        {
            button.interactable = false;
        }
    }

    private void Start()
    {
        resumeButton = _buttons[0];
        settingsButton = _buttons[1];
        quitButton = _buttons[2];
        //Pause();
    }

    public void Pause()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        _canvasGroup.DOFade(1, 0.15f).SetUpdate(true);
        foreach (var button in _buttons)  button.interactable = true;
    }

    public void Resume()
    {
        Debug.Log("Pressed Resume");
        Cursor.lockState = CursorLockMode.Locked;
        _canvasGroup.DOFade(0, 0.15f).SetUpdate(true);
        foreach (var button in _buttons)  button.interactable = false;
        Time.timeScale = 1;
    }

    public void Quit()
    {
        Time.timeScale = 1;
        //BlackScreen .DOFade
        
        // load main menu scene
        SceneManager.LoadScene("MainMenuDEMO");
    }
}
