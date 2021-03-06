﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeFlying : MonoBehaviour, IEnemy
{
    public float Health;
    public float XFlyingSpeed;
    public float YFlyingSpeed;
    public float ZigZagInterval;
    public float DeathGravityScale;

    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private int _xDirectionScale = -1; //1 for positive, -1 for negative
    private int _yDirectionScale = -1; //1 for positive, -1 for negative
    private float _startTime;
    private float _lastDirectionChange;
    private bool _resetTime = false;
    

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _startTime = Time.time;
        _lastDirectionChange = _startTime;
        _rigidbody.velocity = new Vector2(XFlyingSpeed * _xDirectionScale, YFlyingSpeed * _yDirectionScale);
    }


    void Update()
    {
        FlyAround();
    }

    public void OnHit(int damage, bool stun, Vector3 particlePosition)
    {
        if (Health <= 0)
            return;


        Health -= damage;
        if (Health <= 0)
        {
            _rigidbody.velocity = new Vector2(0, 0);
            _rigidbody.gravityScale = DeathGravityScale;
            _animator.SetBool("Dead", true);
            return;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Wall")
        {
            _xDirectionScale *= -1;
            _rigidbody.velocity = new Vector2(XFlyingSpeed * _xDirectionScale, YFlyingSpeed * _yDirectionScale);
        }
        else if (collision.collider.tag == "Room")
        {
            _yDirectionScale *= -1;
            _resetTime = true;
            _rigidbody.velocity = new Vector2(XFlyingSpeed * _xDirectionScale, YFlyingSpeed * _yDirectionScale);
        }
    }

    void FlyAround()
    {
        if (Health <= 0)
        {
            return;
        }

        if (_resetTime)
        {
            _lastDirectionChange = Time.time;
            _resetTime = false;
        }

        if (Time.time - _lastDirectionChange >  ZigZagInterval)
        {
            _yDirectionScale *= -1;
            _lastDirectionChange = Time.time;
        } 
        _rigidbody.velocity = new Vector2(XFlyingSpeed*_xDirectionScale,YFlyingSpeed*_yDirectionScale);
    }



}
