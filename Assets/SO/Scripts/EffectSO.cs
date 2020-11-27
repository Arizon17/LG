using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable, CreateAssetMenu(menuName = "SO/Effects")]
public class EffectSO : ScriptableObject
{
    [Serializable]
    public struct EffectType
    {
        [BoxGroup("Visual Settings")] public string name;
        [BoxGroup("Visual Settings")] public string description;

        [BoxGroup("Visual Settings")] [PreviewField(50, ObjectFieldAlignment.Left)]
        public Sprite effectIcon;

        [BoxGroup("Effect Settings")] public byte duration;
        [BoxGroup("Effect Settings")] public byte strenght;
    }
    public EffectType Effect;

}
