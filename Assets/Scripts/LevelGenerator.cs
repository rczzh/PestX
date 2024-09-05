using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public LevelGenerationData levelGenerationData;
    private List<Vector2Int> levelRooms;

    private void Start()
    {
        levelRooms = LevelGenerationController.GenerateLevel(levelGenerationData);
        SpawnRooms(levelRooms);
    }

    private void SpawnRooms(IEnumerable<Vector2Int> rooms)
    {
        RoomController.instance.LoadRoom("Spawn", 0, 0);
        foreach (Vector2Int roomLocation in rooms)
        {
            RoomController.instance.LoadRoom(RoomController.instance.GetRandomRoomName(), roomLocation.x, roomLocation.y);
        }
    }
}
