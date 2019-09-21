using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRock : MonoBehaviour, IEnemy
{
    public ParticleSystem ParticleEffects;
    public ParticleSystem DestroyParticleEffects;
    public int Health;
    public Sprite TransformationA;
    public int TransformationHealthA;
    public Sprite TransformationB;
    public int TransformationHealthB;

    public void OnHit(int damage, bool stun)
    {
        Health -= damage;
        
        if (Health <= 0)
        {
            Instantiate(DestroyParticleEffects, transform.position, new Quaternion());
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
}
