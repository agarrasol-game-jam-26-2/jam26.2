using UnityEngine;
using System;

public class ConfirmPopupUI : MonoBehaviour
{
    public static ConfirmPopupUI Instance { get; private set; }

    [SerializeField] private GameObject popupPanel;

    private Action onYes;
    private Action onNo;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (popupPanel != null)
            popupPanel.SetActive(false);
    }

    public void Show(Action yesCallback, Action noCallback)
    {
        onYes = yesCallback;
        onNo = noCallback;
        popupPanel.SetActive(true);
    }

    public void AnswerYes()
    {
        popupPanel.SetActive(false);
        onYes?.Invoke();
    }

    public void AnswerNo()
    {
        popupPanel.SetActive(false);
        onNo?.Invoke();
    }
}