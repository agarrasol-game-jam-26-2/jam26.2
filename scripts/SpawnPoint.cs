using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public string spawnId;

    void Start()
    {
        if (string.IsNullOrEmpty(RoomTransitionData.NextSpawnId)) return;
        if (RoomTransitionData.NextSpawnId != spawnId) return;

        PlayerMove player = Object.FindFirstObjectByType<PlayerMove>();
        if (player != null)
            player.transform.position = transform.position;

        RoomTransitionData.NextSpawnId = null;
    }
}
