using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public AudioClip TriggeredSFX;
    public float TriggeredSFXVolume;
    public AudioClip ActivatedSFX;
    public float ActivatedSFXVolume;

    private Inventory _inventory;
    private Equipment _equipment;
    private DungeonManager _dungeon_manager;
    private Animator _animator;
    private AudioSource _audio_source;
    public AudioSource SecondaryAudioSource;
    public GameObject KeyPressAnimation;

    private bool _triggered;

    void Start()
    {
        _inventory = FindObjectOfType<Inventory>();
        _equipment = FindObjectOfType<Equipment>();
        _dungeon_manager = FindObjectOfType<DungeonManager>();
        _animator = GetComponent<Animator>();
        _audio_source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!_triggered)
            return;

        if (Input.GetKeyDown("j"))
        {
            _animator.SetTrigger("Activated");
            SecondaryAudioSource.PlayOneShot(ActivatedSFX);
            SecondaryAudioSource.volume = ActivatedSFXVolume;

            SaveData saveData = new SaveData();

            saveData.Level = _dungeon_manager.Level;
            saveData.MaxLevel = _dungeon_manager.MaxLevel;

            saveData.InventoryItemNames = new string[_inventory.InventorySlots.Count];
            for (int i = 0; i < _inventory.InventorySlots.Count; i++)
            {
                UIItem uiItem = _inventory.InventorySlots[i];
                saveData.InventoryItemNames[i] = uiItem.Item == null ? "" : uiItem.Item.name;
            }

            saveData.PrimaryWeaponName = _equipment.PrimaryWeapon.Item == null ? "" : _equipment.PrimaryWeapon.Item.name;
            saveData.SecondaryWeaponName = _equipment.SecondaryWeapon.Item == null ? "" : _equipment.SecondaryWeapon.Item.name;

            SaveSystem.Save(saveData);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _animator.SetBool("Triggered", true);
        _audio_source.clip = TriggeredSFX;
        _audio_source.volume = TriggeredSFXVolume;
        _audio_source.Play();
        StopAllCoroutines();

        _triggered = true;
        KeyPressAnimation.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _animator.SetBool("Triggered", false);
        StartCoroutine(FadeOutMusic(1));

        _triggered = false;
        KeyPressAnimation.SetActive(false);
    }

    public IEnumerator FadeOutMusic(float time)
    {
        float startTime = Time.time;
        float startVolume = _audio_source.volume;

        while (Time.time < startTime + time)
        {
            _audio_source.volume = Mathf.Lerp(startVolume, 0, (Time.time - startTime) / time);
            yield return null;
        }

        _audio_source.Stop();
    }
}
