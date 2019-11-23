using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonArcher : MonoBehaviour, IEnemy
{
    public float Health;
    public float AttackCooldown;
    public float SpawnAttackBuffer;
    public ParticleSystem Particles;

    private Animator _animator;
    private GameObject _player;
    private float _time_of_last_attack;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _time_of_last_attack = Time.time + SpawnAttackBuffer;
        _player = GameObject.FindWithTag("Player"); //May cause performance issues, consider optimizing if there is a bottleneck
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

        if (_player.transform.position.x > transform.position.x)
            transform.eulerAngles = new Vector3(0, 0, 0);
        else
            transform.eulerAngles = new Vector3(0, 180, 0);

    }

    public void OnHit(int damage, bool stun)
    {
        Health -= damage;
        if (Health < 0)
            _animator.SetBool("Dead", true);

        else if (stun)
            _animator.SetTrigger("Stun");
    }

    public ParticleSystem GetParticles()
    {
        return Particles;
    }

    public float GetHealth()
    {
        return Health;
    }
}
