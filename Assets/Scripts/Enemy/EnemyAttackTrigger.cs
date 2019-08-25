using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerCombat player = collision.GetComponent<PlayerCombat>();
            
            collision.GetComponent<PlayerMovement>().Stop();
            collision.GetComponent<Animator>().SetTrigger("Stun");
            player.enabled = false;
            collision.GetComponent<Animator>().SetBool("Attacking", false);

            if (tag == "Projectile")
                Destroy(gameObject);
        }

        if (collision.tag == "Room")
        {
            if (tag == "Projectile")
                Destroy(gameObject);
        }
    }
}
