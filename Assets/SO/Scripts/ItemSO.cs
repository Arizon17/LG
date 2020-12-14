using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable, CreateAssetMenu(menuName = "SO/Items")]
public class ItemSO : ScriptableObject
{
    public VisualizeItem item;

    public int GetItemId()
    {
        return item.itemData.itemId;
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

    public string itemName;
    public string itemDescription;
}