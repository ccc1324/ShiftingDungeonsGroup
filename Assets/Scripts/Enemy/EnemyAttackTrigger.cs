using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Attach to any object with a trigger2D to implement physical damage
 */
public class EnemyAttackTrigger : MonoBehaviour
{
    public ParticleSystem ParticleEffects;
    public int Damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && 
            !collision.GetComponent<PlayerStun>().ParalyzeHeal &&   //avoid damage during/after stun
            collision.GetComponent<PlayerCombat>().enabled == true) //avoid additional damage on same frame after being hit once
        {
            collision.GetComponent<PlayerMovement>().Stop();
            collision.GetComponent<Animator>().SetTrigger("Stun");
            collision.GetComponent<Animator>().SetBool("Attacking", false);

            //Disables PlayerCombat and re-enables after 0.1 seconds, used to avoid some issues with stunning the player
            collision.GetComponent<PlayerStun>().DisablePlayerCombat(0.1f); 
            collision.GetComponent<Player>().OnHit(Damage);

            if (tag == "Projectile")
            {
                Instantiate(ParticleEffects, transform.position, new Quaternion());
                Destroy(gameObject);
            }
        }

        if (collision.tag == "Room" || collision.tag == "Wall")
        {
            if (tag == "Projectile")
                Destroy(gameObject);
        }
    }
}
