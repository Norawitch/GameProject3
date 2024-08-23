using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameProject3.Collectibles;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuControllerInGame : MonoBehaviour
{
    [Header("Pause Menu")]
    [SerializeField] private GameObject pauseMenuPopup = null;

    [Header("Confirmation")]
    [SerializeField] private GameObject confirmationPrompt = null;

    //private CollectibleCount collectibleCount;
    private Light mainLight; // Reference to the main light in the scene

    public bool isGamePause = false;
    private float controllerSensitivity;

    private void Awake()
    {
        //collectibleCount = FindObjectOfType<CollectibleCount>();
        mainLight = RenderSettings.sun;
    }
    private void Start()
    {
        ApplyBrightness(); // Apply brightness at the start of the scene
    }
    private void Update()
    {
        UpdateInput();
    }

    void UpdateInput()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            isGamePause = !isGamePause;
            HandlePauseResume();
        }
    }
    void HandlePauseResume()
    {
        if (isGamePause)
        {
            // Open pause menu
            pauseMenuPopup.SetActive(true);
            // Stop the Time
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;

            // Store controller sensitivity only once when game is paused
            if (GameSettings.ControllerSensitivity != 0f)
            {
                controllerSensitivity = GameSettings.ControllerSensitivity;
            }
            GameSettings.ControllerSensitivity = 0f;
        }
        else
        {
            // Close pause menu
            pauseMenuPopup.SetActive(false);
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;

            // Restore sensitivity
            GameSettings.ControllerSensitivity = controllerSensitivity;

            // Notify CameraController to update mouse sensitivity
            CameraController cameraController = FindObjectOfType<CameraController>();
            if (cameraController != null)
            {
                cameraController.UpdateMouseSensitivity(controllerSensitivity);
            }
        }
    }
    // Apply the brightness value from the settings
    void ApplyBrightness()
    {
        if (mainLight != null)
        {
            mainLight.intensity = GameSettings.Brightness; // Adjust the main light intensity
        }
        else
        {
            Debug.LogWarning("Main light not found in the scene.");
        }
    }
    // Menu in game
    public void ResumeYes()
    {
        isGamePause = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;

        // Close pause menu
        GameSettings.ControllerSensitivity = controllerSensitivity;

        // Notify CameraController to update mouse sensitivity
        CameraController cameraController = FindObjectOfType<CameraController>();
        if (cameraController != null)
        {
            cameraController.UpdateMouseSensitivity(controllerSensitivity);
        }
    }
    public void RestartDialogYes()
    {
        isGamePause = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;

        // Close pause menu
        GameSettings.ControllerSensitivity = controllerSensitivity;

        // Notify CameraController to update mouse sensitivity
        CameraController cameraController = FindObjectOfType<CameraController>();
        if (cameraController != null)
        {
            cameraController.UpdateMouseSensitivity(controllerSensitivity);
        }

        // Reset Collectible count
        Collectible.ResetTotal();

        // Load current level scene again
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void SaveQuitDialogYes()
    {
        // Close pause menu
        isGamePause = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;

        // Close pause menu
        GameSettings.ControllerSensitivity = controllerSensitivity;

        // Notify CameraController to update mouse sensitivity
        CameraController cameraController = FindObjectOfType<CameraController>();
        if (cameraController != null)
        {
            cameraController.UpdateMouseSensitivity(controllerSensitivity);
        }

        // Save this level
        PlayerPrefs.SetString("SavedLevel", SceneManager.GetActiveScene().name);

        // Reset Collectible count
        Collectible.ResetTotal();

        StartCoroutine(ConfirmationBox());
        SceneManager.LoadScene("MainMenu");
    }

    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }
}
