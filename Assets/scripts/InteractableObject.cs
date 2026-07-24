using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour, IInteractable
{
    [Header("Interação")]
    [SerializeField] private string interactionPrompt = "Pressione E para interagir";
    [SerializeField] private bool canInteract = true;
    [SerializeField] private Collider interactableCollider;

    [Header("Animação (opcional)")]
    [SerializeField] private Animator animator;
    [SerializeField] private string triggerName = "";

    [Header("Som (opcional)")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip interactionSound;

    [Header("Eventos")]
    [SerializeField] private UnityEvent onInteract;

    void Start()
    {
        if (interactableCollider == null)
            interactableCollider = GetComponent<Collider>();

        if (interactableCollider != null && !interactableCollider.isTrigger)
            Debug.LogWarning($"{gameObject.name}: O Collider de interação deve ser Trigger!");
    }

    public string GetInteractionPrompt()
    {
        return interactionPrompt;
    }

    public void Interact(GameObject interactor)
    {
        if (!canInteract) return;

        PlayAnimation();
        PlaySound();
        onInteract?.Invoke();
    }

    public bool CanInteract()
    {
        return canInteract;
    }

    private void PlayAnimation()
    {
        if (animator != null && !string.IsNullOrEmpty(triggerName))
        {
            animator.SetTrigger(triggerName);
        }
    }

    private void PlaySound()
    {
        if (audioSource != null && interactionSound != null)
        {
            audioSource.PlayOneShot(interactionSound);
        }
    }

    public void SetCanInteract(bool value)
    {
        canInteract = value;
    }
}
