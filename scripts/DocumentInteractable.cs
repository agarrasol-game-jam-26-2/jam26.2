using UnityEngine;

public class DocumentInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactionPrompt = "Pressione E para ler";
    [SerializeField] private Sprite[] documentPages;

    [Header("Reação (opcional)")]
    [SerializeField] private LoreData reactionLore;

    public string GetInteractionPrompt() => interactionPrompt;

    public bool CanInteract() => documentPages != null && documentPages.Length > 0;

    public void Interact(GameObject interactor)
    {
        if (!CanInteract()) return;

        DocumentPopup.Instance.OpenDocument(documentPages, () =>
        {
            if (reactionLore != null)
                LoreDisplayUI.Instance.Show(reactionLore);
        });
    }
}
