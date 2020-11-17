using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarPanel : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Image manaBar;
    [SerializeField] private Text nickName;
    private PlayerStats player;

    public void UpdateHealthBar(float health, float maxHealth)
    {
        healthBar.fillAmount = health / maxHealth;
    }

    public void UpdateManaBar(float mana, float maxMana)
    {
        manaBar.fillAmount = mana / maxMana;
    }

    public void SetNickName(string nickNameText)
    {
        nickName.text = nickNameText;
    }
    public void SetPlayer(PlayerStats player)
    {
        this.player = player;
    }

    public PlayerStats GetPlayer()
    {
        return player;
    }
    void Update()
    {
        if (player)
        {
            UpdateHealthBar(player.GetHealth(),player.GetMaxHealth());
            UpdateManaBar(player.GetMana(),player.GetMaxMana());
        }
    }
}
