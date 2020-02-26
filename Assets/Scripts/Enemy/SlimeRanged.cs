using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlimeRanged : MonoBehaviour, IEnemy
{
    public float AttackCooldown;
    public float Health;

    public ParticleSystem Particles;

    public AudioClip HitSFX;

    public bool Stunned;

    private float _time_of_last_attack;
    private Animator _animator;
    private Transform _player_location;
    private ItemDropManager _item_drop_manager;
    private AudioSource _audio_source;

    void Start()
    {
        _time_of_last_attack = Time.time;
        _animator = GetComponent<Animator>();
        _player_location = FindObjectOfType<PlayerInventory>().transform;
        _item_drop_manager = FindObjectOfType<ItemDropManager>();
        _audio_source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Health < 0)
            return;

        if (Stunned)
            return;

        if (Time.time - _time_of_last_attack > AttackCooldown && Health > 0)
        {
            _animator.SetTrigger("Attack");
            _time_of_last_attack = Time.time;
        }
    }

    public void OnHit(int damage, bool stun, Vector3 particlePosition)
    {
        if (Health <= 0)
            return;

        Instantiate(Particles, particlePosition, new Quaternion());
        _audio_source.volume = (stun ? 0.5f : 0.2f) * OptionsManager.GetSoundVolume();
        _audio_source.PlayOneShot(HitSFX);

        Health -= damage;
        if (Health <= 0)
        {
            _animator.SetBool("Dead", true);
        }

        else if (stun)
            _animator.SetTrigger("Stun");
    }
}
