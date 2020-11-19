using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FamilyWikGame;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "Tile/ChestTile")]
public class ChestTile : InteractableTile
{
    public bool itemSetWithChanceBool;
    [Serializable]
    public struct itemSetWithChance
    {
        public byte itemSetId;
        public int chance;

    }
    [HideIf("itemSetWithChanceBool")]
    public byte itemSetId;

    [ShowIf("itemSetWithChanceBool")] public List<itemSetWithChance> itemSetList;
    public byte[] GetItems()
    {
        return GameManager._instance.dungeonItemSet.getBytesInSet(itemSetId);
    }
    public byte[] GetItems(byte id)
    {
        return GameManager._instance.dungeonItemSet.getBytesInSet(id);
    }

    public byte[] GetItemsWithChange()
    {
        int _RandomValue = Random.Range(0, 100);
        byte[] match = new byte[0];
        List<int> Chances = new List<int>();
        foreach (itemSetWithChance set in itemSetList)
            Chances.Add(set.chance);
        int[] point = new int[Chances.Count + 1];
        for (int i = 0; i < Chances.Count; i++)
        {
            point[i + 1] = point[i] + Chances[i];
        }

        for (int i = 0; i < Chances.Count; i++)
        {
            if (_RandomValue >= point[i] && _RandomValue <= point[i + 1])
            {
                match = itemSetList.Where(p => p.chance == Chances[i]).Select(p=>p.itemSetId).ToArray();
            }
        }

        if (match.Length > 1)
            return GetItems((byte) Random.Range(0, match.Length));
        else
            return GetItems(match[0]);
    }
}
