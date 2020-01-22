using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    void OnHit(int Damage, bool Stun, Vector3 ParticleSpawnPosition);
};

