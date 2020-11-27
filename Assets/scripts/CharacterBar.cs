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

    private void OnDisable()
    {
        ClearBio();
    }

    public void ClearBio()
    {
        foreach (Transform o in biographyHolder)
        {
            Destroy(o.gameObject);
        }
    }
}
