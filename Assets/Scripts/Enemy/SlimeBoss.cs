using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlimeBoss : MonoBehaviour, IEnemy
{
    public float AttackCooldown;
    public float RangedCooldown;
    public float Movespeed;
    public float JumpForce;
    public float Health;
    public float HealthTransformOne;
    public float HealthTransformTwo;
    public Color ColorIdle;
    public Color ColorAngry;
    public GameObject TransformProjectile;
    public GameObject RangedProjectile;

    public ParticleSystem Particles;

    public bool Grounded;
    public bool Attacking;
    public bool Dead;

    private float _time_of_last_attack;
    private float _time_of_last_ranged;
    private float _direction;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private Transform _player_location;
    private SpriteRenderer _sprite;
    private DungeonManager _dungeon_manager;

    private bool _idle;

    private Color _tempColor;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _player_location = FindObjectOfType<PlayerInventory>().transform;
        _sprite = GetComponent<SpriteRenderer>();
        _dungeon_manager = FindObjectOfType<DungeonManager>();

        _idle = true;
    }

    void LateUpdate() //color won't change for normal update
    {
        if (_idle)
        {
            Color tempColor = Color.Lerp(ColorAngry, ColorIdle, (Health - 800) / 200);
            _sprite.color = Color.Lerp(tempColor, _tempColor, 0.97f);
        }

        if (Health < HealthTransformOne)
        {
            _sprite.color = ColorAngry;
            _animator.SetBool("Idle", false);
            _time_of_last_attack = Time.time;
            Attacking = true;

            HealthTransformOne = Mathf.NegativeInfinity;
            _idle = false;

            _direction = Mathf.Sign(_player_location.position.x - transform.position.x);
        }
    }

    void Update()
    {
        if (Health >= HealthTransformOne)
            _tempColor = _sprite.color;

        Stop();

        if (Dead)
        {
            _dungeon_manager.BossDefeated = true;
            Destroy(gameObject);
        }

        if (Health < HealthTransformTwo)
        {
            StopAllCoroutines();
            Stop();
            _animator.SetTrigger("Ranged");
            Attacking = false;
            StartCoroutine("Shower");

            HealthTransformTwo = Mathf.NegativeInfinity;

            _direction = Mathf.Sign(_player_location.position.x - transform.position.x);
        }

        if (Attacking && Time.time - _time_of_last_attack > AttackCooldown && Health > 0)
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
    }

    public void OnHit(int damage, bool stun)
    {
        if (Health <= 0)
            return;

        //Instantiate(Particles, transform.position, new Quaternion());

        _direction = Mathf.Sign(_player_location.position.x - transform.position.x);
        Health -= damage;
        if (Health <= 0)
        {
            _animator.SetBool("Dead", true);
            Attacking = false;
        }
    }

    public void Stop()
    {
        _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.5f);

        _rigidbody.velocity = new Vector2(0, JumpForce);
        _animator.ResetTrigger("Attack");
        Grounded = false;
        _animator.SetBool("Grounded", false);

        StartCoroutine(MoveTowardsPlayer());

        yield return new WaitForSeconds(0.2f);

        Grounded = false;
        _animator.SetBool("Grounded", false);

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
        float time = Time.time;
        while (Time.time - time < 0.2f)
        {
            _rigidbody.velocity = new Vector2(Movespeed * Time.deltaTime * _direction, _rigidbody.velocity.y);
            yield return null;
        }

        Grounded = false;

        while (Grounded == false)
        {
            _rigidbody.velocity = new Vector2(Movespeed * Time.deltaTime * _direction, _rigidbody.velocity.y);
            yield return null;
        }
    }

    IEnumerator Shower()
    {
        yield return new WaitForSeconds(2f);

        GameObject projectile = Instantiate(TransformProjectile, transform.position, new Quaternion());
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, 30);

        _animator.ResetTrigger("Ranged");
        Attacking = true;
        _time_of_last_attack = Time.time;
        _time_of_last_ranged = Time.time;

        Vector2 room = GameObject.FindGameObjectWithTag("Room").transform.position;

        while (Attacking)
        {
            if (Time.time - _time_of_last_ranged > RangedCooldown)
            {
                float rng = Random.Range(-8, 8);
                Instantiate(RangedProjectile, room + new Vector2(rng, 6), new Quaternion());
                _time_of_last_ranged = Time.time;
            }
            yield return null;
        }
    }

    public ParticleSystem GetParticles()
    {
        return Particles;
    }
}
