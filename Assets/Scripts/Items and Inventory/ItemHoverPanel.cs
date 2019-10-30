using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemHoverPanel : MonoBehaviour
{
    public Item Item;
    public Image ItemImage;
    public TextMeshProUGUI ItemName;
    public TextMeshProUGUI ItemClass;
    public TextMeshProUGUI ItemDescription;
    public TextMeshProUGUI ItemStats;

    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;

    void Start()
    {
        _rectTransform = ItemImage.GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void UpdateItem()
    {
        ItemImage.sprite = Item.GameObjectSprite;

        if (Item != null && (Item.WeaponClass == "Axe" || Item.WeaponClass == "Hammer"))
            _rectTransform.localPosition = new Vector2(0, -30);
        else
            _rectTransform.localPosition = Vector2.zero;

        ItemName.text = Item.ItemName;
        ItemClass.text = Item.WeaponClass;
        ItemDescription.text = Item.ItemDescription;
        ItemStats.text = "Damage: " + Mathf.Round(((float) Item.WeaponDamage)/10).ToString();
    }

    public void Show()
    {
        _canvasGroup.alpha = 1;
    }

    public void Hide()
    {
        _canvasGroup.alpha = 0;
    }
}
