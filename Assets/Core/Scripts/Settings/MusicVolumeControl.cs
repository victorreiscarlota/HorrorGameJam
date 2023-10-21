using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeControl : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI volumePercentageText;  
    [SerializeField] private AudioSource backgroundMusic; 

    private void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 0.7f);
            Load();
        }
        else
        {
            Load();
        }
        UpdateText(); 
    }

    public void ChangeVolume()
    {
        backgroundMusic.volume = volumeSlider.value;  
        Save();
        UpdateText();  
    }

    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        backgroundMusic.volume = volumeSlider.value;  
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }

    private void UpdateText()
    {
        int percentage = Mathf.RoundToInt(volumeSlider.value * 100);
        volumePercentageText.text = percentage + "%";  
    }
}