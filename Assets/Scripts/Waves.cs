using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Waves", menuName = "Waves")]
public class Waves : ScriptableObject
{
    public float SpawnCooldown;
    public List<Wave> WaveArray;

    [System.Serializable]
    public struct Wave
    {
        public float WaveBuffer;
        public List<Mob> Mobs;
    }

    [System.Serializable]
    public struct Mob
    {
        public GameObject Prefab;
        public Vector2 Location;
        public float EjectSpeed; //if EjectSpeed <= 0, spawn on ceiling
    }
}
