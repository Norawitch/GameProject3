using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Audio;

public class MainMenuController : MonoBehaviour
{
    [Header("Volume Setting")]
    // Audio mixer
    [SerializeField] private AudioMixer mainAudioMixer;
    // Master volume
    [SerializeField] private TMP_Text masterVolumeTextValue = null;
    [SerializeField] private Slider masterVolumeSlider = null;
    [SerializeField] private float defaultVolume = 100.0f;
    // Music volume
    [SerializeField] private TMP_Text musicVolumeTextValue = null;
    [SerializeField] private Slider musicVolumeSlider = null;
    [SerializeField] private float defaultMusicVolume = 100.0f;
    // SFX volume
    [SerializeField] private TMP_Text sfxVolumeTextValue = null;
    [SerializeField] private Slider sfxVolumeSlider = null;
    [SerializeField] private float defaultSfxVolume = 100.0f;

    [Header("Gameplay Settings")]
    [SerializeField] private TMP_Text controllerSenTextValue = null;
    [SerializeField] private Slider controllerSenSlider = null;
    [SerializeField] private int defaultSenValue = 4;
    public int mainControllerSen = 4;

    [Header("Toggle Settings")]
    [SerializeField] private Toggle invertYToggle = null;

    [Header("Graphics Settings")]
    [SerializeField] private TMP_Text brightnessTextValue = null;
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private float defaultBrightness = 1.0f;

    [Space(10)]
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle fullScreenToggle;

    private int _qualityLevel;
    private bool _isFullScreen;
    private float _brightnessLevel;

    [Header("Resolution Dropdown")]
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    [Header("Confirmation")]
    [SerializeField] private GameObject confirmationPrompt = null;

    [Header("Levels to load")]
    public string _newGameLevel;
    private string levelToLoad;
    private bool IsPlayed = false;
    [SerializeField] private GameObject noSaveGameDialog = null;

    private void Start()
    {
        LoadIntitialSettings();
    }
    private void Update()
    {
        // Debug.Log(masterVolumeSlider.value);
    }

    private void LoadIntitialSettings()
    {
        // Load and set Resolution
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Load and set Master Volume
        float masterVolume = PlayerPrefs.GetFloat("masterVolume", defaultVolume);
        masterVolumeSlider.value = masterVolume;
        masterVolumeTextValue.text = masterVolume.ToString("0");
        GameSettings.MasterVolume = masterVolume;

        // Load and set Music Volume
        float musicVolume = PlayerPrefs.GetFloat("musicVolume", defaultMusicVolume);
        musicVolumeSlider.value = musicVolume;
        musicVolumeTextValue.text = musicVolume.ToString("0");
        GameSettings.MusicVolume = musicVolume;

        // Load and set SFX Volume
        float sfxVolume = PlayerPrefs.GetFloat("sfxVolume", defaultSfxVolume);
        sfxVolumeSlider.value = sfxVolume;
        sfxVolumeTextValue.text = sfxVolume.ToString("0");
        GameSettings.SfxVolume = sfxVolume;

        // Load and set Controller Sensitivity
        float sensitivity = PlayerPrefs.GetFloat("masterSen", defaultSenValue);
        controllerSenSlider.value = sensitivity;
        controllerSenTextValue.text = sensitivity.ToString("0");
        GameSettings.ControllerSensitivity = sensitivity;

        // Load and set Invert Y
        bool invertY = PlayerPrefs.GetInt("masterInvertY", 0) == 1;
        invertYToggle.isOn = invertY;
        GameSettings.InvertY = invertY;

        // Load and set Brightness
        float brightness = PlayerPrefs.GetFloat("masterBrightness", defaultBrightness);
        brightnessSlider.value = brightness;
        brightnessTextValue.text = brightness.ToString("0.0");
        GameSettings.Brightness = brightness;

        // Load and set Quality
        int quality = PlayerPrefs.GetInt("masterQuality", 1);
        qualityDropdown.value = quality;
        QualitySettings.SetQualityLevel(quality);
        GameSettings.Quality = quality;

        // Load and set Fullscreen
        bool isFullScreen = PlayerPrefs.GetInt("masterFullscreen", 1) == 1;
        fullScreenToggle.isOn = isFullScreen;
        Screen.fullScreen = isFullScreen;
        GameSettings.FullScreen = isFullScreen;
    }



    public void NewGameDialogYes()
    {
        PlayerPrefs.SetString("SavedLevel", "Level0");
        IsPlayed = true;
        SceneManager.LoadScene(_newGameLevel);
    }
    public void LoadGameDialogYes()
    {
        if (PlayerPrefs.HasKey("SavedLevel") && IsPlayed)
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel");
            if (string.IsNullOrEmpty(levelToLoad))
            {
                levelToLoad = "Level0";
            }
            SceneManager.LoadScene(levelToLoad);
        }
        else
        {
            noSaveGameDialog.SetActive(true);
        }
    }
    public void QuitButton()
    {
        Application.Quit();
    }


    // Graphics
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetBrightness(float brightness)
    {
        _brightnessLevel = brightness;
        brightnessTextValue.text = brightness.ToString("0.0");
        GameSettings.Brightness = brightness;
    }
    public void SetFullScreen(bool isFullScreen)
    {
        _isFullScreen = isFullScreen;
        GameSettings.FullScreen = isFullScreen;
    }
    public void SetQuality(int qualityIndex)
    {
        _qualityLevel = qualityIndex;
        GameSettings.Quality = qualityIndex;
    }
    public void GraphicsApply()
    {
        PlayerPrefs.SetFloat("masterBrightness", _brightnessLevel);
        GameSettings.Brightness = _brightnessLevel;

        PlayerPrefs.SetInt("masterQuality", _qualityLevel);
        QualitySettings.SetQualityLevel(_qualityLevel);
        GameSettings.Quality = _qualityLevel;

        PlayerPrefs.SetInt("masterFullscreen", (_isFullScreen ? 1 : 0));
        Screen.fullScreen = _isFullScreen;
        GameSettings.FullScreen = _isFullScreen;

        StartCoroutine(ConfirmationBox());
    }

    // Volume
    public void SetMasterVolume()
    {
        float masterVolumeLevel = masterVolumeSlider.value;
        float dB = Mathf.Lerp(-80f, 0f, masterVolumeLevel / 100f);
        mainAudioMixer.SetFloat("MasterVol", dB);
        GameSettings.MasterVolume = masterVolumeLevel;

        masterVolumeTextValue.text = masterVolumeLevel.ToString("0");
    }
    public void SetMusicVolume()
    {
        float musicVolumeLevel = musicVolumeSlider.value;
        float dB = Mathf.Lerp(-80f, 0f, musicVolumeLevel / 100f);
        mainAudioMixer.SetFloat("MusicVol", dB);
        GameSettings.MusicVolume = musicVolumeLevel;

        musicVolumeTextValue.text = musicVolumeLevel.ToString("0");
    }
    public void SetSFXVolume()
    {
        float sfxVolumeLevel = sfxVolumeSlider.value;
        float dB = Mathf.Lerp(-80f, 0f, sfxVolumeLevel / 100f);
        mainAudioMixer.SetFloat("SFXVol", dB);
        GameSettings.SfxVolume = sfxVolumeLevel;

        sfxVolumeTextValue.text = sfxVolumeLevel.ToString("0");
    }
    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", masterVolumeSlider.value);
        PlayerPrefs.SetFloat("musicVolume", musicVolumeSlider.value);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolumeSlider.value);

        StartCoroutine(ConfirmationBox());
    }

    // Gameplay
    public void SetControllerSens(float sensitivity)
    {
        mainControllerSen = Mathf.RoundToInt(sensitivity);
        controllerSenTextValue.text = sensitivity.ToString("0");
        GameSettings.ControllerSensitivity = sensitivity;
    }
    public void GameplayApply()
    {
        if (invertYToggle.isOn)
        {
            PlayerPrefs.SetInt("masterInvertY", 1);
            GameSettings.InvertY = true;
            // Invert Y
        }
        else
        {
            PlayerPrefs.SetInt("masterInvertY", 0);
            GameSettings.InvertY = false;
            // Not invert
        }
        PlayerPrefs.SetFloat("masterSen", mainControllerSen);
        GameSettings.ControllerSensitivity = mainControllerSen;
        StartCoroutine(ConfirmationBox());
    }

    // Reset
    public void ResetButton(string MenuType)
    {
        if (MenuType == "Graphics")
        {
            brightnessSlider.value = defaultBrightness;
            brightnessTextValue.text = defaultBrightness.ToString("0.0");
            GameSettings.Brightness = defaultBrightness;

            qualityDropdown.value = 1;
            QualitySettings.SetQualityLevel(1);
            GameSettings.Quality = 1;

            fullScreenToggle.isOn = false;
            Screen.fullScreen = false;
            GameSettings.FullScreen = false;

            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
            resolutionDropdown.value = resolutions.Length;

            GraphicsApply();
        }
        if (MenuType == "Audio")
        {
            masterVolumeSlider.value = defaultVolume;
            masterVolumeTextValue.text = defaultVolume.ToString("0");
            musicVolumeSlider.value = defaultMusicVolume;
            musicVolumeTextValue.text = defaultMusicVolume.ToString("0");
            sfxVolumeSlider.value = defaultSfxVolume;
            sfxVolumeTextValue.text = defaultSfxVolume.ToString("0");
            GameSettings.MasterVolume = defaultVolume;
            GameSettings.MusicVolume = defaultMusicVolume;
            GameSettings.SfxVolume = defaultSfxVolume;

            VolumeApply();
        }
        if (MenuType == "Gameplay")
        {
            controllerSenTextValue.text = defaultSenValue.ToString("0");
            controllerSenSlider.value = defaultSenValue;
            mainControllerSen = defaultSenValue;
            
            invertYToggle.isOn = false;
            GameSettings.ControllerSensitivity = defaultSenValue;
            GameSettings.InvertY = false;

            GameplayApply();
        }
    }

    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }

}
