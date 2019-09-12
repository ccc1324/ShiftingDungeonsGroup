using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackTrigger : MonoBehaviour
{
    public ParticleSystem ParticleEffects;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !collision.GetComponent<PlayerStun>().ParalyzeHeal)
        {
            collision.GetComponent<PlayerMovement>().Stop();
            collision.GetComponent<Animator>().SetTrigger("Stun");
            collision.GetComponent<Animator>().SetBool("Attacking", false);

            collision.GetComponent<PlayerStun>().DisablePlayerCombat(0.1f);

            if (tag == "Projectile")
            {
                Instantiate(ParticleEffects, transform.position, new Quaternion());
                Destroy(gameObject);
            }
        }

        if (collision.tag == "Room")
        {
            if (tag == "Projectile")
                Destroy(gameObject);
        }
    }
}
