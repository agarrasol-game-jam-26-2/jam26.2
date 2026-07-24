using UnityEngine;

public class LeverInteractable : MonoBehaviour, IInteractable
{
    [Header("Comportamento")]
    [SerializeField] private string interactionPrompt = "Pressione E para puxar a alavanca";
    [SerializeField] private bool isActivated = false;
    [SerializeField] private bool canInteractMultipleTimes = false;

    [Header("Animação da Alavanca")]
    [SerializeField] private Animator leverAnimator;
    [SerializeField] private string activateTrigger = "Activate";

    [Header("Alvos para Ativar")]
    [SerializeField] private Animator[] targetAnimators;
    [SerializeField] private string[] targetTriggers;

    [Header("GameObjects para Ativar/Desativar")]
    [SerializeField] private GameObject[] objectsToEnable;
    [SerializeField] private GameObject[] objectsToDisable;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip leverSound;
    [SerializeField] private AudioClip successSound;

    private Collider triggerCollider;

    void Start()
    {
        triggerCollider = GetComponent<Collider>();
        if (triggerCollider == null)
            Debug.LogError($"{gameObject.name}: LeverInteractable requer um Collider com isTrigger = true");
    }

    public string GetInteractionPrompt()
    {
        if (isActivated && !canInteractMultipleTimes)
            return "Alavanca já foi puxada";
        return interactionPrompt;
    }

    public void Interact(GameObject interactor)
    {
        if (isActivated && !canInteractMultipleTimes) return;

        isActivated = true;
        ActivateLever();
    }

    public bool CanInteract()
    {
        return !isActivated || canInteractMultipleTimes;
    }

    private void ActivateLever()
    {
        PlayLeverAnimation();
        PlayLeverSound();
        TriggerTargets();
        ToggleObjects();
        PlaySuccessSound();
    }

    private void PlayLeverAnimation()
    {
        if (leverAnimator != null)
            leverAnimator.SetTrigger(activateTrigger);
    }

    private void TriggerTargets()
    {
        if (targetAnimators.Length == 0) return;

        for (int i = 0; i < targetAnimators.Length; i++)
        {
            if (targetAnimators[i] != null && i < targetTriggers.Length)
            {
                targetAnimators[i].SetTrigger(targetTriggers[i]);
            }
        }
    }

    private void ToggleObjects()
    {
        foreach (GameObject obj in objectsToEnable)
            if (obj != null) obj.SetActive(true);

        foreach (GameObject obj in objectsToDisable)
            if (obj != null) obj.SetActive(false);
    }

    private void PlayLeverSound()
    {
        if (audioSource != null && leverSound != null)
            audioSource.PlayOneShot(leverSound);
    }

    private void PlaySuccessSound()
    {
        if (audioSource != null && successSound != null)
        {
            audioSource.PlayOneShot(successSound, 0.8f);
        }
    }

    public void Reset()
    {
        isActivated = false;
    }
}
