using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler onStateChanged;
    public event EventHandler onGamePaused;
    public event EventHandler onGameUnpaused;

    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }

    private State state;

    private float countdownToStartTimer = 3f;
    private float gamePlayingTime;
    private float gamePlayingTimeMax = 360f;
    private bool isGamePaused = false;

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_onPauseAction;
        GameInput.Instance.OnInteractAction += GameInput_onInteractAction;
    }

    private void GameInput_onInteractAction(object sender, EventArgs e)
    {
        if (state == State.WaitingToStart)
        {
            state = State.CountdownToStart;
            onStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Awake()
    {
        Instance = this;

        state = State.WaitingToStart;
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                // waitingToStartTimer -= Time.deltaTime;
                // if (waitingToStartTimer <= 0f)
                // {
                //     state = State.CountdownToStart;
                //     onStateChanged?.Invoke(this, EventArgs.Empty);
                // }
                break;

            case State.CountdownToStart:
                // Start the game countdown logic here
                // For example, you could start a countdown timer
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer <= 0f)
                {
                    state = State.GamePlaying;
                    gamePlayingTime = gamePlayingTimeMax; // Reset the game playing time
                    onStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;

            case State.GamePlaying:
                gamePlayingTime -= Time.deltaTime;
                if (gamePlayingTime <= 0f)
                {
                    state = State.GameOver;
                    onStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                // Handle game over logic here
                break;
        }
    }

    private void GameInput_onPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            Time.timeScale = 0f; // Pause the game
            // Show the pause UI
            onGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f; // Unpause the game
            // Hide the pause UI
            onGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }

    public bool isCountdownToStart()
    {
        return state == State.CountdownToStart;
    }

    public float GetCountdownToStartTime()
    {
        return countdownToStartTimer;
    }

    public bool IsGameOver()
    {
        return state == State.GameOver;
    }

    public float GetGamePlayingTimerNormalized()
    {
        return 1 - (gamePlayingTime / gamePlayingTimeMax);
    }
}
