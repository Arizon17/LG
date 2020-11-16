﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Stats
{
    public ushort MaxHealth;
    public ushort Health;
    public ushort Damage;

    public Stats()
    {
        MaxHealth = Health = 100;
        Damage = 1;
    }
}
