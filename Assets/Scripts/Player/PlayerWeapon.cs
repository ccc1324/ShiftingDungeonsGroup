using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public bool EnableWeaponHitbox;
    public int WeaponDamage;
    public bool SetToStun;
    public GameObject ParticleSpawnPoint;
    public AudioClip HitSFX;
    public SFXData[] SFXDatas;
    public int SFXID; //to be used by animator when specifying sound effects
    /* 0 = Dagger
     * 1 = Spear/Staff 1&2
     * 2 = Sword
     * 3 = Staff 3
     * 4 = Axe
     * 5 = Hammer
     */

    private bool _hitbox_enabled;
    private Collider2D _weapon_hitbox;
    private AudioSource _audio_source;

    [System.Serializable]
    public struct SFXData
    {
        public AudioClip clip;
        public float volume;
    }

    private void Start()
    {
        _audio_source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "Enemy") && EnableWeaponHitbox)
        {
            if (collision.GetComponent<IEnemy>().GetHealth() <= 0)
                return;

            collision.GetComponent<IEnemy>().OnHit(WeaponDamage, SetToStun);

            Instantiate(collision.GetComponent<IEnemy>().GetParticles(), ParticleSpawnPoint.transform.position, new Quaternion());

            //if (_audio_source.isPlaying) //prevent mutiple hit sounds playing
                //return;
            _audio_source.PlayOneShot(HitSFX);
            _audio_source.volume = SetToStun ? 0.5f : 0.70f;
        }
    }

    private void Update()
    {
        if (EnableWeaponHitbox && !_hitbox_enabled)
        {
            _weapon_hitbox = gameObject.AddComponent<PolygonCollider2D>();
            _weapon_hitbox.isTrigger = true;

            _audio_source.volume = SFXDatas[SFXID].volume;
            _audio_source.PlayOneShot(SFXDatas[SFXID].clip);
        }
        else if (!EnableWeaponHitbox && _hitbox_enabled)
        {
            Destroy(_weapon_hitbox);
        }
        _hitbox_enabled = EnableWeaponHitbox;
    }

}
