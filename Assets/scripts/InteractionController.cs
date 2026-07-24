using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InteractionController : MonoBehaviour
{
    [Header("Detecção")]
    public float interactionRange = 5f;
    public LayerMask interactableLayer;

    [Header("UI")]
    public Text interactionPromptText;
    public CanvasGroup promptCanvasGroup;

    private Camera mainCamera;
    private IInteractable currentInteractable;
    private bool promptVisible = false;

    void Start()
    {
        mainCamera = Camera.main;
        if (promptCanvasGroup != null)
            promptCanvasGroup.alpha = 0f;
    }

    void Update()
    {
        DetectInteractable();
        HandleInteractionInput();
    }

    private void DetectInteractable()
    {
        RaycastHit hit;
        IInteractable newInteractable = null;

        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, interactionRange, interactableLayer))
        {
            newInteractable = hit.collider.GetComponent<IInteractable>();
        }

        if (newInteractable != currentInteractable)
        {
            currentInteractable = newInteractable;
            UpdatePromptUI();
        }
    }

    private void UpdatePromptUI()
    {
        if (currentInteractable != null && currentInteractable.CanInteract())
        {
            ShowPrompt(currentInteractable.GetInteractionPrompt());
        }
        else
        {
            HidePrompt();
        }
    }

    private void ShowPrompt(string prompt)
    {
        if (!promptVisible)
        {
            promptVisible = true;
            if (promptCanvasGroup != null)
                promptCanvasGroup.alpha = 1f;
        }

        if (interactionPromptText != null)
            interactionPromptText.text = prompt;
    }

    private void HidePrompt()
    {
        if (promptVisible)
        {
            promptVisible = false;
            if (promptCanvasGroup != null)
                promptCanvasGroup.alpha = 0f;
        }
    }

    private void HandleInteractionInput()
    {
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (currentInteractable != null && currentInteractable.CanInteract())
            {
                currentInteractable.Interact(gameObject);
            }
        }
    }
}
