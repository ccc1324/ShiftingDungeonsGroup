using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingSlime : MonoBehaviour, IEnemy
{
    public float Health;
    public float FlyingSpeed;
    public float StartMovementBuffer;
    public float ZigZagInterval;

    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private int _xDirectionScale = -1; //1 for positive, -1 for negative
    private int _yDirectionScale = 1; //1 for positive, -1 for negative
    private float _startTime;
    private float _lastDirectionChange;
    


    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _startTime = Time.time;
        _lastDirectionChange = _startTime;
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
            _animator.SetBool("Dead", true);
            return;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Wall")
        {
            _xDirectionScale *= -1;
        }
        if (collision.collider.tag == "Room")
        {
            _yDirectionScale = 1;
        }
    }

    void FlyAround()
    {
        if (Time.time > _startTime + StartMovementBuffer && Time.time - _lastDirectionChange >  ZigZagInterval)
        {
            _lastDirectionChange = Time.time;
            _yDirectionScale *= -1;
        }
        _rigidbody.velocity = new Vector2(FlyingSpeed*_xDirectionScale,FlyingSpeed*_yDirectionScale);
    }


}
