using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomPanel : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button CreateRoomButton;

    [SerializeField] private Dropdown PlayerCount;

    [SerializeField] private InputField RoomName;
    // Start is called before the first frame update
    void Start()
    {
        CreateRoomButton.onClick.AddListener(() =>
        {
            int playerCount = PlayerCount.value + 2;
            string name = RoomName.text;
            PhotonNetwork.CreateRoom(name, 
                new RoomOptions{MaxPlayers = (byte)playerCount});
        });
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        PhotonNetwork.LoadLevel("Dungeon");
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
