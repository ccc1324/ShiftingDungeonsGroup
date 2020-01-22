using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrainingDummy : MonoBehaviour, IEnemy
{
    public float Health = 9999;
    public float DisplayTime;
    public float StartBuffer;
    public TextMeshPro DamageText;
    public ParticleSystem ParticleEffects;
    public AudioClip HitSFX;

    private float _total_damage;
    private float _last_hit_time;
    private float _start_time;

    void Update()
    {
        if (_last_hit_time + DisplayTime < Time.time)
        {
            _total_damage = 0;
            DamageText.text = "0";
        }
    }

    public void OnHit(int damage, bool stun, Vector3 particlePosition)
    {
        _total_damage += damage;
        if (_last_hit_time + DisplayTime < Time.time)
            _start_time = Time.time - StartBuffer;
        _last_hit_time = Time.time;

        float time = Time.time - _start_time;
        time = time <= StartBuffer ? 1 : time;
        DamageText.text = Mathf.Round(_total_damage / 10 / time).ToString();

        Instantiate(ParticleEffects, particlePosition, new Quaternion());
        GetComponent<AudioSource>().volume = stun ? 0.5f : 0.2f;
        GetComponent<AudioSource>().PlayOneShot(HitSFX);
    }
}
