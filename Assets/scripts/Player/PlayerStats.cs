using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using FamilyWikGame;
using Photon.Pun;
using UnityEngine;

public class PlayerStats : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private Stats playerStat;
    [SerializeField] private PhotonView photonView;
    // Start is called before the first frame update
    void Start()
    {
        playerStat = new Stats();
        photonView = GetComponent<PhotonView>();
        PhotonPeer.RegisterType(typeof(Stats), (byte)244, SerializeStats, DeserializeStats);
        InitHealthBarPanel();
    }

    void InitHealthBarPanel()
    {
        PlayerStats Player = this;
        HealthBarPanel healthBar = GameManager._instance.GetHealthBar();
        if (!photonView.IsMine)
        {
            healthBar = GameManager._instance.GetUnactivePartyHealthBar();
            healthBar.gameObject.SetActive(true);
            Player = GameManager._instance.GetAnotherPlayer(photonView.Owner);
            healthBar.SetPlayer(Player);
        }
        
        
        healthBar.SetNickName(Player.photonView.Owner.NickName);
        healthBar.UpdateHealthBar(Player.GetHealth(), Player.GetMaxHealth());
        healthBar.UpdateManaBar(Player.GetMana(), Player.GetMaxMana());
    }

    public override void OnDisable()
    {
        GameManager._instance.GetHealthBarByPlayer(this).gameObject.SetActive(false);
        GameManager._instance.RemovePlayer(GetComponent<PlayerControlls>());
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DealDamage(10);
        }
    }
    
    public void DealDamage(int damage)
    {
        if (playerStat.Health - damage > 0)
        playerStat.Health -= playerStat.Damage;
        GameManager._instance.GetHealthBar().UpdateHealthBar(playerStat.Health, playerStat.MaxHealth);
    }

    public float GetMaxHealth()
    {
        return playerStat.MaxHealth;
    }
    public int GetDamage()
    {
        return playerStat.Damage;
    }
    public ushort GetHealth()
    {
        return playerStat.Health;
    }

    public ushort GetMana()
    {
        return playerStat.Mana;
    }

    public ushort GetMaxMana()
    {
        return playerStat.MaxMana;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(playerStat);
        }

        if (stream.IsReading)
        {
            playerStat = (Stats) stream.ReceiveNext();
        }
    }
    
    public static byte[] SerializeStats(object obj)
    {
        Stats playerStats = (Stats) obj;
        byte[] result = new byte[10];

        BitConverter.GetBytes(playerStats.Damage).CopyTo(result, 0);
        BitConverter.GetBytes(playerStats.Health).CopyTo(result, 2);
        BitConverter.GetBytes(playerStats.MaxHealth).CopyTo(result, 4);
        BitConverter.GetBytes(playerStats.MaxMana).CopyTo(result, 6);
        BitConverter.GetBytes(playerStats.Mana).CopyTo(result,8);

        return result;
    }

    public static object DeserializeStats(byte[] bytes)
    {
        Stats result = new Stats();
        result.Damage = BitConverter.ToUInt16(bytes, 0);
        result.Health = BitConverter.ToUInt16(bytes, 2);
        result.MaxHealth = BitConverter.ToUInt16(bytes, 4);
        result.MaxMana = BitConverter.ToUInt16(bytes, 6);
        result.MaxMana = BitConverter.ToUInt16(bytes, 8);
        
        return result;
    }
}
