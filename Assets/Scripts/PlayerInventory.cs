using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Manages interaction between the player, items, and the inventory
 */

public class PlayerInventory : MonoBehaviour
{
    public Inventory Inventory;
    public List<GameObject> ToAddItems;

    void Start()
    {
        Inventory = FindObjectOfType<Inventory>();
    }

    void Update()
    {
        if (Input.GetKeyDown("j"))
        {
            if (ToAddItems.Count != 0)
            {
                GameObject item = Inventory.AddItem(ToAddItems[0]);
                ToAddItems.Remove(item);
                Destroy(item);
            }
        }
    }
}
