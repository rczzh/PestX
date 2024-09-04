using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    Vector2 levelSize = new Vector2(4, 4);
    Room[,] rooms;
    List<Vector2> takenPositions = new List<Vector2>();
    int gridSizeX, gridSizeY, numberOfRooms = 5;

    public GameObject roomWhiteObj;

    // Start is called before the first frame update
    void Start()
    {
        // check if number of possible rooms is greater than the grid size
        if (numberOfRooms >= (levelSize.x * 2) * (levelSize.y * 2))
        {
            numberOfRooms = Mathf.RoundToInt((levelSize.x * 2) * (levelSize.y * 2));
        }
        gridSizeX = Mathf.RoundToInt(levelSize.x);
        gridSizeY = Mathf.RoundToInt(levelSize.y);

        CreateRooms();
        MarkRooms();
        SetRoomDoors();
        DrawMap();
    }

    void CreateRooms()
    {
        rooms = new Room[gridSizeX * 2, gridSizeY * 2];
        // creates starting room
        rooms[gridSizeX, gridSizeY] = new Room(Vector2.zero, 1);
        takenPositions.Insert(0, Vector2.zero);
        Vector2 checkPos = Vector2.zero;

        // numbers to prevent clumping
        float randomCompare = 0.2f, randomCompareStart = 0.2f, randomCompareEnd = 0.01f;

        // add rooms
        for (int i = 0; i < numberOfRooms - 1; i++)
        {
            
            // as we go further from the origin, the rooms are less likely to branch out
            float randomPerc = ((float) i) / (((float)numberOfRooms - 1));
            randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPerc);

            //new position
            checkPos = NewPosition();

            if (NumberOfNeighbours(checkPos, takenPositions) > 1 && Random.value > randomCompare)
            {
                int iterations = 0;
                do
                {
                    checkPos = SelectiveNewPosition();
                    iterations++;
                }
                while (NumberOfNeighbours(checkPos, takenPositions) > 1 && iterations < 100);
                if (iterations >= 50)
                {
                    print("Error");
                }
            }

            // creates normal rooms
            rooms[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeY] = new Room(checkPos, 0);
            takenPositions.Insert(0, checkPos);
        }
    }

    Vector2 NewPosition()
    {
        int x = 0, y = 0;
        Vector2 checkingPos = Vector2.zero;
        do
        {
            int index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
            x = (int)takenPositions[index].x;
            y = (int)takenPositions[index].y;
            bool UpDown = (Random.value < 0.5f);
            bool positive = (Random.value < 0.5f);

            if (UpDown)
            {
                if (positive)
                {
                    y += 1;
                }
                else
                {
                    y -= 1;
                }
            }
            else
            {
                if (positive)
                {
                    x += 1;
                }
                else
                {
                    x -= 1;
                }
            }
            checkingPos = new Vector2(x, y);
        }
        while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);
        return checkingPos;
    }

    Vector2 SelectiveNewPosition()
    {
        int index = 0, inc = 0;
        int x = 0, y = 0;
        Vector2 checkingPos = Vector2.zero;
        do
        {
            inc = 0;
            do
            {
                index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
                inc++;
            }
            while (NumberOfNeighbours(takenPositions[index], takenPositions) > 1 && inc < 100);

            x = (int)takenPositions[index].x;
            y = (int)takenPositions[index].y;
            bool UpDown = (Random.value < 0.5f);
            bool positive = (Random.value < 0.5f);

            if (UpDown)
            {
                if (positive)
                {
                    y += 1;
                }
                else
                {
                    y -= 1;
                }
            }
            else
            {
                if (positive)
                {
                    x += 1;
                }
                else
                {
                    x -= 1;
                }
            }
            checkingPos = new Vector2(x, y);
        }
        while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);
        if (inc >= 100)
        {
            print("Error");
        }
        return checkingPos;
    }

    int NumberOfNeighbours(Vector2 checkingPos, List<Vector2> usedPositions)
    {
        int ret = 0;
        if (usedPositions.Contains(checkingPos + Vector2.right))
        {
            ret++;
        }
        if (usedPositions.Contains(checkingPos + Vector2.left))
        {
            ret++;
        }
        if (usedPositions.Contains(checkingPos + Vector2.up))
        {
            ret++;
        }
        if (usedPositions.Contains(checkingPos + Vector2.down))
        {
            ret++;
        }
        return ret;
    }

    void MarkRooms()
    {
        List<Room> viableRooms = new List<Room>();

        foreach (var room in rooms)
        {
            int ret = 0;
            if (room == null)
            {
                continue;
            }

            foreach (var takenPos in takenPositions)
            {
                if (room.gridPos + Vector2.up == takenPos)
                {
                    ret++;
                }
                if (room.gridPos + Vector2.down == takenPos)
                {
                    ret++;
                }
                if (room.gridPos + Vector2.left == takenPos)
                {
                    ret++;
                }
                if (room.gridPos + Vector2.right == takenPos)
                {
                    ret++;
                }
            }

            // rooms with only 1 neighbour and not the spawn room
            if (ret == 1 && room.gridPos != Vector2.zero)
            {
                viableRooms.Add(room);
            }
        }

        // pick a room from viableRooms to be boss room
        // removes the room from viableRoom pool
        var randomInt1 = Random.Range(0, viableRooms.Count);
        viableRooms[randomInt1].type = 2;
        viableRooms.Remove(viableRooms[randomInt1]);

        // pick a room from viableRooms to be treasure room
        // removes the room from viableRoom pool
        var randomInt2 = Random.Range(0, viableRooms.Count);
        viableRooms[randomInt2].type = 3;
        viableRooms.Remove(viableRooms[randomInt2]);
    }

    void SetRoomDoors()
    {
        for (int x = 0; x < ((gridSizeX * 2)); x ++)
        {
            for (int y = 0; y < ((gridSizeY * 2)); y ++)
            {
                if (rooms[x, y] == null)
                {
                    continue;
                }
                Vector2 gridPosition = new Vector2(x, y);

                if (y - 1 < 0)
                {
                    rooms[x, y].doorBottom = false;
                }
                else
                {
                    rooms[x, y].doorBottom = (rooms[x, y - 1] != null);
                }

                if (y + 1 >= gridSizeY * 2)
                {
                    rooms[x, y].doorTop = false;
                }
                else
                {
                    rooms[x, y].doorTop = (rooms[x, y + 1] != null);
                }

                if (x - 1 < 0)
                {
                    rooms[x, y].doorLeft = false;
                }
                else
                {
                    rooms[x, y].doorLeft = (rooms[x - 1, y] != null);
                }

                if (x + 1 >= gridSizeX * 2)
                {
                    rooms[x, y].doorRight = false;
                }
                else
                {
                    rooms[x, y].doorRight = (rooms[x + 1, y] != null);
                }
            }
        }
    }
    void DrawMap()
    {
        foreach (Room room in rooms)
        {
            if (room == null)
            {
                continue;
            }
            Vector2 drawPos = room.gridPos;
            drawPos.x *= 16;
            drawPos.y *= 8;
            MapSpriteSelector mapper = Object.Instantiate(roomWhiteObj, drawPos, Quaternion.identity).GetComponent<MapSpriteSelector>();
            mapper.type = room.type;
            mapper.up = room.doorTop;
            mapper.down = room.doorBottom;
            mapper.right = room.doorRight;
            mapper.left = room.doorLeft;
        }
    }
}
