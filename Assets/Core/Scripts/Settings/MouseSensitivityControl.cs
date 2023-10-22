using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivityControl : MonoBehaviour
{
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TextMeshProUGUI sensitivityValueText;
    [SerializeField] private PlayerCameraControl playerCameraControl;

    private const string SensitivityKey = "mouseSensitivity";

    private void Start()
    {
        float savedSensitivity = PlayerPrefs.HasKey(SensitivityKey) ? PlayerPrefs.GetFloat(SensitivityKey) * 40f : 10f;

        sensitivitySlider.value = savedSensitivity;
        UpdateSensitivityText(savedSensitivity);
        playerCameraControl.SetSensitivity(savedSensitivity / 40f);
        sensitivitySlider.onValueChanged.AddListener(UpdateSensitivity);
    }


    public void UpdateSensitivity(float sliderValue)
    {
        float newSensitivity = sliderValue / 40f;
        playerCameraControl.SetSensitivity(newSensitivity);
        PlayerPrefs.SetFloat(SensitivityKey, newSensitivity);

        UpdateSensitivityText(sliderValue);
    }

    private void UpdateSensitivityText(float value)
    {
        sensitivityValueText.text = value.ToString("0.00");
    }
}