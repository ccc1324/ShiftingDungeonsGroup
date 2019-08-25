using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedItem : MonoBehaviour
{
    private UIItem _ui_item;

    void Start()
    {
        _ui_item = GetComponent<UIItem>();
    }

    void Update()
    {
        if (_ui_item.Item != null)
        {
            transform.position = Input.mousePosition;
        }
    }
}
