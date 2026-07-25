using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class InteractionRecord
{
    public string objectName;
    public GameObject interactedObject;
    public string animationTriggered;
    public float timestamp;

    public InteractionRecord(string name, GameObject obj, string anim, float time)
    {
        objectName = name;
        interactedObject = obj;
        animationTriggered = anim;
        timestamp = time;
    }
}

public class InteractionHistory : MonoBehaviour
{
    [SerializeField] private List<InteractionRecord> history = new List<InteractionRecord>();
    [SerializeField] private int maxHistorySize = 20;

    private static InteractionHistory instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public static void RecordInteraction(GameObject obj, string animationName)
    {
        if (instance == null) return;

        InteractionRecord record = new InteractionRecord(
            obj.name,
            obj,
            animationName,
            Time.time
        );

        instance.history.Add(record);

        if (instance.history.Count > instance.maxHistorySize)
            instance.history.RemoveAt(0);

        Debug.Log($"[INTERAÇÃO] {obj.name} | Animação: {animationName}");
    }

    public List<InteractionRecord> GetHistory()
    {
        return history;
    }

    public InteractionRecord GetLastInteraction()
    {
        return history.Count > 0 ? history[history.Count - 1] : null;
    }

    public void ClearHistory()
    {
        history.Clear();
    }

    public int GetInteractionCount()
    {
        return history.Count;
    }

    public int GetInteractionCountByObject(string objectName)
    {
        return history.FindAll(r => r.objectName == objectName).Count;
    }
}
