﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlimeMelee : MonoBehaviour, IEnemy
{
    public float AttackCooldown;
    public float Movespeed;
    public float JumpForce;
    public float Health;

    [System.Serializable]
    public struct ItemSets
    {
        public ItemSet Common;
        public ItemSet Uncommon;
        public ItemSet Epic;
    }
    public ItemSets ItemDropSets;
    public GameObject Item;

    public ParticleSystem Particles;

    public AudioClip JumpSFX;

    public bool Grounded;
    public bool Stunned;
    public bool Dead;

    private float _time_of_last_attack;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private Transform _player_location;
    private ItemDropManager _item_drop_manager;
    private AudioSource _audio_source;

    void Start()
    {
        _time_of_last_attack = Time.time;
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _player_location = FindObjectOfType<PlayerInventory>().transform;
        _item_drop_manager = FindObjectOfType<ItemDropManager>();
        _audio_source = GetComponent<AudioSource>();
    }

    void Update()
    {
        Stop();

        if (Dead)
        {
            Item item = _item_drop_manager.GetDrop(ItemDropSets.Common, ItemDropSets.Uncommon, ItemDropSets.Epic);
            EnemyFunctions.SpawnItem(Item, item, transform.position);
            Destroy(gameObject);
        }

        if (Stunned)
            return;

        if (Time.time - _time_of_last_attack > AttackCooldown && Health > 0)
        {
            _animator.SetTrigger("Attack");
            _time_of_last_attack = Time.time;
            StartCoroutine(Attack());
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Room")
        {
            Grounded = true;
            _animator.SetBool("Grounded", true);
        }

        if (collision.collider.tag == "EnemyPlatform")
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }

    public void OnHit(int damage, bool stun)
    {
        if (Health <= 0)
            return;

        if (stun)
        {
            Stop();
            _animator.SetTrigger("Stun");
            _animator.SetFloat("VerticalSpeed", 0);
            StopAllCoroutines();
        }
        
        Health -= damage;
        if (Health <= 0)
        {
            _animator.SetBool("Dead", true);
            Stop();
            StopAllCoroutines();
        }
    }

    public void Stop()
    {
        _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(1f);

        _audio_source.PlayOneShot(JumpSFX);

        _rigidbody.velocity = new Vector2(0, JumpForce);
        _animator.ResetTrigger("Attack");
        Grounded = false;
        _animator.SetBool("Grounded", false);

        StartCoroutine(MoveTowardsPlayer());

        while (Grounded == false)
        {
            _animator.SetFloat("VerticalSpeed", _rigidbody.velocity.y);
            yield return null;
        }
        _animator.SetFloat("VerticalSpeed", 0);
        Stop();
    }

    IEnumerator MoveTowardsPlayer()
    {
        float direction = Mathf.Sign(_player_location.position.x - transform.position.x);
        while (Grounded == false)
        {
            _rigidbody.velocity = new Vector2(Movespeed * Time.fixedDeltaTime * direction, _rigidbody.velocity.y);
            yield return null;
        }
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
