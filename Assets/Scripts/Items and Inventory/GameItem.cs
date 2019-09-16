using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * Manages the display of items in game
 */

public class GameItem : MonoBehaviour
{
    public Item Item;
    public PlayerInventory PlayerInventory;

    private PolygonCollider2D _collider;
    private PolygonCollider2D _trigger;


    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = Item.GameObjectSprite;
        _collider = gameObject.AddComponent<PolygonCollider2D>();
        _trigger = gameObject.AddComponent<PolygonCollider2D>();
        _trigger.isTrigger = true;
        StartCoroutine(DestroyOutOfBounds());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponentInParent<PlayerInventory>().ToAddItems.Add(gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponentInParent<PlayerInventory>().ToAddItems.Remove(gameObject);
        }
    }

    IEnumerator DestroyOutOfBounds()
    {
        while(transform.position.y > -100)
        {
            yield return new WaitForSeconds(30);
        }
        Destroy(gameObject);
    }
}
