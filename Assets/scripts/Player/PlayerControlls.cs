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

    private Astar myAstar;
    
    private Stack<Vector3Int> path;
    private Vector3 goal;
    private Vector3 destination;
    private Vector3 current;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GetComponent<Inventory>();
        photonView = GetComponent<PhotonView>();
        stats = GetComponent<PlayerStats>();
        myAstar = GetComponent<Astar>();
        nickText.SetText(photonView.Owner.NickName);

        if (photonView.IsMine)
        {
            nickText.color = Color.green;
        }
        GameManager._instance.AddPlayer(this);
        
    }

    void InitPath(Vector3Int target)
    {
        path = null;
        target = new Vector3Int(target.x, target.y, 0);
        if (target != transform.position)
        {
            myAstar.Algorithm(out path, Vector3Int.FloorToInt(target));
        }

        if (path != null)
        {
            current = path.Peek();
            destination = path.Peek();
            goal = target;
        } 
    }
    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            if (Input.GetMouseButton(0))
            {
                Vector3Int worldPos = Vector3Int.FloorToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition)); 
                InitPath(worldPos);
            }
#endif
#if UNITY_ANDROID
            if (Input.touchCount > 0)
            {
                var touch = Input.touches[0];
                Vector3Int worldPos = Vector3Int.FloorToInt(Camera.main.ScreenToWorldPoint(touch.position));
                InitPath(worldPos);
            }
#endif
        }
    }

    void FixedUpdate()
    {
        if (path != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, 5 * Time.deltaTime);
            float distance = Vector2.Distance(destination, transform.position);
            if (distance <= 0f)
            {
                if (path.Count > 0)
                {
                    current = destination;
                    destination = path.Pop();
                }
                else
                {
                    path = null;
                }
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
            Debug.Log(tile is InteractableTile);
            if (tile is InteractableTile)
            {
                Debug.Log("yay, interactable!");
                InteractableTile _tile = (InteractableTile) tile;
                Debug.Log("need key to open " + _tile.needKeyToOpen);
                if (_tile.needKeyToOpen)
                {
                    if (_tile.basicKey)
                    {
                        if (inventory.GetBasicKeysCount() > 0)
                        {
                            inventory.RemoveBasicKeyFromInventory();
                            RemoveTileFromMap(component, component.layoutGrid.WorldToCell(point));
                        }
                        else return;
                    }
                    else
                    {
                        if (inventory.GetAdvancedKeyCount() > 0)
                        {
                            inventory.RemoveAdvancedKeyFromInventory();
                            RemoveTileFromMap(component, component.layoutGrid.WorldToCell(point));
                        }
                        else return;
                    }
                }
                else RemoveTileFromMap(component, component.layoutGrid.WorldToCell(point));
            }
            if (tile is PickUpableTile)
            {
                Debug.Log("yay, pick up!");
                PickUpableTile _tile = (PickUpableTile) tile;
                if (_tile.key)
                {
                    if (_tile.basicKeyToGive)
                        inventory.AddBasicKeyToInventory();
                    else
                        inventory.AddAdvancedKeyToInventory();
                }
                else
                    inventory.AddItemToInventory(_tile.itemId);

                RemoveTileFromMap(component, component.layoutGrid.WorldToCell(point));
                return;
            }
            if (tile is ChestTile)
            {
                Debug.Log("Yay, chest!");
                ChestTile _tile = (ChestTile)tile;
                if (!_tile.itemSetWithChanceBool)
                    inventory.AddItemRangeToInventory(_tile.GetItems());
                else
                    inventory.AddItemRangeToInventory(_tile.GetItemsWithChange());
                return;
            }
        }
    }

    void RemoveTileFromMap(Tilemap map, Vector3Int pos)
    {
        map.SetTile(pos, null);
    }
}
