using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable, CreateAssetMenu(menuName = "SO/Effects")]
public class EffectSO : ScriptableObject
{
    [Serializable]
    public class EffectType
    {
        [BoxGroup("Visual Settings")] public string name;
        [BoxGroup("Visual Settings")] public string description;

        [BoxGroup("Visual Settings")] [PreviewField(50, ObjectFieldAlignment.Left)]
        public Sprite effectIcon;

        [BoxGroup("Effect Settings")] public byte duration;
        [BoxGroup("Effect Settings")] public byte strenght;
        
        public enum EffectTypeDeal
        {
            dealDamage = 0,
            heal
        }

        public EffectTypeDeal effectType;
        public GameObject effectHolder;
    }

    public EffectType Effect;
}
public static class EffectLogic
{
    public static ICreature DoEffect(this ICreature creature)
    {
        ICreature _temp = creature;
        _temp.StatusEffect.ForEach(effect =>
        {
            Debug.Log("Do Effect : " + effect + "On Creature : " +creature.GameObject.name);
            if (effect.Effect.duration > 0)
            {
                effect.Effect.duration--;
                switch (effect.Effect.effectType)
                {
                    case EffectSO.EffectType.EffectTypeDeal.dealDamage:
                    {
                        if (creature.Health - effect.Effect.strenght >= 0)
                        {
                            creature.Health -= effect.Effect.strenght;

                            Manager._Instance.ShowDamage(effect.Effect.strenght, creature.GameObject.transform,
                                Color.red);
                        }
                    }
                        break;
                    case EffectSO.EffectType.EffectTypeDeal.heal:
                    {
                        if (creature.Health + effect.Effect.strenght <= creature.MaxHealth)
                            creature.Health += effect.Effect.strenght;
                        Manager._Instance.ShowDamage(effect.Effect.strenght, creature.GameObject.transform,
                            Color.green);
                    }
                        break;
                }
            }
            else creature.StatusEffect.Remove(effect);
        });
        return _temp;
    }

    public static void Test(this ICreature t, string k)
    {
        Debug.Log(t + " " + k);
    }
}