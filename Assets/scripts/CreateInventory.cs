using System.Collections;
using System.Collections.Generic;
using FamilyWikGame;
using UnityEngine;

public class CreateInventory : MonoBehaviour
{
    [SerializeField] private ItemHolder itemHolderPrefab;

    public void Fire(Inventory inventory)
    {
        gameObject.SetActive(true);
        List<byte> itemsIds = inventory.GetInventoryItems();
        itemsIds.ForEach(p=>Debug.Log(p));
        foreach (var item in itemsIds)
        {
            Debug.Log("In foreach " + item);
            var itemHolder = Instantiate(itemHolderPrefab,transform);
            itemHolder.transform.localScale = new Vector3(1,1,1);
            //itemHolder.ChangeSprite(GameManager._instance.itemSo.GetSpriteById(item));

        }
    }

    public void Hide()
    {
        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }
        gameObject.SetActive(false);
    }
}
