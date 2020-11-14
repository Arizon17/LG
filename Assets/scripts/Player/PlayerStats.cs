using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IPunObservable
{
    [SerializeField] private Stats playerStat = new Stats(); 
    // Start is called before the first frame update
    void Start()
    {
        PhotonPeer.RegisterType(typeof(Stats), (byte)244, SerializeStats, DeserializeStats);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerStat.Health += 1;
        }
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
            Debug.Log(" From write : " + playerStat.Health);
        }

        if (stream.IsReading)
        {
            playerStat = (Stats) stream.ReceiveNext();
            Debug.Log(" From read : " + playerStat.Health);
        }
    }
    
    public static byte[] SerializeStats(object obj)
    {
        Stats playerStats = (Stats) obj;
        byte[] result = new byte[4];

        BitConverter.GetBytes(playerStats.Damage).CopyTo(result, 0);
        BitConverter.GetBytes(playerStats.Health).CopyTo(result, 2);

        return result;
    }

    public static object DeserializeStats(byte[] bytes)
    {
        Stats result = new Stats();
        result.Damage = BitConverter.ToUInt16(bytes, 0);
        result.Health = BitConverter.ToUInt16(bytes, 2);
        return result;
    }
}
