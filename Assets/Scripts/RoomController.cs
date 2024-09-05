using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class RoomInfo
{
    public string name;
    public int X;
    public int Y;
}

public class RoomController : MonoBehaviour
{
    public static RoomController instance;

    public string currentLevelName = "BackAlley";

    RoomInfo currentLoadRoomData;

    Room currentRoom;

    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();

    public List<Room> loadedRooms = new List<Room> ();

    bool isLoadingRoom = false;

    bool spanwedBossRoom = false;

    bool updatedRooms = false;

    private void Awake()
    {
        instance = this;
    }

    public void LoadRoom(string name, int x, int y)
    {
        //print("loadRoom");
        // checking cords to prevent spawning rooms that may overlap
        if (DoesRoomExist(x, y))
        {
            return;
        }

        RoomInfo newRoomData = new RoomInfo();
        newRoomData.name = name;
        newRoomData.X = x;
        newRoomData.Y = y;

        //print("Enqueue Rooms");
        loadRoomQueue.Enqueue(newRoomData);
    }

    IEnumerator LoadRoomRoutine(RoomInfo info)
    {
        //print("LoadRoomNew");

        string roomName = currentLevelName + info.name;

        AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);

        while (loadRoom.isDone != true)
        {
            yield return null;
        }
    }

    public void RegisterRoom(Room room)
    {
        //print("RegisterRoom");
        if (!DoesRoomExist(currentLoadRoomData.X, currentLoadRoomData.Y))
        {
            room.transform.position = new Vector3(currentLoadRoomData.X * room.Width, currentLoadRoomData.Y * room.Height, 0);
            room.X = currentLoadRoomData.X;
            room.Y = currentLoadRoomData.Y;
            room.name = currentLevelName + "-" + currentLoadRoomData.name + " " + room.X + ", " + room.Y;
            room.transform.parent = transform;

            isLoadingRoom = false;

            if (loadedRooms.Count == 0)
            {
                CameraController.instance.currentRoom = room;
            }

            loadedRooms.Add(room);
        }
        else
        {
            Destroy(room.gameObject);
            isLoadingRoom = false;
        }
    }

    public bool DoesRoomExist(int x, int y)
    {
        return loadedRooms.Find(item => item.X == x && item.Y == y) != null;
    }

    public Room FindRoom(int x, int y)
    {
        return loadedRooms.Find(item => item.X == x && item.Y == y);
    }

    public string GetRandomRoomName()
    {
        string[] possibleRooms = new string[]
        {
            "Empty"
        };
        return possibleRooms[Random.Range(0, possibleRooms.Length)];
    }

    private void Update()
    {
        UpdateRoomQueue();
    }

    private void UpdateRoomQueue()
    {
        if (isLoadingRoom)
        {
            //print("isLoadingRoom");
            return;
        }
        
        if (loadRoomQueue.Count == 0)
        {
            //print("loadRoomQueue == 0");
            if (!spanwedBossRoom)
            {
                //print("spawnBossRoom");
                StartCoroutine(SpawnBossRoom());
            }
            else if (spanwedBossRoom && !updatedRooms)
            {
                foreach (Room room in loadedRooms)
                {
                    room.RemoveUnconnectedDoors();
                }
                updatedRooms = true;
            }
            return;
        }

        currentLoadRoomData = loadRoomQueue.Dequeue();
        //Debug.Log(currentLoadRoomData);

        isLoadingRoom = true;

        StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
    }

    public void OnPlayerEntersRoom(Room room)
    {
        CameraController.instance.currentRoom = room;
        currentRoom = room;
    }

    IEnumerator SpawnBossRoom()
    {
        spanwedBossRoom = true;
        yield return new WaitForSeconds(0.5f);
        if (loadRoomQueue.Count == 0)
        {
            Room bossRoom = loadedRooms[loadedRooms.Count - 1];
            Vector2Int tempRoom = new Vector2Int(bossRoom.X, bossRoom.Y);
            Destroy(bossRoom.gameObject);
            var roomToRemove = loadedRooms.Single(r => r.X == tempRoom.x && r.Y == tempRoom.y);
            loadedRooms.Remove(roomToRemove);
            LoadRoom("End", tempRoom.x, tempRoom.y);
        }
    }
}
