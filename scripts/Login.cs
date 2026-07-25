using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    public Button loginButton;
    public Object targetScene;

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
    if (targetScene == null)
    {
        Debug.LogError("Login: Cena não atribuída no Inspector!");
        return;
    }

    string sceneName = targetScene.name;
    Debug.Log("Login realizado com sucesso! Carregando cena: " + sceneName);

    if (SceneTransitionManager.Instance != null)
        SceneTransitionManager.Instance.TransitionToScene(sceneName);
    else
        SceneManager.LoadScene(sceneName);
}
}
