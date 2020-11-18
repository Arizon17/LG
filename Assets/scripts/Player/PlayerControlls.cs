using System;
using System.Collections;
using System.Collections.Generic;
using FamilyWikGame;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerControlls : MonoBehaviourPunCallbacks
{
    private PhotonView photonView;

    [SerializeField] private Inventory inventory;
    [SerializeField] private TextMeshPro nickText;
    private PlayerStats stats;
    // Start is called before the first frame update
    void Start()
    {
        inventory = GetComponent<Inventory>();
        photonView = GetComponent<PhotonView>();
        stats = GetComponent<PlayerStats>();

        nickText.SetText(photonView.Owner.NickName);

        if (photonView.IsMine)
        {
            nickText.color = Color.green;
        }
        GameManager._instance.AddPlayer(this);
        
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

            if (Input.GetKeyDown(KeyCode.B))
            {
                GameManager._instance.ShowInventory(inventory);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Interactable"))
        {
            var point = collision.contacts[0].point;
            var component = collision.gameObject.GetComponent<Tilemap>();
            TileBase tile = component.GetTile(component.layoutGrid.WorldToCell(point));
            if (tile is ChestTile)
            {
                ChestTile _tile = (ChestTile)tile;
                inventory.AddItemRangeToInventory(_tile.GetItems());
                component.SetTile(component.layoutGrid.WorldToCell(point), null);
            }
        }
    }
}
