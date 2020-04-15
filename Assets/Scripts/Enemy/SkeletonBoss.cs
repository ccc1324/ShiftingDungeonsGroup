using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBoss : MonoBehaviour, IEnemy
{
    public float Health = 1f;
    public float AttackCooldown = 1f;
    public float ChargeCooldown = 1f;
    public float ChargeDelay = 1f;
    public float StunDuration = 1f;
    public float PlatformAttackRange = 1f;
    public float PlatformHeight = 1f;   //For now, since the room isn't ready, to determine if the player is on or
                                        //below the platforms, I will check if the player position is > or < the height of the platforms in LedgesRoom_Slime

    private bool _stunned = false;
    private bool _charging = false; //When the enemy is mid charge

    public ParticleSystem Particles;

    private float _time_of_last_attack;
    private float _time_of_last_charge = 0f;
    private float _direction;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private Transform _player_location;
    private DungeonManager _dungeon_manager;
    private AudioSource _audio_source;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _player_location = FindObjectOfType<PlayerInventory>().transform;
        _dungeon_manager = FindObjectOfType<DungeonManager>();
        _audio_source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
         * State 1: Dead
         * State 2: Walking to player -> USE ANIMATION BEHAVIORS!!!!
         * State 3: Attacking Player on Platform
         * State 4: Charging
         * State 5: Stunned
         */

        //Update Charge bool
        if (!_stunned && !_charging)
        {
            if (_player_location.position.y < PlatformHeight &&
                Time.time > _time_of_last_charge + ChargeCooldown + StunDuration)
            {
                //Charge
                _charging = true;
                _time_of_last_charge = Time.time;
                _animator.SetBool("Charging", true);

                
            }//We walk by default so no reason have a trigger for it
        }

        //_direction = Mathf.Sign(_player_location.position.x - transform.position.x);

    }

    public void OnHit(int damage, bool stun, Vector3 particlePosition)
    {
        Debug.Log("Skeleton boss was hit");
        if (Health <= 0)
            return;

        _direction = Mathf.Sign(_player_location.position.x - transform.position.x);

        Instantiate(Particles, particlePosition, new Quaternion());
        _audio_source.volume = (stun ? 0.5f : 0.2f) * OptionsManager.GetSoundVolume();
        //_audio_source.PlayOneShot(HitSFX);

        Health -= damage;
        if (Health <= 0)
        {
            //_audio_source.PlayOneShot(DeathSFX);

            _animator.SetBool("Dead", true);
            //_dungeon_manager.BossDefeated = true;
            StopAllCoroutines();
            Destroy(gameObject, 1f);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.isTrigger)
            return;
        if (collision.tag == "Player" && !_stunned && !_charging)
            if (_time_of_last_attack + AttackCooldown <= Time.time)
            {
                _animator.SetTrigger("Attack");
                _time_of_last_attack = Time.time;
                return;
            }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.collider.tag);
        if (_charging)
        {
            if (collision.collider.tag == "Player")
            {
                //Stop charge but dont stun
                _charging = false;
                _animator.SetBool("Charging", false);
                Stop();
            }
            else if (collision.collider.tag == "Wall")
            {
                Stop();
                Debug.Log("Henlo");
                //Stop charge and stun
                _charging = false;
                _stunned = true;
                _animator.SetBool("Charging", false);
                _animator.SetBool("Stunned", true);
                StartCoroutine(ResetStun());
            }
        }
    }

    private void Stop()
    {
        _rigidbody.velocity = Vector2.zero;
    }

    IEnumerator ResetStun()
    {
        yield return new WaitForSeconds(StunDuration);
        _stunned = false;
        _animator.SetBool("Stunned", false);
    }

}
