using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteAlways]
public class VaildateKeysCount : MonoBehaviour
{
    [SerializeField] private Tilemap map;
    [ContextMenu("Validate Keys Count")]
    void ValidateKeys()
    {
        int needAdvancedKey = 0, needBasicKeys = 0;
        int advancedKeysCount = 0, basicKeysCount = 0;
        for (int n = map.cellBounds.xMin; n < map.cellBounds.xMax; n++)
        {
            for (int p = map.cellBounds.yMin; p < map.cellBounds.yMax; p++)
            {
                Vector3Int localPlace = (new Vector3Int(n, p, (int)map.transform.position.y));
                Vector3 place = map.CellToWorld(localPlace);
                if (map.HasTile(localPlace))
                {
                    if (map.GetTile(localPlace) is InteractableTile)
                    {
                        InteractableTile _tile = (InteractableTile) map.GetTile(localPlace);
                        if (_tile.needKeyToOpen)
                            if (_tile.basicKey)
                                needBasicKeys++;
                            else
                                needAdvancedKey++;
                    }

                    if (map.GetTile(localPlace) is PickUpableTile)
                    {
                        PickUpableTile _tile = (PickUpableTile) map.GetTile(localPlace);
                        if (_tile.key)
                            if (_tile.basicKeyToGive)
                                basicKeysCount++;
                            else
                                advancedKeysCount++;

                    }
                }
            }
        }
        Debug.Log("Need advanced keys : " + needAdvancedKey + " | " + " founded advanced key on map : " +advancedKeysCount);
        Debug.Log("Need basic keys : " + needBasicKeys + " | " + " founded basic keys on map : " +basicKeysCount);
    }
}
