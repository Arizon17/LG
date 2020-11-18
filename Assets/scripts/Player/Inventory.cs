using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using ExitGames.Client.Photon;

public class Inventory : MonoBehaviour, IPunObservable
{
    [SerializeField]
    private InventoryData inventoryData = new InventoryData();

    public void AddItemToInventory(byte id)
    {
        inventoryData.AddToInventoryById(id);
    }

    public void AddItemRangeToInventory(byte[] ids)
    {
        inventoryData.AddToInventoryByRangeId(ids);
    }

    public void AddCoint(ushort amount)
    {
        inventoryData.AddCoin(amount);
    }

    public int GetInventoryLenght()
    {
        return inventoryData.itemIDs.Count;
    }

    public List<byte> GetInventoryItems()
    {
        return inventoryData.itemIDs;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(inventoryData);
        }

        if (stream.IsReading)
        {
            inventoryData = (InventoryData) stream.ReceiveNext();
        }
    }

    public byte GetBasicKeysCount()
    {
        return inventoryData.basicKeyCount;
    }

    public void RemoveBasicKeyFromInventory()
    {
        if (inventoryData.basicKeyCount > 0)
        inventoryData.basicKeyCount--;
    }

    public byte GetAdvancedKeyCount()
    {
        return inventoryData.advancedKeyCount;
    }

    public void RemoveAdvancedKeyFromInventory()
    {
        if (inventoryData.advancedKeyCount > 0)
        inventoryData.advancedKeyCount--;
    }

    public void AddBasicKeyToInventory()
    {
        if(inventoryData.basicKeyCount < byte.MaxValue)
            inventoryData.basicKeyCount++;
    }

    public void AddAdvancedKeyToInventory()
    {
        if (inventoryData.advancedKeyCount < byte.MaxValue)
            inventoryData.advancedKeyCount++;
    }
}
[Serializable]
public class InventoryData
{
    public List<byte> itemIDs;
    public ushort coins;
    public byte basicKeyCount;
    public byte advancedKeyCount;
    public void AddToInventoryById(byte id)
    {
        itemIDs.Add(id);
    }

    public void AddToInventoryByRangeId(byte[] ids)
    {
        itemIDs.AddRange(ids);
    }

    public void AddCoin(ushort amount)
    {
        if (coins + amount <= Int16.MaxValue)
        {
            coins += amount;
        }
    }

    public ushort GetCoins()
    {
        return coins;
    }

    public InventoryData()
    {
        itemIDs = new List<byte>();
        coins = new ushort();
    }

    public InventoryData(int lenght)
    {
        itemIDs = new List<byte>(lenght);
        coins = new ushort();
    }
}
