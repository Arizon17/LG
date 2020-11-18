using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHolder : MonoBehaviour
{
    [SerializeField] private Image itemImage;

    public void ChangeSprite(Sprite sprite)
    {
        itemImage.sprite = sprite;
    }
}
