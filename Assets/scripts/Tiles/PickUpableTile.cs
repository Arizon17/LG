using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Tile/PickUpItemTile")]
public class PickUpableTile : InteractableTile
{
    public bool key;
    [HideIf("key")]
    public byte itemId;

    [ShowIf("key")]
    public bool basicKeyToGive;
}
