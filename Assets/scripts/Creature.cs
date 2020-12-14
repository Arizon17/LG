using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public interface ICreature
{
    [ShowInInspector]
    byte Speed { get; set; }
    
    List<EffectSO> StatusEffect { get; set; }
    int Health { get; set; }
    int MaxHealth { get; set; }

    UnityAction OnTurnStart { get; set; }
    EffectEvent OnApplyEffect { get; set; }
    [HideInInspector]
    GameObject GameObject { get; set; }
}

[System.Serializable]
public class EffectEvent : UnityEvent<ICreature, Transform>
{
    
}