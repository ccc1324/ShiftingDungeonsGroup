using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private Inventory _inventory;
    private Equipment _equipment;
    private DungeonManager _dungeon_manager;

    void Start()
    {
        _inventory = FindObjectOfType<Inventory>();
        _equipment = FindObjectOfType<Equipment>();
        _dungeon_manager = FindObjectOfType<DungeonManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown("u"))
        {
            SaveData saveData = new SaveData();

            saveData.Level = _dungeon_manager.Level;
            saveData.MaxLevel = _dungeon_manager.MaxLevel;

            saveData.InventoryItemNames = new string[15];
            for (int i = 0; i < 15; i++)
            {
                UIItem uiItem = _inventory.InventorySlots[i];
                saveData.InventoryItemNames[i] = uiItem.Item == null ? "" : uiItem.Item.name;
            }

            saveData.PrimaryWeaponName = _equipment.PrimaryWeapon.Item == null ? "" : _equipment.PrimaryWeapon.Item.name;
            saveData.SecondaryWeaponName = _equipment.SecondaryWeapon.Item == null ? "" : _equipment.SecondaryWeapon.Item.name;

            SaveSystem.Save(saveData);
        }
    }
}
