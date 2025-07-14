using System;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    private const string PLAY_NUMBER_POPUP = "PlayNumberPopup";
    [SerializeField]
    private TextMeshProUGUI countdownText;

    private int previousCountdownValue;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

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

            if (countdownValue != previousCountdownValue)
            {
                previousCountdownValue = countdownValue;
                animator.SetTrigger(PLAY_NUMBER_POPUP);
                SoundManager.Instance.PlayCountdownSound(1f);
            }
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
