using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlimeRanged : MonoBehaviour, IEnemy
{
    public float AttackCooldown;
    public float Health;
    public GameObject Projectile;
    public float ProjectileHangTime;

    [System.Serializable]
    public struct ItemSets
    {
        public ItemSet Common;
        public ItemSet Uncommon;
        public ItemSet Rare;
        public ItemSet Epic;
    }
    public ItemSets ItemDropSets;
    public GameObject Item;

    public ParticleSystem Particles;

    public AudioClip AttackSFX;

    public bool Stunned;
    public bool Dead;

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
        if (Dead)
        {
            Item item = _item_drop_manager.GetDrop(ItemDropSets.Common, ItemDropSets.Uncommon, ItemDropSets.Rare, ItemDropSets.Epic);
            if (item != null)
            {
                GameObject clone = Instantiate(Item, transform.position, Quaternion.Euler(new Vector3(0, 0, -45)));
                clone.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 20);
                clone.GetComponent<GameItem>().Item = item;
            }
            Destroy(gameObject);
        }

        if (Stunned)
            return;

        if (Time.time - _time_of_last_attack > AttackCooldown && Health > 0)
        {
            _animator.SetBool("Attacking", true);
            _time_of_last_attack = Time.time;
            StartCoroutine(Attack());
        }
    }

    public void OnHit(int damage, bool stun)
    {
        if (Health <= 0)
            return;
        
        if (stun)
        {
            _animator.SetTrigger("Stun");
            StopAllCoroutines();
        }

        Health -= damage;
        if (Health <= 0)
        {
            _animator.SetBool("Dead", true);
            StopAllCoroutines();
        }
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(1);

        _audio_source.PlayOneShot(AttackSFX);

        GameObject projectile = Instantiate(Projectile, transform.position, new Quaternion());
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        float range = _player_location.position.x - transform.position.x;
        float angle = Mathf.Atan(Physics2D.gravity.y * rb.gravityScale * ProjectileHangTime * ProjectileHangTime / (2 * range));
        float velocity = range / (ProjectileHangTime * Mathf.Cos(angle));

        rb.velocity = new Vector2(velocity * Mathf.Cos(angle), Mathf.Abs(velocity * Mathf.Sin(angle)));

        _animator.SetBool("Attacking", false);
        yield return null;
    }

    public ParticleSystem GetParticles()
    {
        return Particles;
    }
}
