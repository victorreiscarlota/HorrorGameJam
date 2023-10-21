using TMPro;
using UnityEngine;
using UnityEngine.UI;


    public class GeneralVolumeControl : MonoBehaviour
    {
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private TextMeshProUGUI volumePercentageText;

        private void Start()
        {
            if (!PlayerPrefs.HasKey("generalVolume"))
            {
                PlayerPrefs.SetFloat("generalVolume", 0.7f);
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
            AudioListener.volume = volumeSlider.value;
            Save();
            UpdateText();
        }

        private void Load()
        {
            volumeSlider.value = PlayerPrefs.GetFloat("generalVolume");
        }

        private void Save()
        {
            PlayerPrefs.SetFloat("generalVolume", volumeSlider.value);
        }

        private void UpdateText()
        {
            int percentage = Mathf.RoundToInt(volumeSlider.value * 100);
            volumePercentageText.text = percentage + "%";
        }
    }
