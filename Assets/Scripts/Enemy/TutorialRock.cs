using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRock : MonoBehaviour, IEnemy
{
    public ParticleSystem ParticleEffects;
    public ParticleSystem DestroyParticleEffects;
    public float Health;
    public Sprite TransformationA;
    public int TransformationHealthA;
    public Sprite TransformationB;
    public int TransformationHealthB;
    public AudioClip DestroySFX;
    public float DestroySFXVolume;

    public void OnHit(int damage, bool stun)
    {
        Health -= damage;
        
        if (Health <= 0)
        {
            ParticleSystem particles = Instantiate(DestroyParticleEffects, transform.position, new Quaternion());
            AudioSource audio_source = particles.gameObject.AddComponent<AudioSource>();
            audio_source.PlayOneShot(DestroySFX);
            audio_source.volume = DestroySFXVolume;
            Destroy(gameObject);
            return;
        }

        if (Health <= TransformationHealthB)
            GetComponent<SpriteRenderer>().sprite = TransformationB;
        else if (Health  <= TransformationHealthA)
            GetComponent<SpriteRenderer>().sprite = TransformationA;
    }

    public ParticleSystem GetParticles()
    {
        return ParticleEffects;
    }

    public float GetHealth()
    {
        return Health;
    }
}
