using GameProject3.Collectibles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevelTrigger : MonoBehaviour
{
    [Header("Finish level popup")]
    [SerializeField] private GameObject finishPopup = null;

    CollectibleCount collectibleCount;

    // private float controllerSensitivity;

    private void Start()
    {
        collectibleCount = FindObjectOfType<CollectibleCount>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (collectibleCount != null && collectibleCount.canFinishLevel)
            {
                //controllerSensitivity = GameSettings.ControllerSensitivity;
                finishPopup.SetActive(true);
                // Stop the Time
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;

                // Set the controller sensitivity to 0

                // Notify CameraController to update mouse sensitivity
                CameraController cameraController = FindObjectOfType<CameraController>();
                if (cameraController != null)
                {
                    cameraController.UpdateMouseSensitivity(0f);
                }
            }
        }
    }
    public void RestartDialogYes()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;

        // Notify CameraController to update mouse sensitivity
        CameraController cameraController = FindObjectOfType<CameraController>();
        if (cameraController != null)
        {
            cameraController.UpdateMouseSensitivity(GameSettings.ControllerSensitivity);
        }

        // Reset Collectible count
        Collectible.ResetTotal();

        // Load current level scene again
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
