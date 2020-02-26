using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*
 * Manages display of items in the UI
 */

public class UIItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item Item;
    public UIItem SelectedItem;
    public ItemHoverPanel ItemHoverPanel;
    public EquipmentStats EquipmentStats;
    public bool Equipment;
    public bool DropItem;
    public GameObject ItemPrefab;
    public Transform Player;

    public AudioClip EquipSFX;
    public float EquipSFXVolume;
    public AudioClip DropSFX;
    public float DropSFXVolume;

    private AudioSource _audio_source;

    private Image _image;


    void Start()
    {
        _image = GetComponent<Image>();
        SelectedItem = FindObjectOfType<SelectedItem>().GetComponent<UIItem>();
        ItemHoverPanel = FindObjectOfType<ItemHoverPanel>();
        _audio_source = GetComponent<AudioSource>();
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
        if (Equipment && Item != null)
        {
            _audio_source.volume = EquipSFXVolume * PlayerPrefsController.GetSoundVolume();
            _audio_source.PlayOneShot(EquipSFX);
            EquipmentStats.Item = Item;
            EquipmentStats.UpdateItem();
        }
        if (Equipment && Item == null)
            EquipmentStats.ClearItem();
        if (DropItem && Item != null)
        {
            _audio_source.volume = DropSFXVolume * PlayerPrefsController.GetSoundVolume();
            _audio_source.PlayOneShot(DropSFX);
            GameObject clone = Instantiate(ItemPrefab, Player.position, Quaternion.Euler(new Vector3(0, 0, -45)));
            clone.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 20);
            clone.GetComponent<GameItem>().Item = Item;
            Item = null;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Item != null)
        {
            ItemHoverPanel.Item = Item;
            ItemHoverPanel.UpdateItem();
            ItemHoverPanel.transform.position = transform.position;
            ItemHoverPanel.Show();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemHoverPanel.Hide();
    }
}
