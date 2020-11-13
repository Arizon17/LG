using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace FamilyWikGame
{
public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform StartPosition;
    public GameObject PlayerPferab;
    // Start is called before the first frame update
        void Start()
        {
            InitPlayer();
        }

        void InitPlayer()
        {
            PhotonNetwork.Instantiate(PlayerPferab.name, StartPosition.position, Quaternion.identity);
        }
    // Update is called once per frame
        void Update()
        {
            
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log(newPlayer.NickName);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
             Debug.Log(otherPlayer.NickName);
        }
}
}
