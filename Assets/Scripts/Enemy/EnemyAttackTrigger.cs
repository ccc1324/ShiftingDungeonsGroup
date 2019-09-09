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
            collision.GetComponent<Animator>().SetBool("Attacking", false);

            player.enabled = false; //need to diable temporaily to get around animator overiding stun
            StartCoroutine(ReEnable(player));

            if (tag == "Projectile")
            {
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<Collider2D>().enabled = false;
                Destroy(gameObject, 0.2f);
            }
        }

        if (collision.tag == "Room")
        {
            if (tag == "Projectile")
                Destroy(gameObject);
        }
    }

    IEnumerator ReEnable(PlayerCombat player)
    {
        yield return new WaitForSeconds(0.05f);
        player.enabled = true;
    }
}
