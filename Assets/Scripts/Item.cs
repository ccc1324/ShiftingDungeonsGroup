using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public string ItemName;
    public string ItemDescription;
    public string WeaponClass;
    public int WeaponDamage;
    public Sprite GameObjectSprite;
    public Sprite InventorySprite;
    
    void Start()
    {

    }

    public int GetWeaponAnimationKey()
    {
        switch (WeaponClass)
        {
            case "Sword":
                return 1;
            case "Axe":
                return 2;
            case "Dagger":
                return 3;
            case "Spear":
                return 4;
            case "Staff":
                return 5;
            case "Hammer":
                return 6;
            default:
                Debug.Log("Invalid Weapon Class");
                return 0;
        }
    }
}
