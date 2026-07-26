using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private int totalTasks = 0;
    [SerializeField] private int completedTasks = 0;

    private static TaskManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void CompleteTask()
    {
        if (instance == null) return;

        if (instance.completedTasks < instance.totalTasks)
        {
            instance.completedTasks++;
            Debug.Log($"[TASK] Tarefa concluída! {instance.completedTasks}/{instance.totalTasks}");
        }
    }

    public static bool AllTasksCompleted
    {
        get
        {
            if (instance == null) return false;
            return instance.totalTasks > 0 && instance.completedTasks >= instance.totalTasks;
        }
    }

    public static (int completed, int total) GetProgress()
    {
        if (instance == null) return (0, 0);
        return (instance.completedTasks, instance.totalTasks);
    }
}
