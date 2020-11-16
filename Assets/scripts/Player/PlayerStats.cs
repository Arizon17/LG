﻿using System;
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
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerStat.Health -= 1;
        }
        GameManager._instance.UpdateUI(playerStat.Health, playerStat.MaxHealth);
    }

    public void DealDamage(int damage)
    {
        if (playerStat.Health - damage > 0)
        playerStat.Health -= playerStat.Damage;
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
        byte[] result = new byte[6];

        BitConverter.GetBytes(playerStats.Damage).CopyTo(result, 0);
        BitConverter.GetBytes(playerStats.Health).CopyTo(result, 2);
        BitConverter.GetBytes(playerStats.MaxHealth).CopyTo(result, 4);

        return result;
    }

    public static object DeserializeStats(byte[] bytes)
    {
        Stats result = new Stats();
        result.Damage = BitConverter.ToUInt16(bytes, 0);
        result.Health = BitConverter.ToUInt16(bytes, 2);
        result.MaxHealth = BitConverter.ToUInt16(bytes, 4);
        return result;
    }
}
