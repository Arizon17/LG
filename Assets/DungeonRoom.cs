using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

[Serializable]
public class DungeonRoom
{
    public List<DungeonRoom> connectedRooms = new List<DungeonRoom>();
    public string roomName;

    public enum RoomType
    {
        lootRoom = 0,
        fightRoom,
        fightForLoot,
        eventRoom
    }

    public RoomType roomType;

    public DungeonRoom(string name)
    {
        roomName = name;
        roomType = (RoomType)Random.Range(0, 4);
    }

    public void SpawnNeighbor(string name, DungeonRoom parent)
    {
        for (int i = 0; i < 3; i++)
        {
            parent.connectedRooms.Add(new DungeonRoom(name + "_" + i));
        }
    }
}
