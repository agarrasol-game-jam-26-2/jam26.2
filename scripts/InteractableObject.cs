using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour, IInteractable
{
    [Header("Interação")]
    [SerializeField] private string interactionPrompt = "Pressione E para interagir";
    [SerializeField] private bool canInteract = true;
    [SerializeField] private Collider2D interactableCollider;

    [Header("Animação (opcional)")]
    [SerializeField] private Animator animator;
    [SerializeField] private string triggerName = "";

    [Header("Som (opcional)")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip interactionSound;

    [Header("Transição de Sprite (opcional)")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite spriteA;
    [SerializeField] private Sprite spriteB;
    [SerializeField] private float transitionDuration = 0.5f;
    [SerializeField] private bool transitionOneTimeOnly = false;

    [Header("Lore (opcional)")]
    [SerializeField] private LoreData lore;

    [Header("Dormir (opcional)")]
    [SerializeField] private bool hasSleepAction = false;
    [SerializeField] private string sleepTriggerName = "Sleep";
    [SerializeField] private LoreData sleepLore;
    [SerializeField] private string sleepSceneName = "";

    [Header("Eventos")]
    [SerializeField] private UnityEvent onInteract;
    [SerializeField] private UnityEvent onSleep;

    private bool isTransitioning = false;
    private bool isShowingSpriteA = true;
    private bool spriteSwitched = false;
    private bool hasTransitioned = false;
    private float transitionTimer = 0f;
    private Color originalColor;

    void Start()
    {
        if (interactableCollider == null)
            interactableCollider = GetComponent<Collider2D>();

        if (interactableCollider != null && !interactableCollider.isTrigger)
            Debug.LogWarning($"{gameObject.name}: O Collider2D de interação deve ser Trigger!");

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    void Update()
    {
        if (isTransitioning)
            UpdateTransition();
    }

    private void StartTransition()
    {
        if (isTransitioning) return;
        isTransitioning = true;
        spriteSwitched = false;
        transitionTimer = 0f;
    }

    private void UpdateTransition()
    {
        transitionTimer += Time.deltaTime;
        float progress = Mathf.Clamp01(transitionTimer / transitionDuration);

        if (progress <= 0.5f)
        {
            float fadeProgress = progress * 2f;
            Color fadeColor = originalColor;
            fadeColor.a = 1f - fadeProgress;
            spriteRenderer.color = fadeColor;
        }
        else
        {
            if (!spriteSwitched)
            {
                spriteRenderer.sprite = isShowingSpriteA ? spriteB : spriteA;
                isShowingSpriteA = !isShowingSpriteA;
                spriteSwitched = true;
            }

            float fadeProgress = (progress - 0.5f) * 2f;
            Color fadeColor = originalColor;
            fadeColor.a = fadeProgress;
            spriteRenderer.color = fadeColor;
        }

        if (progress >= 1f)
        {
            isTransitioning = false;
            spriteRenderer.color = originalColor;
            hasTransitioned = true;
        }
    }

    public string GetInteractionPrompt()
    {
        if (hasSleepAction && TaskManager.AllTasksCompleted)
            return "Pressione E para dormir";

        return interactionPrompt;
    }

    public void Interact(GameObject interactor)
    {
        Debug.Log($"[InteractableObject] {gameObject.name} foi interagido!");

        if (!canInteract)
        {
            Debug.Log($"[InteractableObject] {gameObject.name} não pode ser interagido agora");
            return;
        }

        if (hasSleepAction && TaskManager.AllTasksCompleted)
        {
            ExecuteSleepAction();
            return;
        }

        if (transitionOneTimeOnly && hasTransitioned)
        {
            Debug.Log($"[InteractableObject] {gameObject.name} já foi transitado (one time only)");
            return;
        }

        PlayAnimation();
        PlaySound();
        StartTransition();

        if (lore != null)
        {
            Debug.Log($"[InteractableObject] Abrindo lore: {lore.name}");
            LoreDisplayUI.Instance.Show(lore);
        }
        else
        {
            Debug.Log($"[InteractableObject] {gameObject.name} não tem lore atribuído");
        }

        onInteract?.Invoke();

        InteractionHistory.RecordInteraction(gameObject, triggerName);
        TaskManager.CompleteTask();
        
    }

    private void ExecuteSleepAction()
    {
        if (animator != null && !string.IsNullOrEmpty(sleepTriggerName))
        {
            animator.SetTrigger(sleepTriggerName);
        }

        if (sleepLore != null)
        {
            Debug.Log($"[InteractableObject] Abrindo sleep lore: {sleepLore.name}");
            LoreDisplayUI.Instance.Show(sleepLore);
        }

        onSleep?.Invoke();

        InteractionHistory.RecordInteraction(gameObject, sleepTriggerName);
        Debug.Log($"[InteractableObject] {gameObject.name} - Ação de dormir executada!");

        if (!string.IsNullOrEmpty(sleepSceneName))
        {
            SceneTransitionManager.Instance.TransitionToScene(sleepSceneName, 1f);
        }
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
