using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        playButton.onClick.AddListener(OnPlayButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
        playButton.Select(); // Set focus on the play button when the main menu is shown
    }

    private void OnQuitButtonClicked()
    {
        Application.Quit();
    }

    private void OnPlayButtonClicked()
    {
        Loader.Load(Loader.Scene.GameScene);
    }
}
