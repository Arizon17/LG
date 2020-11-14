using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerControlls : MonoBehaviourPunCallbacks
{
    private PhotonView photonView;

    private PlayerStats stats;
    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        stats = GetComponent<PlayerStats>();

    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Time.deltaTime * 5 * Vector3.up);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Time.deltaTime * 5 * Vector3.left);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Time.deltaTime * 5 * Vector3.right);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Time.deltaTime * 5 * Vector3.down);
            }

            if (Input.GetKey(KeyCode.Return))
            {

            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.gameObject.name);
    }
}
