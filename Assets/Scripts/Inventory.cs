using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<UIItem> InventorySlots;

    private CanvasGroup _canvas_group;

    void Start()
    {
        _canvas_group = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (Input.GetKeyDown("i"))
        {
            _canvas_group.alpha = _canvas_group.alpha == 1 ? 0 : 1;
            _canvas_group.interactable = _canvas_group.interactable ? false : true;
            _canvas_group.blocksRaycasts = _canvas_group.blocksRaycasts ? false : true;
        }
    }

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
