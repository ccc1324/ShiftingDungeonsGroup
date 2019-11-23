using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
