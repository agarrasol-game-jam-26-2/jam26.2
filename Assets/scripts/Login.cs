using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public Button loginButton;

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
        // Aqui você pode adicionar a lógica de login, como autenticação do usuário.
        Debug.Log("Login realizado com sucesso!");
    }
}