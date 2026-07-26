using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactionPrompt = "Pressione E para conversar";
    [SerializeField] private NPCDialogue dialogue;
    [SerializeField] private bool loopDialogue = true;

    [Header("Animação")]
    [SerializeField] private Animator animator;
    [SerializeField] private string talkTrigger = "Talk";
    [SerializeField] private bool faceInteractor = true;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Ao terminar o diálogo (opcional)")]
    [SerializeField] private LoreData reportPopup;
    [SerializeField] private GameObject doorToUnlock;

    private int currentLine = 0;
    private bool dialogueCompleted = false;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public string GetInteractionPrompt()
    {
        return interactionPrompt;
    }

    public bool CanInteract()
    {
        return dialogue != null && dialogue.lines.Length > 0;
    }

    public void Interact(GameObject interactor)
    {
        if (!CanInteract() || dialogueCompleted) return;

        bool isLastLine = currentLine >= dialogue.lines.Length - 1;

        if (isLastLine && !loopDialogue)
        {
            dialogueCompleted = true;

            if (reportPopup != null)
                LoreDisplayUI.Instance.Show(reportPopup);

            if (doorToUnlock != null)
                doorToUnlock.SetActive(true);

            return;
        }

        if (faceInteractor && spriteRenderer != null)
        {
            bool interactorIsLeft = interactor.transform.position.x < transform.position.x;
            spriteRenderer.flipX = interactorIsLeft;
        }

        if (animator != null && !string.IsNullOrEmpty(talkTrigger))
            animator.SetTrigger(talkTrigger);

        LoreData tempLine = ScriptableObject.CreateInstance<LoreData>();
        tempLine.title = dialogue.lines[currentLine].speaker;
        tempLine.body = dialogue.lines[currentLine].text;

        LoreDisplayUI.Instance.Show(tempLine);

        currentLine++;
    }
}