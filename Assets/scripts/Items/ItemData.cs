using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemData
{
    public byte itemId;
    public byte itemEffectId;
}
[Serializable]
public class WeaponData : ItemData
{
    public byte damage;
    public byte attackSpeed;
}