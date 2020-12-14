using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[Serializable, CreateAssetMenu(menuName = "SO/Monsters")]
public class Monsters : ScriptableObject
{
    [Serializable]
    public class Monster : ICreature
    {
        public byte prioritySlot;
        public int hp;
        public int maxHp;
        public byte damage;
        public byte critChance;
        public byte hitChance;
        public List<byte> skillList;
        [PreviewField(150, ObjectFieldAlignment.Left)] public Sprite monsterIcon;
        public string name;
        private ICreature _creatureImplementation;

        [ShowInInspector]
        public byte Speed { get; set; }
        [ShowInInspector]

        public List<EffectSO> StatusEffect { get; set; }
        public int Health { get => hp; set => hp = value; }

        public int MaxHealth { get => maxHp; set => maxHp = value; }

        public UnityAction OnTurnStart { get; set; }
        public EffectEvent OnApplyEffect { get; set; }
        
        public GameObject GameObject { get; set; }

        public Monster(Monster reference)
        {
            this.MaxHealth = reference.MaxHealth;
            this.damage = reference.damage;
            this.Health = reference.Health;
            this.name = reference.name;
            this.monsterIcon = reference.monsterIcon;
            this.prioritySlot = reference.prioritySlot;
            this.skillList = reference.skillList;
            this.hitChance = reference.hitChance;
            this.critChance = reference.critChance;
            this.Speed = reference.Speed;
        }

        public Monster()
        {
            //Empty monster
        }
    }
    public List<Monster> monsterList;

    public Monster GetMonsterById(int id)
    {
        Monster monster = new Monster(monsterList[id]);
        return monster;
    }
}
