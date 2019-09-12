using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadManager : MonoBehaviour
{
    public ItemDictionary ItemDictionary;

    private Inventory _inventory;
    private Equipment _equipment;
    private DungeonManager _dungeon_manager;

    void Start()
    {
        ItemDictionary.BuildDictionary();
    }

    void Update()
    {
        if (Input.GetKeyDown("y"))
        {
            _inventory = FindObjectOfType<Inventory>();
            _equipment = FindObjectOfType<Equipment>();
            _dungeon_manager = FindObjectOfType<DungeonManager>();

            SaveData saveData = SaveSystem.Load();

            _dungeon_manager.Level = saveData.Level;
            _dungeon_manager.MaxLevel = saveData.MaxLevel;

            for (int i = 0; i < 15; i++)
            {
                _inventory.InventorySlots[i].Item = saveData.InventoryItemNames[i] == "" ? null : ItemDictionary.GetItem(saveData.InventoryItemNames[i]);
            }

            _equipment.PrimaryWeapon.Item = saveData.PrimaryWeaponName == "" ? null : ItemDictionary.GetItem(saveData.PrimaryWeaponName);
            _equipment.SecondaryWeapon.Item = saveData.SecondaryWeaponName == "" ? null : ItemDictionary.GetItem(saveData.SecondaryWeaponName);
        }
    }
}
