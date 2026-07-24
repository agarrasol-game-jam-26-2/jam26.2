using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class InteractionController : MonoBehaviour
{
    [Header("Detecção")]
    public Vector2 boxSize = new Vector2(1f, 1f);
    public float distanceInFront = 0.6f;
    public LayerMask interactableLayer;

    [Header("Referências")]
    public PlayerMove playerMove;

    [Header("UI")]
    public TextMeshProUGUI interactionPromptText;
    public GameObject promptPanel;

    private Camera mainCamera;
    private IInteractable currentInteractable;
    private bool promptVisible = false;

    void Start()
    {
        mainCamera = Camera.main;
        if (promptPanel != null)
            promptPanel.SetActive(false);
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
            if (promptPanel != null)
                promptPanel.SetActive(true);
        }

        if (interactionPromptText != null)
            interactionPromptText.text = prompt;
    }

    private void HidePrompt()
    {
        if (promptVisible)
        {
            promptVisible = false;
            if (promptPanel != null)
                promptPanel.SetActive(false);
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
