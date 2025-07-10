using TMPro;
using UnityEngine;

public class SliderScriptUI : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Slider _slider;
    [SerializeField] private TextMeshProUGUI _sliderValueText;
    [SerializeField] private bool isDecimal = true;
    [SerializeField] private int maxValue = 100;
    [SerializeField] private float defaultValue = 0;

    private void Awake()
    {
        _slider.onValueChanged.AddListener(OnSliderValueChanged);
        _slider.maxValue = maxValue;
        _slider.minValue = 0;
        _slider.value = defaultValue;
    }

    private void OnSliderValueChanged(float value)
    {
        // Clamp the value to ensure it does not exceed the maximum value

        _sliderValueText.text = isDecimal ? value.ToString("F2") : value.ToString("F0");
    }
}
