using UnityEngine;
using UnityEngine.UI;

public class Logout : MonoBehaviour
{
    public Button logoutButton;

    void Start()
    {
        if (logoutButton != null)
        {
            logoutButton.onClick.AddListener(QuitGame);
        }
        else
        {
            Debug.LogWarning("Logout: Botão não atribuído no Inspector!");
        }
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
