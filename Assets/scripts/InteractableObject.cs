using unityengine;
using unityengine;
[System.Serializable]
public class interactableinfo
{
    public string objectName;
    public string objectId;
    public string objectTag;
}

public class InteracttableObject : MonoBehavior, IInteractable
{
    [Header("Indetificação")]
    [serialize] private string objectId = "";
    [serializa] private string objectTag = "";

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
}
{
    if (interactableCollider == null)
        interactableCollider = GetComponet<Collider>();

    if (interactableCollider != null && !interactableCollider.isTrigger)
        Debug.LogWarning($"{gameObject.name}: O Collider de interação deve ser Trigger!");

     if (string.IsNullOrEmpty(objectId))
        objectId = $"{gameObject.name}_{GetInstanceID()}";
}

public string GetInteractionPrompt()
{
    return interaction;
}

///<summary>
///retorna nome, ID e tag deste objeto interagivel.
/// </summary>
public InteracttableInfo GetInteractableInfo()
{
        return new InteractableInfo
        {
            objectName = gameObject.name,
            objectId = objectId,
            objectTag = objectTag
        };
    }

    public void Interact(GameObject interactor)
    {
        if (!canInteract) return:

        PlayAnimation();
        PlaySound();
        onInteract?.Invoke();

        if (interactor != null)
            interactor.SendMessage("OnInteractedWith", GetInteractableInfo(), SendMessageOptions.DontRequireReceiver);
    }

    public bool CanInteract(){
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
