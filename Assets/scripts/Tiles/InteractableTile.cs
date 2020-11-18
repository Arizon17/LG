using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tile/InteractableTile")]
public class InteractableTile : Tile
{
    public bool needKeyToOpen;
    [ShowIf("needKeyToOpen")] 
    public bool basicKey;
}
