using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SkillTargetPanel : MonoBehaviour
{
    [SerializeField] private Image first, second;

    public void Init(Sprite firstSprite, Sprite secondSprite)
    {
        first.sprite = firstSprite;
        second.sprite = secondSprite;
        gameObject.SetActive(true);
    }

    public Tween AnimationSkill()
    {
        Vector3 pos = second.transform.position;
        Tween tween = second.transform.DOMove(first.transform.position, 0.65f).OnComplete(() =>
        {
            second.transform.DOMove(pos, 0.35f);
        });
        return tween;
    }
}
