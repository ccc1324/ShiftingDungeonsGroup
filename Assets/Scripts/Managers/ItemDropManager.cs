using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropManager : MonoBehaviour
{
    public int NullWeight;
    public int CommonWeight;
    public int UncommonWeight;
    public int RareWeight;
    public int EpicWeight;

    public int MaxNoDrops;
    public int MaxNoEpics;

    public int DropsSinceLastItem;
    public int DropsSinceLastEpic;

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
