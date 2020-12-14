using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHolder : MonoBehaviour
{
    public Monsters.Monster monster;

    [SerializeField] private Image icon;
    [SerializeField] private Image chooseIcon;
    [SerializeField] private Image hpBar;
    [SerializeField] private Transform effectHolder;

    public void UpdateSprite()
    {
        icon.sprite = monster.monsterIcon;
    }

    public void ShowChooseIcon()
    {
        chooseIcon.gameObject.SetActive(true);
    }

    public void HideChooseIcon()
    {
        if (chooseIcon.gameObject != null)
            chooseIcon.gameObject.SetActive(false);
    }
    public void OnDamageClick(Spells.SpellType spell, GameObject character)
    {
        RemoveListener();
        GetComponent<Button>().onClick.AddListener(()=>
        {
            StartCoroutine(ApplyDamage(spell, character));
        });
        
    }
    public void RemoveListener()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
    }
    private IEnumerator ApplyDamage(Spells.SpellType spell, GameObject character)
    {
        Vector3 pos = character.transform.position;
        yield return new DOTweenCYInstruction.WaitForCompletion(character.transform.DOMove(transform.position, 0.65f).OnComplete(()=>character.transform.DOMove(pos, 0.45f)));
        if (Random.Range(0, 100) <= spell.hitChance)
        {
            if (monster.Health - spell.strength <= 0)
                monster.Health = 0;
            else monster.Health -= spell.strength;
            UpdateHpBar();
            Manager._Instance.ShowDamage(spell.strength, transform, Color.black);
            if (spell.hasEffect)
            {
                if (Random.Range(0,100) <= spell.chanceToApplyEffect)
                    spell.effectIdList.ForEach(p =>
                    {
                        EffectSO _effect = Instantiate(p);
                        monster.StatusEffect.Add(_effect);
                        monster.OnApplyEffect.Invoke(monster,effectHolder);
                    });
            }
            if (spell.target == Spells.SpellType.SpellTarget.MultiplyEnemy)
            {
                int targetCount = spell.AdditionaltargetCount;
                List<MonsterHolder> Mobs = Manager._Instance.GetMonsterHolder();
                switch (spell.multiplyTargetType)
                {
                    case Spells.SpellType.MultiplyTarget.NearestToStart:
                        Mobs.Remove(this);
                        for (int i = 0; i < targetCount; i++)
                        {
                            Mobs.ElementAt(i).monster.Health -= spell.strength / 2;
                            Mobs.ElementAt(i).UpdateHpBar();
                            Manager._Instance.ShowDamage(spell.strength / 2, Mobs[i].transform, Color.black);
                        }

                        break;
                    case Spells.SpellType.MultiplyTarget.NearestToTarget:
                        int targetId = Mobs.FindIndex(p => p == this);
                        List<MonsterHolder> closestIds = Mobs.FindClosestElements(targetId);
                        if (closestIds.Count > targetCount)
                        {
                            closestIds.RemoveAt(Random.Range(0, closestIds.Count));
                        }
                        if (closestIds != null)
                        {
                            closestIds.ForEach(p =>
                            {
                                p.monster.Health -= spell.strength / 2;
                                p.UpdateHpBar();
                                Manager._Instance.ShowDamage(spell.strength / 2, p.transform, Color.black);
                                if (Random.Range(0, 100) <= spell.chanceToApplyEffect)
                                {
                                    p.monster.StatusEffect.AddRange(spell.effectIdList);
                                    p.monster.OnApplyEffect.Invoke(p.monster, p.effectHolder);
                                }
                            });
                        }
                        break;
                    case Spells.SpellType.MultiplyTarget.NearestToEnd :
                        Mobs.Remove(this);
                        for (int i = Mobs.Count; i < targetCount; i--)
                        {
                            Mobs.ElementAt(i).monster.Health -= spell.strength / 2;
                            Mobs.ElementAt(i).UpdateHpBar();
                            Manager._Instance.ShowDamage(spell.strength / 2, Mobs[i].transform, Color.black);
                        }
                        break;
                    case Spells.SpellType.MultiplyTarget.All:
                        Mobs.Remove(this);
                        Mobs.ForEach(p =>
                        {
                            p.monster.Health -= spell.strength / 2;
                            p.UpdateHpBar();
                            Manager._Instance.ShowDamage(spell.strength / 2, p.transform, Color.black);
                            if (Random.Range(0, 100) <= spell.chanceToApplyEffect)
                            {
                                p.monster.StatusEffect.AddRange(spell.effectIdList);
                                p.monster.OnApplyEffect.Invoke(p.monster, p.effectHolder);
                            }
                        });
                        break;
                }
            }
        }
        Manager._Instance.NextTurn();
    }

    public void UpdateHpBar()
    {
        hpBar.fillAmount = monster.Health / (float) monster.maxHp;
    }
}
