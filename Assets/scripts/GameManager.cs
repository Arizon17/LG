using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace FamilyWikGame
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Transform startPosition;
        public GameObject playerPferab;
        [SerializeField] private Image PlayerHealth;

        public static GameManager _instance;

        // Start is called before the first frame update
        void Start()
        {
            InitPlayer();
            Init();
        }
        public void UpdateUI(float health, float maxHealth)
        {
            PlayerHealth.fillAmount = health / maxHealth;
        }
        void Init()
        {
            if (_instance == null)
                _instance = this;
            else Destroy(gameObject);
            DontDestroyOnLoad(this);
        }
        

        // Update is called once per frame
        void Update()
        {
            
        }

        void InitPlayer()
        {
            PhotonNetwork.Instantiate(playerPferab.name, startPosition.position, Quaternion.identity);
        }

        public void ShowChat()
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