using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectHolder : MonoBehaviour
{
    public EffectSO effect;
    public void Init(EffectSO effect)
    {
        this.effect = effect;
        GetComponent<Image>().sprite = effect.Effect.effectIcon;
        this.effect.Effect.effectHolder = gameObject;
    }

    void Update()
    {
        if (effect.Effect.duration <= 0) Destroy(gameObject);
    }
}
