using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Manages interaction between the player, items, and the inventory
 */

public class PlayerInventory : MonoBehaviour
{
    public Inventory Inventory;
    public Equipment Equipment;
    public List<GameObject> ToAddItems;
    public bool Stunned;

    void Start()
    {
        Inventory = FindObjectOfType<Inventory>();
        Equipment = FindObjectOfType<Equipment>();
    }

    void Update()
    {
        if (Stunned)
            return;

        if (Input.GetKeyDown("j"))
        {
            if (ToAddItems.Count != 0)
            {
                GameObject item = Inventory.AddItem(ToAddItems[0]);
                ToAddItems.Remove(item);
                Destroy(item);
            }
        }

        if (Input.GetKeyDown("i"))
        {
            CanvasGroup canvasGroup = Inventory.GetComponent<CanvasGroup>();
            canvasGroup.alpha = canvasGroup.alpha == 1 ? 0 : 1;
            canvasGroup.interactable = canvasGroup.interactable ? false : true;
            canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts ? false : true;
        }

        if (Input.GetKeyDown("o"))
        {
            CanvasGroup canvasGroup = Equipment.GetComponent<CanvasGroup>();
            canvasGroup.alpha = canvasGroup.alpha == 1 ? 0 : 1;
            canvasGroup.interactable = canvasGroup.interactable ? false : true;
            canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts ? false : true;
        }
    }
}
