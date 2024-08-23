using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameProject3.Collectibles;
using UnityEngine.InputSystem;

public class NPCInteractable : MonoBehaviour
{
    [Header("Dialog Settings")]
    [SerializeField] private string npcName;
    [SerializeField][TextArea(3, 10)] private string[] dialogLines;

    [Header("UI Popup")]
    [SerializeField] private GameObject interactionIcon;
    [SerializeField] private float interactionDistance = 3f;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip interactionClip;

    [SerializeField] private Transform playerTransform;

    MenuControllerInGame menuControllerInGame;

    private Dialog dialogUI;
    private CollectibleCount collectibleCount;

    private float controllerSensitivity;
    public bool canInteract = true; // Indicates if the NPC can be interacted with
    public string questID; // ID for the quest associated with this NPC

    private bool isInteracting = false;

    private void Start()
    {
        menuControllerInGame = FindObjectOfType<MenuControllerInGame>();
        collectibleCount = FindObjectOfType<CollectibleCount>();
        dialogUI = FindObjectOfType<Dialog>(true);
        interactionIcon.SetActive(false);
    }

    private void Update()
    {
        CheckForInteraction();
    }
    private void CheckForInteraction()
    {
        if (playerTransform == null)
        {
            Debug.LogError("Player transform is not assigned.");
            return;
        }

        float distance = Vector3.Distance(playerTransform.position, transform.position);
        if (distance <= interactionDistance)
        {
            interactionIcon.SetActive(true);
            if (Keyboard.current.eKey.wasPressedThisFrame && !isInteracting)
            {
                Interact();
            }
        }
        else
        {
            interactionIcon.SetActive(false);
        }
    }

    // Method to handle interaction with the NPC
    public void Interact()
    {
        if (canInteract && !isInteracting)
        {
            isInteracting = true; // Set interacting to true when starting interaction

            // Play the interaction sound
            if (audioSource != null && interactionClip != null)
            {
                audioSource.PlayOneShot(interactionClip);
            }
            StartQuest();

            // Set mouse sensitivity to 0 when the dialog starts
            SetMouseSensitivity();
        }
    }

    // Method to start a quest
    private void StartQuest()
    {
        if (dialogUI != null && dialogLines.Length > 0)
        {
            dialogUI.StartDialog(dialogLines, OnDialogFinished);
        }
        // collectibleCount.gameObject.SetActive(true);
        collectibleCount.StartQuest();
    }
    // Method to restore mouse sensitivity when the dialog finishes
    private void OnDialogFinished()
    {
        // Restore sensitivity
        GameSettings.ControllerSensitivity = controllerSensitivity;

        // Notify CameraController to update mouse sensitivity
        CameraController cameraController = FindObjectOfType<CameraController>();
        if (cameraController != null)
        {
            cameraController.UpdateMouseSensitivity(controllerSensitivity);
        }
        isInteracting = false; // Allow interaction again after the dialog finishes
    }

    // Method to set mouse sensitivity
    private void SetMouseSensitivity()
    {
        // Store controller sensitivity only once when game is paused
        if (GameSettings.ControllerSensitivity != 0f)
        {
            controllerSensitivity = GameSettings.ControllerSensitivity;
        }
        GameSettings.ControllerSensitivity = 0f;
    }
}
