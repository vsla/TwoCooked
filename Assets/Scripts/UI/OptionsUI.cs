using System;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }
    [SerializeField] private Slider soundEffectSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Button closeButton;


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
    }



    private void Start()
    {
        GameManager.Instance.onGameUnpaused += GameManager_onGameUnpaused;
        Hide(); // Initially hide the options UI
        float defaultSoundsVolume = SoundManager.Instance.GetPrefsDefaultVolume();
        float defaultMusicVolume = MusicManager.Instance.GetPrefsDefaultVolume();

        soundEffectSlider.value = defaultSoundsVolume * 10; // Assuming slider value is from 0 to 10
        musicSlider.value = defaultMusicVolume * 10; // Assuming slider value is from
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
}
