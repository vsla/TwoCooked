using System;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button optionsButton;


    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.TogglePauseGame();
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            GameManager.Instance.TogglePauseGame(); // Unpause the game if it was paused
            // Load the main menu scene
            Loader.Load(Loader.Scene.MainMenu);
        });

        optionsButton.onClick.AddListener(() =>
        {
            Hide(); // Hide the pause UI before showing options
            OptionsUI.Instance.Show(Show); // Pass the Show method to OptionsUI
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

        resumeButton.Select(); // Set focus on the resume button when the pause UI is shown
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
