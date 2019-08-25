using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*
 * Manages display of items in the UI
 */

public class UIItem : MonoBehaviour, IPointerClickHandler
{
    public Item Item;
    public UIItem SelectedItem;

    private Image _image;
    

    void Start()
    {
        _image = GetComponent<Image>();
        SelectedItem = FindObjectOfType<SelectedItem>().GetComponent<UIItem>();
    }

    void Update()
    {
        _image.sprite = Item ? Item.InventorySprite : null;
        _image.color = Item ? Color.white : Color.clear;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Item temp_item = Item;
        Item = SelectedItem.Item;
        SelectedItem.Item = temp_item;
    }
}
