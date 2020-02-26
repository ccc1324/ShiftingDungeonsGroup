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

    public AudioClip PickupSFX;
    public float PickupSFXVolume;
    public AudioClip OpenSFX;
    public AudioClip CloseSFX;
    public float OpenCloseSFXVolume;
    private AudioSource _audio_source;

    void Start()
    {
        Inventory = FindObjectOfType<Inventory>();
        Equipment = FindObjectOfType<Equipment>();
        _audio_source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Stunned)
            return;

        if (Input.GetKeyDown("j"))
        {
            if (ToAddItems.Count != 0)
            {
                _audio_source.volume = PickupSFXVolume * OptionsManager.GetSoundVolume();
                _audio_source.PlayOneShot(PickupSFX);
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

            canvasGroup = Equipment.GetComponent<CanvasGroup>();
            canvasGroup.alpha = canvasGroup.alpha == 1 ? 0 : 1;
            canvasGroup.interactable = canvasGroup.interactable ? false : true;
            canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts ? false : true;
            Equipment.PrimaryWeapon.EquipmentStats.UpdateItem();
            Equipment.SecondaryWeapon.EquipmentStats.UpdateItem();

            _audio_source.volume = OpenCloseSFXVolume * OptionsManager.GetSoundVolume();
            if (canvasGroup.alpha == 1)
                _audio_source.PlayOneShot(OpenSFX);
            else
                _audio_source.PlayOneShot(CloseSFX);
        }

        if (Input.GetKeyDown("o"))
        {
            
        }
    }
}
