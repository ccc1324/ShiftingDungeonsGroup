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

    public enum Direction { None, Left, Right, Up, Down}
    public Direction OneWay;
    public Collider2D OneWayTrigger; //only needed if the trigger needs to be OneWay

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && 
            !other.GetComponent<PlayerStun>().ParalyzeHeal &&   //avoid damage during/after stun
            other.GetComponent<PlayerCombat>().enabled == true) //avoid additional damage on same frame after being hit once
        {
            //Check for OneWay Condition
            if (OneWay != Direction.None)
            {
                if (OneWay == Direction.Left)
                    if (other.transform.position.x + other.offset.x > transform.position.x + OneWayTrigger.offset.x)
                        return;
                if (OneWay == Direction.Right)
                {
                    float a = other.transform.position.x + other.offset.x;
                    float b = transform.position.x + other.offset.x;
                    if (other.transform.position.x + other.offset.x < transform.position.x + OneWayTrigger.offset.x)
                        return;
                }
                    
                if (OneWay == Direction.Up)
                    if (other.transform.position.y + other.offset.y < transform.position.y + OneWayTrigger.offset.y)
                        return;
                if (OneWay == Direction.Down)
                    if (other.transform.position.y + other.offset.y > transform.position.y + OneWayTrigger.offset.y)
                        return;
            }

            other.GetComponent<PlayerMovement>().Stop();
            other.GetComponent<Animator>().SetTrigger("Stun");
            other.GetComponent<Animator>().SetBool("Attacking", false);

            //Disables PlayerCombat and re-enables after 0.1 seconds, used to avoid some issues with stunning the player
            other.GetComponent<PlayerStun>().DisablePlayerCombat(0.1f); 
            other.GetComponent<Player>().OnHit(Damage);

            if (tag == "Projectile")
            {
                Instantiate(ParticleEffects, transform.position, new Quaternion());
                Destroy(gameObject);
            }
        }

        if (other.tag == "Room" || other.tag == "Wall")
        {
            if (tag == "Projectile")
                Destroy(gameObject);
        }
    }
}
