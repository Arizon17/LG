using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroHolder : MonoBehaviour
{
    [SerializeField] private Image hpField;
    [SerializeField] private Image chooseIcon;
    [SerializeField] private Image choosenFrame;
    [SerializeField] private Image target;
    [SerializeField] private Transform effectPanel;
    public CharacterSO.Character character;
    public byte aggroCount;

    public Transform GetEffectPanel()
    {
        return effectPanel;
    }
    public void UpdateHpField()
    {
        float _currentTemp = character.Health;
        float _maxTemp = character.maxHealth;
        hpField.fillAmount = _currentTemp / _maxTemp;
    }

    public void ShowTarget()
    {
        target.gameObject.SetActive(true);
    }

    public void HideTarget()
    {
        target.gameObject.SetActive(false);
    }
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(()=>OnClick());
        HideChooseFrame();
    }
    public void UpdateHeroSprite()
    {
        GetComponent<Image>().sprite = character.characterIcon;
    }

    private void OnClick()
    {
        if (Manager._Instance)
        {
            ShowSkill();
        }
    }

    public void HideChooseFrame()
    {
        chooseIcon.gameObject.SetActive(false);
    }

    public void ShowChoosenFrame()
    {
        choosenFrame.gameObject.SetActive(true);
    }

    public void HideFrame()
    {
        choosenFrame.gameObject.SetActive(false);
    }

    public void RemoveListener()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
    }

    public void EndTurn()
    {
        GetComponent<Image>().color = Color.white;
        HideFrame();
        HideChooseFrame();
    }
    public void AddHealListener(Spells.SpellType spell)
    {
        RemoveListener();
        GetComponent<Button>().onClick.AddListener(()=>
            {
                if (Random.Range(0, 100) <= spell.hitChance)
                {
                    ApplyHeal(spell.strength);
                    if (spell.hasEffect)
                    {
                        if (Random.Range(0, 100) <= spell.chanceToApplyEffect)
                            for (int i = 0; i < spell.effectIdList.Count; i++)
                            {
                                EffectSO effect = Instantiate(spell.effectIdList[i]);
                                character.StatusEffect.Add(effect);
                                character.OnApplyEffect.Invoke(character, GetEffectPanel());
                            }
                    }
                }
                else Debug.Log("Miss Heal on target : " + character.name);
                Manager._Instance.NextTurn();
            });
    }

    public void AddSkillChooseListener()
    {
        RemoveListener();
        GetComponent<Button>().onClick.AddListener(()=>OnClick());
    }
    public void ApplyHeal(int heal)
    {
        if (character.Health + heal >= character.maxHealth)
            character.Health = character.maxHealth;
        else 
            character.Health += (byte)heal;
        
        Manager._Instance.ShowDamage(heal, transform, Color.green);
        UpdateHpField();
    }
    public void ShowSkill()
    {
        Manager._Instance.DestroyOldSkill();
        chooseIcon.gameObject.SetActive(true);
        for (int i = 0; i < 4; i++)
        {
            Spells.SpellType spell = Manager._Instance.spellSo.GetSpellById(character.skillList[i]);
            Manager._Instance.InstantiateSkill(spell, gameObject);
        }
    }
}
