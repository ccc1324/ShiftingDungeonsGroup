using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonArcher : MonoBehaviour, IEnemy
{
    public float Health;
    public float AttackCooldown;
    public float SpawnAttackBuffer;
    public ParticleSystem Particles;
    public AudioClip HitSFX;

    private Animator _animator;
    private GameObject _player;
    private float _time_of_last_attack;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _time_of_last_attack = Time.time + SpawnAttackBuffer;
    }

    void Update()
    {
        //Check if dead
        if (Health < 0)
            return;

        //Check if time to attack
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
        GetComponent<AudioSource>().volume = (stun ? 0.5f : 0.2f) * OptionsManager.GetSoundVolume();
        GetComponent<AudioSource>().PlayOneShot(HitSFX);

        Health -= damage;
        if (Health <= 0)
        {
            _animator.SetBool("Dead", true);
        }

        else if (stun)
            _animator.SetTrigger("Stun");
    }

}
