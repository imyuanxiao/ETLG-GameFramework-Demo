using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameFramework.Localization;
using TMPro;

namespace ETLG
{
    public class UISettingsForm : UGuiFormEx
    {
        public Button okButton;
        public Button cancelButton;

        public TMP_Dropdown resolutionDropdown;
        public Slider volumeSlider;
        public TMP_Dropdown difficultyDropdown;
        private string resolution;

        private int width;
        private int height;
        private int resolutionOptionIdx;
        private int difficulty;  // 0 - easy, 1 - normal, 2 - hard, 3 - challenge

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            okButton.onClick.AddListener(OnOkButtonClick);
            cancelButton.onClick.AddListener(OnCancelButtonClick);
            resolutionDropdown.onValueChanged.AddListener(delegate {
                DropdownValueChanged(resolutionDropdown);
            });
            volumeSlider.onValueChanged.AddListener(delegate {
                SliderValueChanged(volumeSlider);
            });
            difficultyDropdown.onValueChanged.AddListener(delegate {
                DifficultyChange(difficultyDropdown);
            });
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            LoadResolution();  // Init the default option of the resolution dropdown (doesn't change the actual resultion)
            LoadSoundVolume(); // Init the default position of the volume slider (doesn't change the actual sound volume)
            LoadDifficulty();  // Init the default option of the difficulty dropdown (doesn't change the actual difficulty)
        }
        private void OnCancelButtonClick()
        {
            GameEntry.Sound.PlaySound(EnumSound.ui_sound_back);
            Close();
        }

        private void OnOkButtonClick()
        {
            Screen.SetResolution(width, height, true);

            SaveResolution(this.resolutionOptionIdx);
            SaveManager.Instance.SaveResolution(width, height);
            SaveManager.Instance.difficulty = this.difficulty;
            SaveManager.Instance.Save("Difficulty_0", this.difficulty);

            GameEntry.Sound.PlaySound(EnumSound.ui_sound_back);
            Close();
        }

        private void DropdownValueChanged(TMP_Dropdown change)
        {
            int optionIdx = change.value;

            this.resolution = change.options[optionIdx].text;

            string widthStr = resolution.Split('X')[0];
            string heightStr = resolution.Split('X')[1];
            int width = int.Parse(widthStr);
            int height = int.Parse(heightStr);

            this.width = width;
            this.height = height;
            this.resolutionOptionIdx = optionIdx;
        }

        private void DifficultyChange(TMP_Dropdown change)
        {
            int optionIdx = change.value;
            this.difficulty = optionIdx;
        }
        
        private void SliderValueChanged(Slider volumeSlider)
        {
            AudioListener.volume = volumeSlider.value;
            SaveSoundVolume(volumeSlider.value);
            SaveManager.Instance.SaveSoundVolume(volumeSlider.value);
        }

        private void SaveResolution(int optionIdx)
        {
            PlayerPrefs.SetInt("Resolution_Option_Idx", optionIdx);
        }

        private void LoadResolution()
        {
            if (PlayerPrefs.HasKey("Resolution_Option_Idx"))
            {
                resolutionDropdown.value = PlayerPrefs.GetInt("Resolution_Option_Idx");
                this.resolutionOptionIdx = PlayerPrefs.GetInt("Resolution_Option_Idx");
            }
            else
            {
                resolutionDropdown.value = 0;
                this.resolutionOptionIdx = 0;
            }
        }

        private void SaveSoundVolume(float soundVolume)
        {
            PlayerPrefs.SetFloat("SoundVolume", soundVolume);
        }

        private void LoadSoundVolume()
        {
            if (PlayerPrefs.HasKey("SoundVolume"))
            {
                volumeSlider.value = PlayerPrefs.GetFloat("SoundVolume");
            }
            else 
            {
                volumeSlider.value = 1.0f;
            }
        }

        private void LoadDifficulty()
        {
            if (PlayerPrefs.HasKey("Difficulty_0"))
            {
                difficultyDropdown.value = PlayerPrefs.GetInt("Difficulty_0");
            }
        }
    }
}


