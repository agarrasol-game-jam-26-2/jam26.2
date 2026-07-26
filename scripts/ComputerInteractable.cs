using UnityEngine;

public class ComputerInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactionPrompt = "Pressione E para usar o computador";
    [SerializeField] private LoreData finalReport;

    public string GetInteractionPrompt() => interactionPrompt;
    public bool CanInteract() => true;

    public void Interact(GameObject interactor)
    {
        if (finalReport != null)
            LoreDisplayUI.Instance.Show(finalReport);

        ConfirmPopupUI.Instance.Show(
            yesCallback: () => Debug.Log("Deletar arquivos: SIM"),
            noCallback: () => Debug.Log("Deletar arquivos: NAO")
        );
    }
}