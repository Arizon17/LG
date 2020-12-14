using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[Serializable, CreateAssetMenu(menuName = "SO/Characters")]
public class CharacterSO : ScriptableObject
{
    [Serializable] public class Character : ICreature
    {
        [BoxGroup("Visual Settings")] public string name;
        [BoxGroup("Visual Settings")] public string description;
        [BoxGroup("Visual Settings")] public byte[] biographyId;

        [BoxGroup("Visual Settings")] [PreviewField(128, ObjectFieldAlignment.Left)]
        public Sprite characterIcon;

        [BoxGroup("Skills Settings")] public List<byte> skillList;
        
        [BoxGroup("Base Stat Settings")] public int health;
        [BoxGroup("Base Stat Settings")] public byte speed;
        [BoxGroup("Base Stat Settings")] public int maxHealth;
        [BoxGroup("Base Stat Settings")] public byte damage;
        [BoxGroup("Base Stat Settings")] public byte critChance;
        [BoxGroup("Base Stat Settings")] public byte hitChance;
        public byte Speed { get => speed; set => speed = value; }
        
        [ShowInInspector]
        public List<EffectSO> StatusEffect { get; set; }
        [BoxGroup("Base Stat Settings")]
        public int Health { get => health; set => health = value;}

        public int MaxHealth { get=>maxHealth; set => maxHealth = value; }

        public UnityAction OnTurnStart { get; set; }
        public EffectEvent OnApplyEffect { get; set; }
        public GameObject GameObject { get; set; }

        public Character newCharacter()
        {
            return this;
        }
    }

    public List<Character> characterList;

    public Character GetCharacterById(int id)
    {
        return characterList[id];
    }
}
