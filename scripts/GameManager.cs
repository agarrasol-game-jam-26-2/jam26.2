using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Estado das Tarefas")]
    public List<QuestData> activeQuests = new List<QuestData>();
    public List<QuestData> completedQuests = new List<QuestData>();

    private void Awake()
    {
        // Garante que só existe UMA instância viva
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartQuest(QuestData quest)
    {
        if (!activeQuests.Contains(quest))
        {
            activeQuests.Add(quest);
        }
    }

    public void CompleteQuest(QuestData quest)
    {
        if (activeQuests.Remove(quest))
        {
            completedQuests.Add(quest);
        }
    }
}
