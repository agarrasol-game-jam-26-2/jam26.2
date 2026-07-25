using UnityEngine;

public class ButtonInteractable : MonoBehaviour, IInteractable
{
    [Header("Botão")]
    [SerializeField] private string interactionPrompt = "Pressione E para pressionar o botão";
    [SerializeField] private bool oneTimeOnly = false;
    [SerializeField] private bool isPressed = false;

    [Header("Efeitos")]
    [SerializeField] private Animator buttonAnimator;
    [SerializeField] private string pressTrigger = "Press";
    [SerializeField] private Light buttonLight;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pressSound;

    [Header("Lore (opcional)")]
    [SerializeField] private LoreData lore;

    [Header("Targets")]
    [SerializeField] private GameObject[] poweredObjects;

    void Start()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;
    }

    public string GetInteractionPrompt()
    {
        if (isPressed && oneTimeOnly)
            return "Botão já foi acionado";
        return interactionPrompt;
    }

    public void Interact(GameObject interactor)
    {
        if (isPressed && oneTimeOnly) return;

        isPressed = true;
        PressButton();
    }

    public bool CanInteract()
    {
        return !isPressed || !oneTimeOnly;
    }

    private void PressButton()
    {
        if (buttonAnimator != null)
            buttonAnimator.SetTrigger(pressTrigger);

        if (audioSource != null && pressSound != null)
            audioSource.PlayOneShot(pressSound);

        if (lore != null)
            LoreDisplayUI.Instance.Show(lore);

        ActivatePower();
        InteractionHistory.RecordInteraction(gameObject, pressTrigger);
    }

    private void ActivatePower()
    {
        if (buttonLight != null)
            buttonLight.intensity = 1f;

        foreach (GameObject obj in poweredObjects)
        {
            if (obj != null)
                obj.SetActive(true);
        }
    }
}
