using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillIcon : MonoBehaviour
{
    [SerializeField] private Image spellIcon;
    [SerializeField] private Image chooseIcon;
    public void SetIcon(Sprite icon)
    {
        spellIcon.sprite = icon;
    }

    public void ChooseIcon()
    {
        chooseIcon.gameObject.SetActive(true);
    }

    public void HideChooseIcon()
    {
        chooseIcon.gameObject.SetActive(false);
    }
}
