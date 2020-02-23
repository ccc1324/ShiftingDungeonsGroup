using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropManager : MonoBehaviour
{
    [Header("Drop Rate Variables")]
    public int NullWeight;
    public int CommonWeight;
    public int UncommonWeight;
    public int RareWeight;
    public int EpicWeight;
    /*Suggested Weights: 244, 28, 7, 0, 1*/

    public int MaxNoDrops;
    public int MaxNoEpics;

    public int DropsSinceLastItem;
    public int DropsSinceLastEpic;

    [Header("Item Drop Behavior")]
    public GameObject ItemPrefab;


    public Item GetDrop(ItemSet common, ItemSet uncommon, ItemSet epic)
    {
        //DetermineDrop
        int totalWeight = NullWeight + CommonWeight + UncommonWeight + EpicWeight;
        int rng = Random.Range(0, totalWeight);


        if (DropsSinceLastItem >= MaxNoDrops)
            rng = Random.Range(NullWeight, totalWeight);
        if (DropsSinceLastEpic >= MaxNoEpics)
            rng = Random.Range(NullWeight + CommonWeight + UncommonWeight, totalWeight);

        if (rng < NullWeight)
        {
            DropsSinceLastItem++;
            DropsSinceLastEpic++;
            return null;
        }
        else if (rng < NullWeight + CommonWeight)
        {
            DropsSinceLastItem = 0;
            DropsSinceLastEpic++;
            rng = Random.Range(0, common.Items.Length);
            return common.Items[rng];
        }
        else if (rng < NullWeight + CommonWeight + UncommonWeight)
        {
            DropsSinceLastItem = 0;
            DropsSinceLastEpic++;
            rng = Random.Range(0, uncommon.Items.Length);
            return uncommon.Items[rng];
        }
        else
        {
            DropsSinceLastItem = 0;
            DropsSinceLastEpic = 0; ;
            rng = Random.Range(0, epic.Items.Length);
            return epic.Items[rng];
        }
    }

    public void SpawnItem(Item item, Vector3 position, float yOffset, float velocity)
    {
        if (item != null)
        {
            position = new Vector3(position.x, position.y + yOffset);
            GameObject clone = Instantiate(ItemPrefab, position, Quaternion.Euler(new Vector3(0, 0, -45)));
            clone.GetComponent<Rigidbody2D>().velocity = new Vector2(0, velocity);
            clone.GetComponent<GameItem>().Item = item;
        }
    }

    //Old version of GetDrop that uses 4 item sets
    public Item GetDrop(ItemSet common, ItemSet uncommon, ItemSet rare, ItemSet epic)
    {
        //DetermineDrop
        int totalWeight = NullWeight + CommonWeight + UncommonWeight + RareWeight + EpicWeight;
        int rng = Random.Range(0, totalWeight);


        if (DropsSinceLastItem >= MaxNoDrops)
            rng = Random.Range(NullWeight, totalWeight);
        if (DropsSinceLastEpic >= MaxNoEpics)
            rng = Random.Range(NullWeight + CommonWeight + UncommonWeight + RareWeight, totalWeight);

        
        if (rng < NullWeight)
        {
            DropsSinceLastItem++;
            DropsSinceLastEpic++;
            return null;
        }
        else if (rng < NullWeight + CommonWeight)
        {
            DropsSinceLastItem = 0;
            DropsSinceLastEpic++;
            rng = Random.Range(0, common.Items.Length);
            return common.Items[rng];
        }
        else if (rng < NullWeight + CommonWeight + UncommonWeight)
        {
            DropsSinceLastItem = 0;
            DropsSinceLastEpic++;
            rng = Random.Range(0, uncommon.Items.Length);
            return uncommon.Items[rng];
        }
        else if (rng < NullWeight + CommonWeight + UncommonWeight + RareWeight)
        {
            DropsSinceLastItem = 0;
            DropsSinceLastEpic++;
            rng = Random.Range(0, rare.Items.Length);
            return rare.Items[rng];
        }
        else
        {
            DropsSinceLastItem = 0;
            DropsSinceLastEpic = 0; ;
            rng = Random.Range(0, epic.Items.Length);
            return epic.Items[rng];
        }
    }
}
