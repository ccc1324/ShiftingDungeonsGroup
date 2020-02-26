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
    public AudioClip HitSFX;
    public AudioClip DestroySFX;
    public float DestroySFXVolume;

    public void OnHit(int damage, bool stun, Vector3 particlePosition)
    {
        Health -= damage;
        
        if (Health <= 0)
        {
            ParticleSystem particles = Instantiate(DestroyParticleEffects, transform.position, new Quaternion());
            AudioSource audio_source = particles.gameObject.AddComponent<AudioSource>();
            audio_source.PlayOneShot(DestroySFX);
            audio_source.volume = DestroySFXVolume * OptionsManager.GetSoundVolume();
            Destroy(gameObject);
            return;
        }
        else
        {
            Instantiate(ParticleEffects, particlePosition, new Quaternion());
            GetComponent<AudioSource>().volume = (stun ? 0.5f : 0.2f) * OptionsManager.GetSoundVolume();
            GetComponent<AudioSource>().PlayOneShot(HitSFX);
        }

        if (Health <= TransformationHealthB)
            GetComponent<SpriteRenderer>().sprite = TransformationB;
        else if (Health  <= TransformationHealthA)
            GetComponent<SpriteRenderer>().sprite = TransformationA;
    }
}
