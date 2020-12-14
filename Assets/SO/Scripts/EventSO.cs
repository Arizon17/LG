using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable, CreateAssetMenu(menuName = "SO/Events")]
public class EventSO : ScriptableObject
{
    [Serializable]
    public struct Events
    {
        public enum EventType
        {
            MoneyGive = 0,
            AddItem,
            ApplyEffectOnTarget,
            StartFight,
        }
        public EventType eventType;
        private bool ShowSlot => eventType == EventType.ApplyEffectOnTarget ? true : false;
        [ShowIf("ShowSlot")]
        public int[] targetSlot;

        [ShowIf("ShowSlot")] public EffectSO effect;

        private bool ShowMobs => eventType == EventType.StartFight ? true : false;
        [ShowIf("ShowMobs")][MinMax(0,4)]
        public int[] monsterIds;

        private bool ShowMoneyCount => eventType == EventType.MoneyGive ? true : false;
        [ShowIf("ShowMoneyCount")] public int moneyCount;

        private bool ShowItemId => eventType == EventType.AddItem ? true : false;
        [ShowIf("ShowItemId")]
        public bool SingleItem;
        [ShowIf("SingleItem")] public ItemSO item;
        [HideIf("SingleItem")][ShowIf("ShowItemId")] public ItemSO[] items;
    }
    public List<Events> events;

    public Events GetEventById(int id)
    {
        return events[id];
    }
}
