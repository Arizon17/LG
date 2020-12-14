using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using DG.Tweening;
using FamilyWikGame;
using TMPro;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [SerializeField] private HeroHolder heroPrefab;
    [SerializeField] private Transform heroPanel;
    [SerializeField] private SkillIcon skillIconPrefab;
    [SerializeField] private Transform skillPanel;
    [SerializeField] private MonsterHolder monsterPrefab;
    [SerializeField] private Transform monsterPanelHolder;
    private List<HeroHolder> heroList = new List<HeroHolder>();
    private List<MonsterHolder> monsterList = new List<MonsterHolder>();
    
    [SerializeField] private List<DungeonRoom> dungeonRoom;


    [SerializeField] private Transform currentRoomPanel;
    [SerializeField] private DungeonRoomHolder currentRoomObject;
    [SerializeField] private DungeonRoom currentRoom;
    [SerializeField] private DungeonRoomHolder dungeonRoomHolder;
    [SerializeField] private Transform dungeonRoomPanel;
    [SerializeField] private GameObject DungeonRoomMain;

    [SerializeField] private EffectHolder effectHolder;


    [SerializeField] private TextMeshProUGUI damagePrefab;

    [SerializeField] private SkillTargetPanel skillTargetPanel;
    
    [ShowInInspector]
    private List<ICreature> turnedList = new List<ICreature>();
    [ShowInInspector]
    private ICreature _currentCreatureTurn;
    public Monsters monsterSo;
    public CharacterSO characterSo;
    public Spells spellSo;
    public EventSO eventSo;
    public static Manager _Instance;
    public Data data = new Data();
    public void InitGame(int characterCount = 4)
    {
        for (int i = 0; i < characterCount; i++)
        {
            if (PlayerPrefs.GetInt($"hero_slot_{i}") >= 0)
            {
                HeroHolder hero = Instantiate(heroPrefab, heroPanel);
                heroList.Add(hero);
                hero.character = characterSo.GetCharacterById(PlayerPrefs.GetInt($"hero_slot_{i}"));
                hero.character.GameObject = hero.gameObject;
                hero.character.Health = hero.character.maxHealth;
                hero.character.StatusEffect = new List<EffectSO>();
                hero.character.OnApplyEffect = new EffectEvent();
                hero.character.OnApplyEffect.AddListener(OnApplyEffect);
                hero.name = hero.character.name;
                hero.UpdateHeroSprite();
            }
        }
        InitRoom();
    }

    public void InitRoom()
    {
        currentRoom = new DungeonRoom("First room");
        currentRoomObject = Instantiate(dungeonRoomHolder,currentRoomPanel);
        currentRoomObject.room = currentRoom;
        currentRoomObject.HideImage();
        InitNeighborRooms();
    }

    public void InitNeighborRooms()
    {
        DungeonRoomMain.gameObject.SetActive(true);
        int _k = 0;
        currentRoom.SpawnNeighbor(currentRoom.roomName, currentRoom);
        currentRoom.connectedRooms.ForEach(p =>
        {
            Vector3 pos = new Vector3();
            switch (_k)
            {
                case 0: pos = new Vector3(-350, -450, 0);break;
                case 1: pos = new Vector3(-350, 0, 0); break;
                case 2: pos = new Vector3(-350, 450, 0); break;
            }
            var _dungR = Instantiate(dungeonRoomHolder, dungeonRoomPanel);
            _dungR.room = p;
            _dungR.gameObject.AddComponent<Button>().onClick.AddListener(() =>
            {
                MoveToRoom(p);
            });
            _dungR.Init(pos);
            _k++;
        });
    }
    [ContextMenu("Move Top")]
    public void MoveTop()
    {
        currentRoom = currentRoom.connectedRooms[0];
    }

    void OnApplyEffect(ICreature target, Transform parentHolder)
    {
        var effectObj = Instantiate(effectHolder, parentHolder);
        effectObj.Init(target.StatusEffect.Last());
        Debug.Log(target.GameObject.name + " " + effectObj.effect);
    }
    void MoveToRoom(DungeonRoom room)
    {
        heroList.ForEach(p =>
        {
            DoEffect(p.character);
            p.UpdateHpField();
        });
        dungeonRoom.Add(currentRoom);
        foreach (RectTransform roomObj in dungeonRoomPanel)
        {
            Destroy(roomObj.gameObject);
        }
        currentRoom = room;
        currentRoomObject.room = currentRoom;
        switch (currentRoom.roomType)
        {
            case  DungeonRoom.RoomType.fightRoom:
                InitFight();
                return;
            case DungeonRoom.RoomType.eventRoom :
                RandomEvent();
                return;
        }
        InitNeighborRooms();
    }

    private ICreature DoEffect(ICreature target)
    {
       return target.DoEffect();
    }
    
    public void Start()
    {
        PlayerPrefs.SetInt("hero_slot_0", 0);
        InitGame();
        data.Init();
        data.SaveLoot();
    }

    void RandomEvent()
    {
        int rNumber = Random.Range(0, eventSo.events.Count);
        EventSO.Events _event = eventSo.GetEventById(rNumber);
        Debug.Log(_event.eventType);
        switch (_event.eventType)
        {
            case EventSO.Events.EventType.AddItem : 
                data.AddItem(_event.item.GetItemId());
                break;
            case EventSO.Events.EventType.MoneyGive : 
                data.AddMoney(_event.moneyCount);
                break;
            case EventSO.Events.EventType.StartFight :
                if (_event.monsterIds.Length > 0)
                    InitFight(_event.monsterIds);
                else 
                    InitFight();
                return;
            case EventSO.Events.EventType.ApplyEffectOnTarget :
                HeroHolder target = null;
                if (_event.targetSlot.Length > 0)
                {
                    for (int i = 0; i < _event.targetSlot.Length; i++)
                    {
                        target = heroList[_event.targetSlot[i]];
                    }
                }
                else target = heroList[Random.Range(0, heroList.Count)];
                EffectSO effect = Instantiate(_event.effect);
                target.character.StatusEffect.Add(effect);
                target.character.OnApplyEffect.Invoke(target.character, target.GetEffectPanel());
                break;
        }
        InitNeighborRooms();
    }
    void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
        }
        else Destroy(_Instance);
        DontDestroyOnLoad(_Instance);
    }

    public void DestroyOldSkill()
    {
        if (skillPanel.childCount > 0)
        foreach (Transform oldSkill in skillPanel)
        {
            Destroy(oldSkill.gameObject);
        }

        foreach (var hero in heroList)
        {
            hero.HideChooseFrame();
        }
    }
    public void InstantiateSkill(Spells.SpellType skill, GameObject character)
    {
        var _inst = Instantiate(skillIconPrefab, skillPanel);
        _inst.SetIcon(skill.SpellIcon);
        if (_currentCreatureTurn.GameObject == character)
        {
            _inst.gameObject.AddComponent<Button>().onClick.AddListener(()=>
            {
                HideChooseSkill();
                DoSkill(skill);
                _inst.ChooseIcon();
            });
        }
    }

    void HideChooseSkill()
    {
        foreach (Transform icons in skillPanel)
        {
            icons.GetComponent<SkillIcon>().HideChooseIcon();
        }
    }

    void DoSkill(Spells.SpellType skill)
    {
        monsterList.ForEach(p => p.HideChooseIcon());
        if (skill.target == Spells.SpellType.SpellTarget.Enemy || 
            skill.target == Spells.SpellType.SpellTarget.MultiplyEnemy)
        {
            monsterList.ForEach(p => p.ShowChooseIcon());
            if (skill.spellTypeEffect == Spells.SpellType.SpellTypeEffect.Damage)
            {
                monsterList.ForEach(p => p.OnDamageClick(skill, _currentCreatureTurn.GameObject));
            }
        }

        if (skill.target == Spells.SpellType.SpellTarget.Ally ||
            skill.target == Spells.SpellType.SpellTarget.MultiplyAlly)
        {
            heroList.ForEach(p => p.ShowChoosenFrame());
            if (skill.spellTypeEffect == Spells.SpellType.SpellTypeEffect.Heal)
            {
                heroList.ForEach(p => p.AddHealListener(skill));
            }
        }
    }

    public List<MonsterHolder> GetMonsterHolder()
    {
        return monsterList;
    }
    public void ShowDamage(int damage, Transform transform, Color textColor)
    {
        var _t = Instantiate(damagePrefab, FindObjectOfType<Canvas>().transform);
        _t.text = damage.ToString();
        _t.color = textColor;
        _t.transform.position = transform.position;
        _t.transform.DOMove(_t.transform.position + new Vector3(-15, 25, 0), 1f).OnComplete(()=>Destroy(_t.gameObject));
    }
    public void UpdateCharacter()
    {
        heroList.ForEach(p => p.UpdateHpField());
    }
    [ContextMenu("Spawn Mobs")]
    public void SpawnMonsters(int count = 4, int[] ids = null)
    {
        for (int i = 0; i < count; i++)
        {
            Monsters.Monster monster = new Monsters.Monster();
            if (ids == null)
            {
                int randomMonsterId = Random.Range(0, monsterSo.monsterList.Count);
                monster = monsterSo.GetMonsterById(randomMonsterId);
            }
            else
            {
                if (ids[i] != null)
                {
                    monster = monsterSo.GetMonsterById(ids[i]);
                }
                else return;
            }
            var _inst = Instantiate(monsterPrefab, monsterPanelHolder);
            _inst.monster = monster;
            _inst.monster.Health = _inst.monster.MaxHealth;
            _inst.UpdateSprite();
            _inst.monster.GameObject = _inst.gameObject;
            _inst.gameObject.name = i.ToString();
            _inst.monster.StatusEffect = new List<EffectSO>();
            _inst.monster.OnApplyEffect = new EffectEvent();
            _inst.monster.OnApplyEffect.AddListener(OnApplyEffect);
            monsterList.Add(_inst);
        }
    }
    [ContextMenu("Fight")]
    void InitFight()
    {
        DungeonRoomMain.gameObject.SetActive(false);
        SpawnMonsters();
        InitTurnList();
        StartTurn();
    }

    void InitFight(int[] id)
    {
        DungeonRoomMain.gameObject.SetActive(false);
        SpawnMonsters(id.Length, id);
        InitTurnList();
        StartTurn();
    }
    [ContextMenu("Start Turn")]
    void StartTurn()
    {
        this.StopAllCoroutines();
        _currentCreatureTurn.OnTurnStart?.Invoke();
    }

    void InitTurnList()
    {
        List<ICreature> temp = new List<ICreature>();
        monsterList.ForEach(p =>
        {
            Debug.Log(p.monster.Health);
            if (p.monster.Health > 0) 
                temp.Add(p.monster);
        });
        heroList.ForEach(p =>
        {
            if(p.character.Health > 0)
                temp.Add(p.character);
        });
        turnedList = temp.OrderByDescending(p => p.Speed).ToList();
        _currentCreatureTurn = turnedList.First();
        turnedList.ForEach(p => p.OnTurnStart = OnTurnStart);
        CheckForMonsters();
    }

    void CheckForMonsters()
    {
        foreach (var _temp in turnedList)
            if (_temp is Monsters.Monster)
                return;
        DespawnMonsters();
        InitNeighborRooms();
    }

    void DespawnMonsters()
    {
        foreach (Transform monster in monsterPanelHolder)
        {
            Destroy(monster.gameObject);
        }
    }
    public void NextTurn()
    {
        if (monsterList.All(p => p.monster.Health <= 0))
        {
            Debug.Log("aaaaaaa");
            DespawnMonsters();
            InitNeighborRooms();
            ResetThings();
            turnedList = null;
            return;
        }
        if (turnedList.Count > 1)
        {
            turnedList.RemoveAt(0);
            _currentCreatureTurn = turnedList[0];
        }
        else
        {
            InitTurnList();
        }
        StartTurn();
    }
    void OnTurnStart()
    {
        ResetThings();
        StartCoroutine(Turn());
    }

    public void UpdateMobs()
    {
        monsterList.ForEach(p => p.UpdateHpBar());
    }
    public void ResetThings()
    {
        monsterList.ForEach(p =>
        {
            if (p)
            {
                p.HideChooseIcon();
                p.RemoveListener();
            }
        });
        heroList.ForEach(p =>
        {
            if (p)
            {
                p.RemoveListener();
                p.AddSkillChooseListener();
                p.EndTurn();
                p.HideTarget();
            }
        });
        HideChooseSkill();
    }
    public IEnumerator Turn()
    {
        Debug.Log("Turn");
        if (_currentCreatureTurn.StatusEffect != null)
        {
            if (_currentCreatureTurn.StatusEffect.Count > 0)
            {
                _currentCreatureTurn = DoEffect(_currentCreatureTurn);
                UpdateCharacter();
                UpdateMobs();
            }
        }

        if (_currentCreatureTurn.Health == 0)
        {
            NextTurn();
            yield break;
        }
        if (_currentCreatureTurn is Monsters.Monster)
        {
            var _temp = (Monsters.Monster) _currentCreatureTurn;
            if (_temp.skillList.Count > 0)
            {
                Spells.SpellType spell = spellSo.GetSpellById(_temp.skillList[Random.Range(0, _temp.skillList.Count)]);
                if (spell.target == Spells.SpellType.SpellTarget.Enemy)
                {
                    HeroHolder target = null;
                    if (spell.spellTypeEffect == Spells.SpellType.SpellTypeEffect.Damage)
                    {
                        if (Random.Range(0, 100) <= spell.hitChance)
                        {
                            if (Random.Range(0, 100) <= 85)
                            {
                                if (heroList[_temp.prioritySlot].character.Health > 0)
                                {
                                    target = heroList[_temp.prioritySlot];
                                }
                                else target = heroList[Random.Range(0, heroList.Count)];
                            }
                            else target = heroList.OrderByDescending(p => p.aggroCount).First(p=>p.character.Health > 0);
                        }
                        if (target != null)
                        {
                            target.ShowTarget();
                            yield return new WaitForSeconds(0.5f);
                            skillTargetPanel.Init(target.character.characterIcon, _temp.monsterIcon);
                            yield return new DOTweenCYInstruction.WaitForCompletion(skillTargetPanel.AnimationSkill());
                            target.character.Health -= (byte) Random.Range(spell.strength / 2, spell.strength);
                            if (spell.hasEffect)
                            {
                                if (Random.Range(0,100) <= spell.chanceToApplyEffect)
                                spell.effectIdList.ForEach(p =>
                                {
                                    EffectSO _effect = Instantiate(p);
                                    target.character.StatusEffect.Add(_effect);
                                    target.character.OnApplyEffect.Invoke(target.character,target.GetEffectPanel());
                                });
                            }
                            skillTargetPanel.gameObject.SetActive(false);
                        }
                    }
                }
                else
                {
                    MonsterHolder target = null;
                    if (Random.Range(0, 100) < spell.hitChance)
                    {
                        target = monsterList[Random.Range(0, monsterList.Count)];
                    }

                    if (target != null)
                    {
                        if (target.monster.Health + spell.strength <= target.monster.Health)
                            target.monster.Health += spell.strength;
                    }
                }
            }
            else
            {
                HeroHolder target = null;
                byte damage = (byte)Random.Range(_temp.damage / 2, _temp.damage);
                if (heroList[_temp.prioritySlot].character.Health > 0 && Random.Range(0,100) < 65)
                {
                        target = heroList[_temp.prioritySlot];
                }
                else
                {
                        if (Random.Range(0, 2) == 0)
                        {
                            int randomTarget = Random.Range(0, heroList.Count);
                            while (heroList[randomTarget].character.Health == 0 || randomTarget == _temp.prioritySlot)
                                randomTarget = Random.Range(0, heroList.Count);
                            target = heroList[randomTarget];
                        }
                        else target = heroList.OrderByDescending(p => p.aggroCount).First(p=>p.character.Health > 0);
                }
                if (target)
                {
                    Vector3 pos = _temp.GameObject.transform.position;
                    target.ShowTarget();
                    //Move back for charge
                    yield return new DOTweenCYInstruction.WaitForCompletion(
                        _temp.GameObject.transform.DOLocalMove(
                            new Vector3(_temp.GameObject.transform.localPosition.x + 45f,
                                _temp.GameObject.transform.localPosition.y + 10f, 0), 0.15f));
                    //charge to target
                    yield return new DOTweenCYInstruction.WaitForCompletion(
                        _temp.GameObject.transform.DOMove(target.gameObject.transform.position, 0.75f));
                    if (Random.Range(0, 100) < _temp.hitChance)
                    {
                        if (target.character.Health - damage <= 0)
                            target.character.Health = 0;
                        else target.character.Health -= damage;

                        //Shape target
                        yield return new DOTweenCYInstruction.WaitForCompletion(
                            target.gameObject.transform.DOShakeScale(0.5f, 1.2f));
                        ShowDamage(damage,target.transform, Color.black);
                    }
                    //Move back to position
                    yield return new DOTweenCYInstruction.WaitForCompletion(
                        _temp.GameObject.transform.DOMove(pos, 0.35f));
                }
            }
            heroList.ForEach(p => p.UpdateHpField());
            NextTurn();
        }
        else
        {
            CharacterSO.Character character = (CharacterSO.Character) _currentCreatureTurn;
            HeroHolder heroHolder = character.GameObject.GetComponent<HeroHolder>();
            heroHolder.ShowSkill();
            heroHolder.GetComponent<Image>().color = Color.green;
        }
    }
}
