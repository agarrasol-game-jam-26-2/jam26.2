using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Quests/Quest Data", fileName = "new_quest")]
public class QuestData : ScriptableObject
{
    [Header("Informações da Quest")]
    public string questName;

    [TextArea(3, 5)]
    public string description;

    [Header("Objetivos")]
    public List<QuestObjective> objectives = new List<QuestObjective>();

    public bool isCompleted
    {
        get
        {
            if (objectives.Count == 0) return false;
            foreach (var objective in objectives)
            {
                if (!objective.isCompleted) return false;
            }
            return true;
        }
    }
}

[System.Serializable]
public class QuestObjective
{
    public string description;
    public bool isCompleted;
}
