using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMovement : MonoBehaviour
{
    public GameObject Player;
    public bool Animated;

    private void Start()
    {
        Player = FindObjectOfType<PlayerMovement>().gameObject;
        if (Player == null)
        {
            Debug.Log("Wall could not find player");
        }
    }

    void Update()
    {
        if (Animated) //Enable animated walls that appear when player moves
        {
            gameObject.GetComponent<Renderer>().enabled = Player.GetComponent<Rigidbody2D>().velocity.x != 0 ? true : false;
        }
        transform.position = new Vector3(Player.transform.position.x, transform.position.y, transform.position.z);
    }
}
