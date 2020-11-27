using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable, CreateAssetMenu(menuName = "SO/Biography")]
public class BiographySO : ScriptableObject
{
    [Serializable]
    public struct BiographyType
    {
        public string name;
        public string description;
    }

    public List<BiographyType> biographyTypes;

    public BiographyType GetBiographyTypeById(int id)
    {
        return biographyTypes[id];
    }
}
