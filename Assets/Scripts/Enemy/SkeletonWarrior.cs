using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Skeleton Warrior - will block attacks (see OnHit)
 */
public class SkeletonWarrior : MonoBehaviour, IEnemy
{
    public float Health;
    public float AttackCooldown;
    public float BlockTime;
    public float SpawnAttackBuffer;
    public ParticleSystem Particles;
    public AudioClip HitSFX;
    public AudioClip BlockSFX;

    private Animator _animator;
    private GameObject _player;
    private float _time_of_last_attack;
    private float _time_of_block_start;
    private bool _stunned;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _time_of_last_attack = Time.time + SpawnAttackBuffer;
        _player = GameObject.FindWithTag("Player"); //May cause performance issues, consider optimizing if there is a bottleneck
        _time_of_block_start = Time.time;
    }

    void Update()
    {
        //Check if dead
        if (Health < 0)
            return;

        if (Time.time > _time_of_block_start + BlockTime)
            _animator.SetBool("Block", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger)
            return;
        if (collision.tag == "Player" && !_animator.GetBool("Block") && !_stunned)
            if (_time_of_last_attack + AttackCooldown <= Time.time)
            {
                _animator.SetTrigger("Attack");
                _time_of_last_attack = Time.time;
                return;
            }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.isTrigger)
            return;
        if (collision.tag == "Player" && !_animator.GetBool("Block") && !_stunned)
            if (_time_of_last_attack + AttackCooldown <= Time.time)
            {
                _animator.SetTrigger("Attack");
                _time_of_last_attack = Time.time;
                return;
            }
    }

    /*
     * If hit with a normal attack from the front, the skeleton warrior should block and take no damage 
     * If hit with a normal attack from the back, the skeleton warrior should take damage
     * If hit with a stun attack, should be stun regardless if blocking or not
     */
    public void OnHit(int damage, bool stun, Vector3 particlePosition)
    {
        if (Health <= 0)
            return;

        bool facing_player = false;
        //if facing right and to the left of player
        if (transform.eulerAngles.y == 0 && transform.position.x < _player.transform.position.x)
            facing_player = true;
        if (transform.eulerAngles.y == 180 && transform.position.x > _player.transform.position.x)
            facing_player = true;

        if (!stun && facing_player && !_stunned && Health > 0)
        {
            _animator.ResetTrigger("Attack");
            _animator.SetBool("Block", true);
            _time_of_block_start = Time.time;

            GetComponent<AudioSource>().volume = 0.2f;
            GetComponent<AudioSource>().PlayOneShot(BlockSFX);
            return;
        }

        Instantiate(Particles, particlePosition, new Quaternion());
        GetComponent<AudioSource>().volume = stun ? 0.5f : 0.2f;
        GetComponent<AudioSource>().PlayOneShot(HitSFX);

        Health -= damage;
        if (Health <= 0)
        {
            _animator.SetBool("Dead", true);
            return;
        }

        else if (stun)
        {
            _animator.SetBool("Block", false);
            _animator.SetTrigger("Stun");

            _stunned = true;
            StopCoroutine(ResetStun());
            StartCoroutine(ResetStun());
        }
    }

    //Just used to not trigger an attack when stunned
    IEnumerator ResetStun()
    {
        yield return new WaitForSeconds(1);
        _animator.ResetTrigger("Attack");
        _stunned = false;
    }
}
