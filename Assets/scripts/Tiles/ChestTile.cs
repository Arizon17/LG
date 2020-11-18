using System.Collections;
using System.Collections.Generic;
using FamilyWikGame;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tile/ChestTile")]
public class ChestTile : InteractableTile
{
    public byte itemSetId;

    public byte[] GetItems()
    {
        return GameManager._instance.dungeonItemSet.getBytesInSet(itemSetId);
    }
}
