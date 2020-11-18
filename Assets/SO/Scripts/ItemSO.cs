using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable, CreateAssetMenu(menuName = "SO/Items")]
public class ItemSO : ScriptableObject
{
    public List<VisualizeItem> items;

    public Sprite GetSpriteById(int id)
    {
        return items.First(p => p.itemData.itemId == id || p.weaponData.itemId == id).itemSprite;
    }
}
[Serializable]
public class VisualizeItem
{
    public bool isWeapon = false;
    [HideIf("isWeapon")]
    public ItemData itemData;
    [ShowIf("isWeapon")]
    public WeaponData weaponData;
    [PreviewField(100)]
    public Sprite itemSprite;
}