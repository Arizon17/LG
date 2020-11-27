using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


[Serializable, CreateAssetMenu(menuName = "SO/Spells")]
public class Spells : ScriptableObject
{
    [Serializable]
    public struct SpellType
    {
        [BoxGroup("Visual Settings")] [PreviewField(50, ObjectFieldAlignment.Left)] 
        public Sprite SpellIcon;
        [BoxGroup("Visual Settings")] public string name;
        [BoxGroup("Visual Settings")] public string description;
        
        [BoxGroup("Status Settings")] public byte critChance;
        [BoxGroup("Status Settings")] public byte hitChance;
        [BoxGroup("Status Settings")] public byte damage;

        [BoxGroup("Effect Settings")] public bool hasEffect;

        [BoxGroup("Effect Settings")] [ShowIf("hasEffect")] public byte chanceToApplyEffect;
        [BoxGroup("Effect Settings")] [ShowIf("hasEffect")] public List<EffectSO> effectIdList;
    }
    [SerializeField] public List<SpellType> SpellList;
}
