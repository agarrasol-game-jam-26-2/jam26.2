using UnityEngine;

public class NPCPatrol : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float patrolDistance = 3f;
    [SerializeField] private bool patrolEnabled = true;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private int direction = 1;

    void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition + Vector3.right * patrolDistance;
    }

    void Update()
    {
        if (!patrolEnabled) return;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            direction *= -1;
            targetPosition = direction > 0
                ? startPosition + Vector3.right * patrolDistance
                : startPosition;
        }
    }
}
