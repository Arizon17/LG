using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button CreateRoomButton;

    [SerializeField] private Button JoinRandomRoomButton;
    // Start is called before the first frame update
    void Start()
    {
        InitPlayer("sashawik");
        InitRooms();
    }

    void InitRooms()
    {
        CreateRoomButton.onClick.AddListener(() =>
        {
            CreateRoom(PhotonNetwork.NickName, 2);
        });
        JoinRandomRoomButton.onClick.AddListener(() =>
        {
            JoinRandomRoom();
        });
    }
    void InitPlayer(string nickname)
    {
        PhotonNetwork.NickName = nickname;
        Debug.Log("Player connected : " + PhotonNetwork.NickName);

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.ConnectUsingSettings();

    }

    public override void OnConnectedToMaster()
    {
#if UNITY_EDITOR
        Debug.Log("Connected to master");
#endif
        CreateRoomButton.enabled = true;
        JoinRandomRoomButton.enabled = true;
    }

    void CreateRoom(string PlayerRoom, byte MaxPlayerInRoom)
    {
        PhotonNetwork.CreateRoom(PlayerRoom + Random.Range(0,1000).ToString(), 
            new RoomOptions{MaxPlayers = MaxPlayerInRoom});
    }

    void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    void JoinCustomRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
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
