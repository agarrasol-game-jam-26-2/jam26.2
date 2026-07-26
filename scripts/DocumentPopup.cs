using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class DocumentPopup : MonoBehaviour
{
    public static DocumentPopup Instance { get; private set; }

    [Header("Referências")]
    public GameObject popupPanel;
    public Image pageDisplay;

    private Sprite[] pages;
    private int currentPage = 0;
    private Action onCloseCallback;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (popupPanel != null)
            popupPanel.SetActive(false);
    }

    public void OpenDocument(Sprite[] documentPages, Action onClose = null)
    {
        pages = documentPages;
        currentPage = 0;
        onCloseCallback = onClose;
        popupPanel.SetActive(true);
        ShowPage();
    }

    public void CloseDocument()
    {
        popupPanel.SetActive(false);
        onCloseCallback?.Invoke();
        onCloseCallback = null;
    }

    public void NextPage()
    {
        if (pages != null && currentPage < pages.Length - 1)
        {
            currentPage++;
            ShowPage();
        }
    }

    public void PreviousPage()
    {
        if (pages != null && currentPage > 0)
        {
            currentPage--;
            ShowPage();
        }
    }

    private void ShowPage()
    {
        if (pages != null && pages.Length > 0)
            pageDisplay.sprite = pages[currentPage];
    }

    void Update()
    {
        if (popupPanel != null && popupPanel.activeSelf && Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            CloseDocument();
    }
}
