using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Moved)
            {
                var pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x,
                    Input.GetTouch(0).position.y, -2));
                transform.position = new Vector3(pos.x, pos.y, pos.z);
            }
        }
    }
}
