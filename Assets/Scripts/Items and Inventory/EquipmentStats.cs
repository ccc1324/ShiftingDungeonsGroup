using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * Used to display item info in UI
 */
public class EquipmentStats : MonoBehaviour
{
    public Item Item;
    public TextMeshProUGUI ItemName;
    public TextMeshProUGUI ItemDamage;
    public TextMeshProUGUI ItemDPS;

    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;

    public void ClearItem()
    {
        Item = null;
        ItemName.text = "";
        ItemDamage.text = "";
        ItemDPS.text = "";
    }

    public void UpdateItem()
    {
        if (Item == null)
            return;

        ItemName.text = Item.ItemName;
        ItemDamage.text = "Damage: " + Mathf.Round(((float)Item.WeaponDamage) / 10).ToString();
        ItemDPS.text = "DPS: " + Mathf.Round((Item.WeaponDamage) / GetFramesPerHit(Item.WeaponClass) * 60 / 10).ToString();
    }

    //poor way of keeping track of weapon attack speed
    private float GetFramesPerHit(string weapon)
    {
        switch (Item.WeaponClass)
        {
            case "Sword":
                return 25;
            case "Spear":
                return 22;
            case "Staff":
                return 26;
            case "Dagger":
                return 17;
            case "Hammer":
                return 60;
            case "Axe":
                return 60;
        }
        return 30;
    }
}
