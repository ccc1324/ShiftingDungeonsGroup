using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    void OnHit(int damage, bool stun);

    ParticleSystem GetParticles();
};

