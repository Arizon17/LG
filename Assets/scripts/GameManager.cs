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
        [SerializeField] private HealthBarPanel HealthBarPanel;
        [SerializeField] private List<PlayerControlls> PlayerList;
        [SerializeField] private List<HealthBarPanel> PartyHealthBar;
        public DungeonItemSet dungeonItemSet;
        public ItemSO itemSo;
        [SerializeField] private CreateInventory inventoryVisualize;
        private bool isInventoryShow = false;
    
        public static GameManager _instance;
        
        public void ShowInventory(Inventory inventory)
        {
            if (!isInventoryShow)
            {
                HealthBarPanel.gameObject.SetActive(false);
                PartyHealthBar.ForEach(p => p.gameObject.SetActive(false));
                inventoryVisualize.Fire(inventory);
                isInventoryShow = true;
            }
            else
            {
                HealthBarPanel.gameObject.SetActive(true);
                PartyHealthBar.ForEach(p =>
                {
                    if (p.GetPlayer())
                        p.gameObject.SetActive(true);
                        
                });
                inventoryVisualize.Hide();
                isInventoryShow = false;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            InitPlayer();
            Init();
        }

        public PlayerStats GetAnotherPlayer(Player player)
        {
            return PlayerList.First(p => p.photonView.Owner.Equals(player)).GetComponent<PlayerStats>();
        }
        public HealthBarPanel GetHealthBar()
        {
            return HealthBarPanel;
        }
        public HealthBarPanel GetUnactivePartyHealthBar()
        {
            return PartyHealthBar.First(p => p.gameObject.activeInHierarchy == false);
        }
        public void SetName(string text)
        {
            HealthBarPanel.SetNickName(text);
        }
        void Init()
        {
            if (_instance == null)
                _instance = this;
            else Destroy(gameObject);
            DontDestroyOnLoad(this);
        }

        public void AddPlayer(PlayerControlls Player)
        {
            PlayerList.Add(Player);
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
            Debug.Log("Player Left Room : " + otherPlayer.NickName);
        }
        public HealthBarPanel GetHealthBarByPlayer(PlayerStats player)
        {
            return PartyHealthBar.First(p => p.GetPlayer().Equals(player));
        }

        public void RemovePlayer(PlayerControlls player)
        {
            PlayerList.Remove(player);
        }
    }
}