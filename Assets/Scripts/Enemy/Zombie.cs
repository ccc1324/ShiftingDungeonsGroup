using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour, IEnemy
{
    public float Health;
    public float AttackCooldown;
    public float SpawnAttackBuffer;
    public ParticleSystem Particles;
    public AudioClip HitSFX;

    private Animator _animator;
    private float _time_of_last_attack;
    private bool _stunned;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _time_of_last_attack = Time.time + SpawnAttackBuffer;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger)
            return;
        if (collision.tag == "Player" && !_stunned)
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
        if (collision.tag == "Player" && !_stunned)
            if (_time_of_last_attack + AttackCooldown <= Time.time)
            {
                _animator.SetTrigger("Attack");
                _time_of_last_attack = Time.time;
                return;
            }
    }

    public void OnHit(int damage, bool stun, Vector3 particlePosition)
    {
        if (Health <= 0)
            return;

        Instantiate(Particles, particlePosition, new Quaternion());
        GetComponent<AudioSource>().volume = (stun ? 0.5f : 0.2f) * PlayerPrefsController.GetSoundVolume();
        GetComponent<AudioSource>().PlayOneShot(HitSFX);

        Health -= damage;
        if (Health <= 0)
        {
            _animator.SetBool("Dead", true);
            return;
        }

        else if (stun)
        {
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
