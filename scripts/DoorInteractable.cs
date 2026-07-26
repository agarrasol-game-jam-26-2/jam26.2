using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactionPrompt = "Pressione E para abrir a porta";
    [SerializeField] private string targetSceneName;
    [SerializeField] private string targetSpawnId;

    public string GetInteractionPrompt() => interactionPrompt;

    public bool CanInteract() => !string.IsNullOrEmpty(targetSceneName);

    public void Interact(GameObject interactor)
    {
        if (!CanInteract()) return;

        RoomTransitionData.NextSpawnId = targetSpawnId;

        if (SceneTransitionManager.Instance != null)
            SceneTransitionManager.Instance.TransitionToScene(targetSceneName);
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene(targetSceneName);
    }
}
