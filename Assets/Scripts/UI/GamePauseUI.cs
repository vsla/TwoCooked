using System;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;

    [SerializeField] private Button mainMenuButton;


    private void Awake()
    {
        resumeButton.onClick.AddListener(() => {
            GameManager.Instance.TogglePauseGame();
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            GameManager.Instance.TogglePauseGame(); // Unpause the game if it was paused
            // Load the main menu scene
            Loader.Load(Loader.Scene.MainMenu);    
        });
    }

    private void Start()
    {
        GameManager.Instance.onGamePaused += GameManager_onGamePaused;
        GameManager.Instance.onGameUnpaused += GameManager_onGameUnpaused;

        Hide(); // Initially hide the pause UI
    }

    private void GameManager_onGameUnpaused(object sender, EventArgs e)
    {
        Hide();
    }

    private void GameManager_onGamePaused(object sender, EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
