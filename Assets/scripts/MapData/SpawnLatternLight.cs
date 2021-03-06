﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteAlways]
public class SpawnLatternLight : MonoBehaviour
{
    [SerializeField] private GameObject lightPrefab;
    [SerializeField] private Tilemap map;

    [SerializeField]
    private Transform lightHolder;
    [ContextMenu("Spawn Laterns")]
    void SpawnLight()
    {
        for (int n = map.cellBounds.xMin; n < map.cellBounds.xMax; n++)
        {
            for (int p = map.cellBounds.yMin; p < map.cellBounds.yMax; p++)
            {
                Vector3Int localPlace = (new Vector3Int(n, p, (int)map.transform.position.y));
                Vector3 place = map.CellToWorld(localPlace);
                if (map.HasTile(localPlace))
                {
                    if (map.GetTile(localPlace) is LaternTile)
                    {
                        LaternTile _tile = (LaternTile)map.GetTile(localPlace);
                        GameObject light = Instantiate(lightPrefab, new Vector3(place.x, place.y, -1.5f),Quaternion.identity, lightHolder);
                        light.GetComponent<Light>().intensity = _tile.intensity;
                        light.GetComponent<Light>().range = _tile.range;
                    }
                }
            }
        }
    }
}
