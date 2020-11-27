using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable, CreateAssetMenu(menuName = "SO/Characters")]
public class CharacterSO : ScriptableObject
{
    [Serializable] public struct Character
    {
        [BoxGroup("Visual Settings")] public string name;
        [BoxGroup("Visual Settings")] public string description;
        [BoxGroup("Visual Settings")] public byte[] biographyId;

        [BoxGroup("Visual Settings")] [PreviewField(128, ObjectFieldAlignment.Left)]
        public Sprite characterIcon;

        [BoxGroup("Skills Settings")] public List<byte> skillList;

        [BoxGroup("Base Stat Settings")] public byte health;
        [BoxGroup("Base Stat Settings")] public byte maxHealth;
        [BoxGroup("Base Stat Settings")] public byte damage;
        [BoxGroup("Base Stat Settings")] public byte critChance;
        [BoxGroup("Base Stat Settings")] public byte hitChance;
    }

    public List<Character> characterList;

    public Character GetCharacterById(int id)
    {
        return characterList[id];
    }
}
