using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EnemyTestMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DOLocalMove(new Vector3(-10, 0, 0), 10).SetEase(Ease.InBack).OnComplete(() =>
        {
            transform.DOLocalMove(new Vector3(20, 0, 0), 10);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
