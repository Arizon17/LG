using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBar : MonoBehaviour
{
    public Text name;
    public Text description;
    public Text biographyPrefab;
    public Text hpText;
    public Text damageText;
    public Text critChanceText;
    public Text hitChanceText;
    public Image characterIcon;
    public Transform biographyHolder;
    public Transform skillHolder;
    public Image skillIconPrefab;

    private void OnDisable()
    {
        ClearBio();
        ClearSkills();
    }

    public void ClearBio()
    {
        foreach (Transform o in biographyHolder)
        {
            Destroy(o.gameObject);
        }
    }
    public void ClearSkills()
    {
        foreach (Transform o in skillHolder)
        {
            Destroy(o.gameObject);
        }
    }
}
