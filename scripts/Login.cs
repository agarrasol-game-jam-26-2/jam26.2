using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    public Button loginButton;
    public string targetSceneName;

    void Start()
    {
        if (loginButton != null)
        {
            loginButton.onClick.AddListener(LoginGame);
        }
        else
        {
            Debug.LogWarning("Login: Botão não atribuído no Inspector!");
        }
    }

    public void LoginGame()
    {
        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogError("Login: Nome da cena não atribuído no Inspector!");
            return;
        }

        Debug.Log("Login realizado com sucesso! Carregando cena: " + targetSceneName);

        if (SceneTransitionManager.Instance != null)
            SceneTransitionManager.Instance.TransitionToScene(targetSceneName);
        else
            SceneManager.LoadScene(targetSceneName);
    }
}