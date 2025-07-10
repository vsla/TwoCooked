using System;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

    private void Start()
    {
        GameManager.Instance.onStateChanged += GameManager_onStateChanged;
        Hide();
    }

    private void GameManager_onStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.isCountdownToStart())
        {

            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Update()
    {
        if (GameManager.Instance.isCountdownToStart())
        {
            int countdownValue = Mathf.CeilToInt(GameManager.Instance.GetCountdownToStartTime());
            countdownText.text = countdownValue.ToString();
        }
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
