using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyFunctions
{
    public static void SpawnItem(GameObject itemPrefab, Item item, Vector3 position)
    {
        if (item != null)
        {
            position = new Vector3(position.x, position.y + 0.2f);
            GameObject clone = GameObject.Instantiate(itemPrefab, position, Quaternion.Euler(new Vector3(0, 0, -45)));
            clone.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 20);
            clone.GetComponent<GameItem>().Item = item;
        }
    }

}
