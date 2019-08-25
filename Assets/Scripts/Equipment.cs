using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public UIItem PrimaryWeapon;
    public UIItem SecondaryWeapon;

    private CanvasGroup _canvas_group;

    void Start()
    {
        _canvas_group = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (Input.GetKeyDown("o"))
        {
            _canvas_group.alpha = _canvas_group.alpha == 1 ? 0 : 1;
            _canvas_group.interactable = _canvas_group.interactable ? false : true;
            _canvas_group.blocksRaycasts = _canvas_group.blocksRaycasts ? false : true;
        }
    }
}
