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
        public enum SpellTarget
        {
            Ally = 0,
            Enemy = 1,
            RandomAlly = 2,
            RandomEnemy = 3,
            MultiplyEnemy = 4,
            MultiplyAlly = 5
        }

        public enum SpellTypeEffect
        {
            Heal = 0,
            Damage = 1
        }

        public enum MultiplyTarget
        {
            NearestToTarget = 0,
            NearestToStart = 1,
            NearestToEnd = 2,
            All = 3
        }
        
        [BoxGroup("Visual Settings")] [PreviewField(50, ObjectFieldAlignment.Left)] 
        public Sprite SpellIcon;
        [BoxGroup("Visual Settings")] public string name;
        [BoxGroup("Visual Settings")] public string description;


        [BoxGroup("Status Settings")] public SpellTypeEffect spellTypeEffect;
        [BoxGroup("Status Settings")] public byte critChance;
        [BoxGroup("Status Settings")] public byte hitChance;
        [BoxGroup("Status Settings")] public byte strength;
        [BoxGroup("Status Settings")] public SpellTarget target;

        [BoxGroup("Status Settings")] [ShowIf("MultiplyAlly")]
        public byte targetCount;

        [BoxGroup("Status Settings")] [ShowIf("MultiplyAlly")]
        public MultiplyTarget multiplyTargetType;

        public bool MultiplyAlly() => target == SpellTarget.MultiplyAlly || target == SpellTarget.MultiplyEnemy ? true : false;
        
        [BoxGroup("Effect Upgrade Setting")] public byte increaseStrengthPerLvl;
        [BoxGroup("Effect Upgrade Setting")] public byte increaseHitChance0PerLvl;
        [BoxGroup("Effect Upgrade Setting")] public byte increaseCritChancePerLvl;
        
        [BoxGroup("Effect Settings")] public bool hasEffect;
        [BoxGroup("Effect Settings")] [ShowIf("hasEffect")] public byte chanceToApplyEffect;
        [BoxGroup("Effect Settings")] [ShowIf("hasEffect")] public List<EffectSO> effectIdList;
    }
    [SerializeField] public List<SpellType> SpellList;

    public SpellType GetSpellById(int id)
    {
        return SpellList[id];
    }
}
