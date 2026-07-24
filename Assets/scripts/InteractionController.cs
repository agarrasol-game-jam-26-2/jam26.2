using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

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

    private IInteractable currentInteractable;
    private bool promptVisible = false;
    private Vector2 lastBoxCenter;

    void Start()
    {
        if (promptPanel != null)
            promptPanel.SetActive(false);

        if (playerMove == null)
            playerMove = GetComponent<PlayerMove>();
    }

    void Update()
    {
        DetectInteractable();
        HandleInteractionInput();
    }

    private void DetectInteractable()
    {
        Vector2 facing = playerMove != null ? playerMove.facingDirection : Vector2.down;
        Vector2 boxCenter = (Vector2)transform.position + facing * distanceInFront;
        lastBoxCenter = boxCenter;

        float angle = Mathf.Atan2(facing.y, facing.x) * Mathf.Rad2Deg;

        Collider2D[] hits = Physics2D.OverlapBoxAll(boxCenter, boxSize, angle, interactableLayer);

        IInteractable newInteractable = null;
        float closestDist = float.MaxValue;

        foreach (var hit in hits)
        {
            IInteractable interactable = hit.GetComponentInParent<IInteractable>();
            if (interactable == null) continue;
            if (!interactable.CanInteract()) continue;

            float dist = Vector2.Distance(transform.position, hit.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                newInteractable = interactable;
            }
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
            ShowPrompt(currentInteractable.GetInteractionPrompt());
        else
            HidePrompt();
    }

    private void ShowPrompt(string prompt)
    {
        if (!promptVisible)
        {
            promptVisible = true;
            if (promptPanel != null) promptPanel.SetActive(true);
        }
        if (interactionPromptText != null) interactionPromptText.text = prompt;
    }

    private void HidePrompt()
    {
        if (promptVisible)
        {
            promptVisible = false;
            if (promptPanel != null) promptPanel.SetActive(false);
        }
    }

    private void HandleInteractionInput()
    {
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (currentInteractable != null && currentInteractable.CanInteract())
                currentInteractable.Interact(gameObject);
        }
    }

    void OnDrawGizmos()
    {
        Vector2 facing = Application.isPlaying
            ? (playerMove != null ? playerMove.facingDirection : Vector2.down)
            : Vector2.down;

        Vector2 center = Application.isPlaying
            ? lastBoxCenter
            : (Vector2)transform.position + facing * distanceInFront;

        Gizmos.color = currentInteractable != null ? Color.green : Color.yellow;

        Matrix4x4 oldMatrix = Gizmos.matrix;
        float angle = Mathf.Atan2(facing.y, facing.x) * Mathf.Rad2Deg;
        Gizmos.matrix = Matrix4x4.TRS(center, Quaternion.Euler(0, 0, angle), Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxSize);
        Gizmos.matrix = oldMatrix;
    }
}