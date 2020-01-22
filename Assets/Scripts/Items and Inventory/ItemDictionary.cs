using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Dictionary", menuName = "Item Dictionary")]
public class ItemDictionary : ScriptableObject
{
    public Item[] ItemList;
    public Dictionary<string, Item> Dictionary;

    public Item GetItem(string name)
    {
        Dictionary.TryGetValue(name, out Item item);
        return item;
    }

    public void BuildDictionary()
    {
        Dictionary = new Dictionary<string, Item>();

        foreach (Item item in ItemList)
        {
            Dictionary.Add(item.name, item);
        }
    }
}
