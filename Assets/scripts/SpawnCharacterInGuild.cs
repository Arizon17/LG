using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using Character = CharacterSO.Character;
using Spell = Spells.SpellType;
public class SpawnCharacterInGuild : MonoBehaviour
{
    [SerializeField] private CharacterSlotInGuild slotPrefab;
    
    [SerializeField] private int count;

    [SerializeField] private CharacterSO characterSo;
    [SerializeField] private BiographySO biographySo;
    [SerializeField] private CharacterBar characterStatBar;
    [SerializeField] private Spells spellSo;
    
    [ContextMenu("refresh characters")]
    public void Start()
    {
        List<Character> characters = InstantiateRandomCharacters();
        foreach (var character in characters)
        {
            var _t = Instantiate(slotPrefab, transform);
            _t.characterSprite.sprite = character.characterIcon;
            _t.characterText.text = character.name;
            _t.gameObject.AddComponent<Button>().onClick.AddListener(()=>
            {
                if (characterStatBar.name.text == character.name)
                {
                    characterStatBar.gameObject.SetActive(false);
                    characterStatBar.name.text = "";
                }
                else
                {
                    characterStatBar.gameObject.SetActive(true);
                    ShowCharacter(character);
                }
            });
        }
    }
    void ShowCharacter(Character character)
    {
        characterStatBar.ClearBio();
        characterStatBar.ClearSkills();
        characterStatBar.spellDescription.gameObject.SetActive(false);
        characterStatBar.description.text = character.description; 
        characterStatBar.name.text = character.name;
        characterStatBar.hpText.text = character.maxHealth.ToString();
        characterStatBar.damageText.text = character.damage.ToString();
        characterStatBar.critChanceText.text = character.critChance.ToString();
        characterStatBar.hitChanceText.text = character.hitChance.ToString();
        characterStatBar.characterIcon.sprite = character.characterIcon;
        foreach (int biog in character.biographyId)
        {
            var _t = Instantiate(characterStatBar.biographyPrefab, characterStatBar.biographyHolder);
            _t.text = biographySo.GetBiographyTypeById(biog).name;
        }
        foreach (byte id in character.skillList)
        {
            var _t = Instantiate(characterStatBar.skillIconPrefab, characterStatBar.skillHolder);
            _t.transform.GetChild(0).GetComponent<Image>().sprite = spellSo.GetSpellById(id).SpellIcon;
            _t.gameObject.AddComponent<Button>().onClick.AddListener(()=>SpellDescription(spellSo.GetSpellById(id)));
        }
    }

    void SpellDescription(Spell spell)
    {
        if (characterStatBar.spellDescription.name != spell.name)
        {
            characterStatBar.spellDescription.text =
                "Spell target : " + spell.target + " |Spell str : " + spell.strength +
                " |Spell crit chance : " + spell.critChance + " |Spell hit chance : " +
                spell.hitChance;
            characterStatBar.spellDescription.name = spell.name;
            characterStatBar.spellDescription.gameObject.SetActive(true);
        }
        else
        {
            characterStatBar.spellDescription.name = "";
            characterStatBar.spellDescription.gameObject.SetActive(false);
        }
    }
    private List<Character> InstantiateRandomCharacters()
    {
        List<int> blockedIds = new List<int>();
        List<Character> characters = new List<Character>();
        int randomId = Random.Range(0, characterSo.characterList.Count);
        for (int i = 0; i < count; i++)
        {
            while(blockedIds.Contains(randomId))
                randomId = Random.Range(0, characterSo.characterList.Count);
            Debug.Log(blockedIds.Contains(randomId) + " " + randomId);
            blockedIds.Add(randomId);
            characters.Add(characterSo.GetCharacterById(randomId));
        }
        return characters;
    }
}
