using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonRoomHolder : MonoBehaviour
{
    public DungeonRoom room;
    [SerializeField] private Image roomIcon;
    [SerializeField] private Sprite[] icons;

    public void Init(Vector3 pos)
    {
        gameObject.AddComponent<LineRenderer>().SetPosition(0,pos);
        GetComponent<LineRenderer>().SetPosition(1, Vector3.zero);
        GetComponent<LineRenderer>().useWorldSpace = false;
        GetComponent<LineRenderer>().startColor = Color.green;

        name = room.roomName;
        ChangeIcon();
    }

    public void HideImage()
    {
        roomIcon.gameObject.SetActive(false);
    }
    void ChangeIcon()
    {
        switch (room.roomType)
        {
            case DungeonRoom.RoomType.eventRoom :
                roomIcon.sprite = icons[2]; break;
            case DungeonRoom.RoomType.fightRoom :
                roomIcon.sprite = icons[0]; break;
            case DungeonRoom.RoomType.lootRoom :
                roomIcon.sprite = icons[1]; break;
            case DungeonRoom.RoomType.fightForLoot :
                roomIcon.sprite = icons[3]; break;
        }
    }
}
