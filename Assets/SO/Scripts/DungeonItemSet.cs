using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable,  CreateAssetMenu(menuName = "SO/Dungeon Item Set")]
public class DungeonItemSet: ScriptableObject
{
    [Serializable]
    public struct ItemSet
    {
        public byte itemSetId;
        public List<byte>  itemId;
    }

    [SerializeField] private List<ItemSet> dungeonItemSets;

    public byte[] getBytesInSet(int id)
    {
        return dungeonItemSets.First(p => p.itemSetId == id).itemId.ToArray();
    }
}
