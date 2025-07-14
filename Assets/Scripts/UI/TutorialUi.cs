using System;
using TMPro;
using UnityEngine;

public class TutorialUi : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI keyMoveUpText;

    [SerializeField]
    private TextMeshProUGUI keyMoveDownText;

    [SerializeField]
    private TextMeshProUGUI keyMoveLeftText;

    [SerializeField]
    private TextMeshProUGUI keyMoveRightText;

    [SerializeField]
    private TextMeshProUGUI keyInteractText;

    [SerializeField]
    private TextMeshProUGUI keyInteractAltText;

    [SerializeField]
    private TextMeshProUGUI keyPauseText;

    [SerializeField]
    private TextMeshProUGUI keyInteractGamepadText;

    [SerializeField]
    private TextMeshProUGUI keyInteractAltGamepadText;

    [SerializeField]
    private TextMeshProUGUI keyPauseGamepadText;

    private void Start()
    {
        GameInput.Instance.OnBindingRebound += GameInput_OnBindingRebound;
        GameManager.Instance.onStateChanged += GameManager__OnStateChanged;

        UpdateTextVisualBindings();
        Show();
    }

    private void GameManager__OnStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.isCountdownToStart())
        {
            Hide();
        }
    }

    private void GameInput_OnBindingRebound(object sender, EventArgs e)
    {
        UpdateTextVisualBindings();
    }

    private void OnDestroy()
    {
        GameInput.Instance.OnBindingRebound -= GameInput_OnBindingRebound;
    }

    private void UpdateTextVisualBindings()
    {
        keyMoveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveUp);
        keyMoveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveDown);
        keyMoveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveLeft);
        keyMoveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveRight);
        keyInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        keyInteractAltText.text = GameInput.Instance.GetBindingText(
            GameInput.Binding.InteractAlternate
        );
        keyPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
        keyInteractGamepadText.text = GameInput.Instance.GetBindingText(
            GameInput.Binding.GamepadInteract
        );
        keyInteractAltGamepadText.text = GameInput.Instance.GetBindingText(
            GameInput.Binding.GamepadInteractAlternate
        );
        keyPauseGamepadText.text = GameInput.Instance.GetBindingText(
            GameInput.Binding.GamepadPause
        );
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void GameManager_onGameUnpaused(object sender, EventArgs e)
    {
        Hide();
    }
}
