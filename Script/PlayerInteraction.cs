using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionRange = 3f; // Distance the player can interact
    public LayerMask interactableLayer; // Layer for interactable objects
    public Text interactionText; // UI text for prompts
    private PlayerInput playerInput;
    private InputAction interactAction;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        interactAction = playerInput.actions["Interact"]; // Make sure "Interact" is set up in Input Actions
        interactionText.gameObject.SetActive(false);
    }

    private void Update()
    {
        DetectInteractable();
    }

    private void DetectInteractable()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactionRange, interactableLayer))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                ShowInteractionPrompt(interactable.promptMessage);
                
                if (interactAction.WasPressedThisFrame())
                {
                    interactable.Interact();
                }
            }
        }
        else
        {
            HideInteractionPrompt();
        }
    }

    private void ShowInteractionPrompt(string message)
    {
        if (interactionText != null)
        {
            interactionText.text = message;
            interactionText.gameObject.SetActive(true);
        }
    }

    private void HideInteractionPrompt()
    {
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
    }
}
