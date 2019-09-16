using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<UIItem> InventorySlots;

    /*
     * Use to add items to the inventory
     * Returns the item added if successful
     * Return null if the inventory is full and item cannot be added
     */
    public GameObject AddItem (GameObject itemObject)
    {
        Item item = itemObject.GetComponent<GameItem>().Item;
        foreach (UIItem slot in InventorySlots)
        {
            if (slot.Item == null)
            {
                slot.Item = item;
                return itemObject;
            }
        }
        return null;
    }


}
