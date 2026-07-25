using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;

public class LoreDisplayUI : MonoBehaviour
{
    public static LoreDisplayUI Instance { get; private set; }
    public static bool IsOpen { get; private set; }

    [SerializeField] private GameObject lorePanel;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI bodyText;
    [SerializeField] private float charDelay = 0.05f;

    private Coroutine typewriterCoroutine;
    private bool justOpened = false;

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
        if (lorePanel != null)
            lorePanel.SetActive(false);
        IsOpen = false;
    }

    void Update()
    {
        if (justOpened)
        {
            justOpened = false;
            return;
        }

        if (IsOpen && Keyboard.current != null)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame || Keyboard.current.escapeKey.wasPressedThisFrame)
                Hide();
        }
    }

    public void Show(LoreData data)
    {
        if (data == null) return;

        if (typewriterCoroutine != null)
            StopCoroutine(typewriterCoroutine);

        if (titleText != null)
            titleText.text = data.title;

        if (bodyText != null)
            bodyText.text = "";

        if (lorePanel != null)
            lorePanel.SetActive(true);

        IsOpen = true;
        justOpened = true;
        typewriterCoroutine = StartCoroutine(TypewriterEffect(data.body));
    }

    public void Hide()
    {
        if (typewriterCoroutine != null)
            StopCoroutine(typewriterCoroutine);

        if (lorePanel != null)
            lorePanel.SetActive(false);
        IsOpen = false;
    }

    private IEnumerator TypewriterEffect(string text)
    {
        for (int i = 0; i <= text.Length; i++)
        {
            if (bodyText != null)
                bodyText.text = text.Substring(0, i);
            yield return new WaitForSeconds(charDelay);
        }
    }
}
