using UnityEngine;

public class MouseFollow : MonoBehaviour
{
    private GameObject player;
    private void Start()
    {
        player = transform.parent.gameObject;
    }
}
