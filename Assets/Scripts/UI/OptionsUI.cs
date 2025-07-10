using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }
    [SerializeField] private Slider soundEffectSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Button closeButton;

    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI interactAltText;
    [SerializeField] private TextMeshProUGUI pauseText;

    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button interactAltButton;
    [SerializeField] private Button pauseButton;

    [SerializeField] private Transform pressToRebindKeyTransform;


    private void Awake()
    {
        Instance = this;


        soundEffectSlider.onValueChanged.AddListener((v) =>
        {
            SoundManager.Instance.ChangeVolume(v / 10);
        });
        musicSlider.onValueChanged.AddListener((v) =>
        {
            MusicManager.Instance.ChangeVolume(v / 10);
        });
        closeButton.onClick.AddListener(() =>
        {
            Hide();
        });

        moveUpButton.onClick.AddListener(() => RebindingBinding(GameInput.Binding.MoveUp));
        moveDownButton.onClick.AddListener(() => RebindingBinding(GameInput.Binding.MoveDown));
        moveLeftButton.onClick.AddListener(() => RebindingBinding(GameInput.Binding.MoveLeft));
        moveRightButton.onClick.AddListener(() => RebindingBinding(GameInput.Binding.MoveRight));
        interactButton.onClick.AddListener(() => RebindingBinding(GameInput.Binding.Interact));
        interactAltButton.onClick.AddListener(() => RebindingBinding(GameInput.Binding.InteractAlternate));
        pauseButton.onClick.AddListener(() => RebindingBinding(GameInput.Binding.Pause));
    }



    private void Start()
    {
        GameManager.Instance.onGameUnpaused += GameManager_onGameUnpaused;
        Hide(); // Initially hide the options UI
        float defaultSoundsVolume = SoundManager.Instance.GetPrefsDefaultVolume();
        float defaultMusicVolume = MusicManager.Instance.GetPrefsDefaultVolume();

        soundEffectSlider.value = defaultSoundsVolume * 10; // Assuming slider value is from 0 to 10
        musicSlider.value = defaultMusicVolume * 10; // Assuming slider value is from

        UpdateVisualBindings();
        HidePressToRebindKey();
    }

    private void UpdateVisualBindings()
    {
        moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveUp);
        moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveDown);
        moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveLeft);
        moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveRight);
        interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        interactAltText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
        pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
    }

    private void GameManager_onGameUnpaused(object sender, EventArgs e)
    {
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ShowPressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(true);
    }

    private void HidePressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(false);
    }

    private void RebindingBinding(GameInput.Binding binding)
    {
        ShowPressToRebindKey();
        GameInput.Instance.RebindBinding(binding, () =>
        {
            HidePressToRebindKey();
            UpdateVisualBindings();
        });
    }
}
