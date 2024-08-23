using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class LoadPrefs : MonoBehaviour
{
    [Header("General Setting")]
    [SerializeField] private bool canUse = false;
    [SerializeField] private MainMenuController mainMenuController;

    [Header("Volume Setting")]
    // Audio mixer
    [SerializeField] private AudioMixer mainAudioMixer;
    // Master volume
    [SerializeField] private TMP_Text masterVolumeTextValue = null;
    [SerializeField] private Slider masterVolumeSlider = null;
    // Music volume
    [SerializeField] private TMP_Text musicVolumeTextValue = null;
    [SerializeField] private Slider musicVolumeSlider = null;
    // SFX volume
    [SerializeField] private TMP_Text sfxVolumeTextValue = null;
    [SerializeField] private Slider sfxVolumeSlider = null;

    [Header("Brightness Setting")]
    [SerializeField] private TMP_Text brightnessTextValue = null;
    [SerializeField] private Slider brightnessSlider = null;

    [Header("Quality Level Setting")]
    [SerializeField] private TMP_Dropdown qualityDropdown;

    [Header("Fullscreen Setting")]
    [SerializeField] private Toggle fullScreenToggle;

    [Header("Sensitivity Setting")]
    [SerializeField] private TMP_Text controllerSenTextValue = null;
    [SerializeField] public Slider controllerSenSlider = null;

    [Header("Invert Y Setting")]
    [SerializeField] private Toggle invertYToggle = null;

    private void Awake()
    {
        if (canUse)
        {
            if (PlayerPrefs.HasKey("masterVolume") && PlayerPrefs.HasKey("musicVolume") && PlayerPrefs.HasKey("sfxVolume"))
            {
                float masterVolume = PlayerPrefs.GetFloat("masterVolume");
                float musicVolume = PlayerPrefs.GetFloat("musicVolume");
                float sfxVolume = PlayerPrefs.GetFloat("sfxVolume");

                float masterdB = Mathf.Lerp(-80f, 0f, masterVolume / 100f);
                mainAudioMixer.SetFloat("MasterVol", masterdB);
                float musicdB = Mathf.Lerp(-80f, 0f, musicVolume / 100f);
                mainAudioMixer.SetFloat("MusicVol", musicdB);
                float sfxdB = Mathf.Lerp(-80f, 0f, sfxVolume / 100f);
                mainAudioMixer.SetFloat("SFXVol", sfxdB);

                masterVolumeTextValue.text = masterVolume.ToString("0");
                masterVolumeSlider.value = masterVolume;
                musicVolumeTextValue.text = musicVolume.ToString("0");
                musicVolumeSlider.value = musicVolume;
                sfxVolumeTextValue.text = sfxVolume.ToString("0");
                sfxVolumeSlider.value = sfxVolume;
            }
            else
            {
                mainMenuController.ResetButton("Audio");
            }

            if (PlayerPrefs.HasKey("masterQuality"))
            {
                int localQuality = PlayerPrefs.GetInt("masterQuality");
                qualityDropdown.value = localQuality;
                QualitySettings.SetQualityLevel(localQuality);
            }

            if (PlayerPrefs.HasKey("masterFullscreen"))
            {
                int localFullscreen = PlayerPrefs.GetInt("masterFullscreen");
                if (localFullscreen == 1)
                {
                    Screen.fullScreen = true;
                    fullScreenToggle.isOn = true;
                }
                else
                {
                    Screen.fullScreen = false;
                    fullScreenToggle.isOn = false;
                }
            }

            if (PlayerPrefs.HasKey("masterBrightness"))
            {
                float localBrightness = PlayerPrefs.GetFloat("masterBrightness");

                brightnessTextValue.text = localBrightness.ToString("0.0");
                brightnessSlider.value = localBrightness;
            }

            if (PlayerPrefs.HasKey("masterSen"))
            {
                float localSensivity = PlayerPrefs.GetInt("masterSen");

                controllerSenTextValue.text = localSensivity.ToString("0");
                controllerSenSlider.value = localSensivity;
                mainMenuController.mainControllerSen = Mathf.RoundToInt(localSensivity);
            }

            if (PlayerPrefs.HasKey("masterInvertY"))
            {
                if (PlayerPrefs.GetInt("masterInvertY") == 1)
                {
                    invertYToggle.isOn = true;
                }
                else
                {
                    invertYToggle.isOn = false;
                }
            }
        }
    }
}
