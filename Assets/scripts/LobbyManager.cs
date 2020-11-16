using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button createRoomButton;

    [SerializeField] private Button joinRandomRoomButton;
    [SerializeField] private GameObject createRoomPanel;
public string Username { get; set; }
    void Start()
    {
        InitPlayer(Username);
        InitRooms();
    }

    void InitRooms()
    {
        createRoomButton.onClick.AddListener(() =>
        {
            CreateRoom();
        });
        joinRandomRoomButton.onClick.AddListener(() =>
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
        createRoomButton.enabled = true;
        joinRandomRoomButton.enabled = true;
        AmplitudeLogger.instance.LogEvent("Player connected", "player name", PhotonNetwork.NickName);
    }

    void CreateRoom()
    {
        createRoomPanel.SetActive(true);
        transform.parent.gameObject.SetActive(false);
        createRoomPanel.transform.GetChild(0).DOLocalMove(new Vector3(0, 0, 0), 1.25f);
    }

    void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
        AmplitudeLogger.instance.LogEvent("Player joined room", "player name", PhotonNetwork.CurrentRoom.Name);
    }

    void JoinCustomRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
