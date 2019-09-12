using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Set", menuName = "Item Set")]
public class ItemSet : ScriptableObject
{
    public Item[] Items;
}
