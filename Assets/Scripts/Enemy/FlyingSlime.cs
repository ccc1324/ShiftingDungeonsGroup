using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingSlime : MonoBehaviour, IEnemy
{
    public float Health;
    public float FlyingSpeed;
    public float ZigZagInterval;
    public float AttackInterval;
    public float AttackDuration;

    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private int _xDirectionScale = -1; //1 for positive, -1 for negative
    private int _yDirectionScale = -1; //1 for positive, -1 for negative
    private float _startTime;
    private float _lastDirectionChange;
    private float _lastTimeAttacking;
    private float _timeStartAttacking;
    private bool _resetTime = false;
    
    


    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _startTime = Time.time;
        _lastDirectionChange = _startTime;
        _lastTimeAttacking = Time.time;
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
            _rigidbody.gravityScale = .6f;
            _animator.SetBool("Dead", true);
            return;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Wall")
        {
            _xDirectionScale *= -1;
            _rigidbody.velocity = new Vector2(FlyingSpeed * _xDirectionScale, FlyingSpeed * _yDirectionScale);
        }
        else if (collision.collider.tag == "Room")
        {
            _yDirectionScale *= -1;
            _resetTime = true;
            _rigidbody.velocity = new Vector2(FlyingSpeed * _xDirectionScale, FlyingSpeed * _yDirectionScale);
        }
    }

    void FlyAround()
    {
        if (_animator.GetBool("Dead"))
        {
            return;
        }

        if (Time.time - _lastTimeAttacking > AttackInterval && !_animator.GetBool("Attacking")) 
        {
            _timeStartAttacking = Time.time;
            _animator.SetBool("Attacking", true);
        }
        
        if(_animator.GetBool("Attacking") && Time.time - _timeStartAttacking > AttackDuration)
        {
            _animator.SetBool("Attacking", false);
            _lastTimeAttacking = Time.time;
            return;
        }

        if (_animator.GetBool("Attacking") && Time.time - _timeStartAttacking < AttackDuration)
        {
            _rigidbody.velocity = new Vector2(0, 0);
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
        _rigidbody.velocity = new Vector2(FlyingSpeed*_xDirectionScale,FlyingSpeed*_yDirectionScale);
    }



}
