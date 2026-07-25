using UnityEngine;

public class ErrorPopupUI : MonoBehaviour
{
    public static ErrorPopupUI Instance { get; private set; }

    [SerializeField] private GameObject errorPanel;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        if (errorPanel != null)
            errorPanel.SetActive(false);
    }

    public void Show()
    {
        if (errorPanel != null)
            errorPanel.SetActive(true);
    }

    public void Hide()
    {
        if (errorPanel != null)
            errorPanel.SetActive(false);
    }
}
