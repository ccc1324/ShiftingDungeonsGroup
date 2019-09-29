using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Contains information on how many waves to spawn each stage of a level
 * and how to spawn a the mobs in each wave.
 */

[CreateAssetMenu(fileName = "New WavesData", menuName = "WavesData")]
public class Waves : ScriptableObject
{
    public List<Stage> StageList; //The stages contained in a level

    [System.Serializable]
    public struct Stage
    {
        public float SpawnCooldown; //How long between spawning of each wave
        public List<Wave> WaveList; //Waves for a stage
    }

    [System.Serializable]
    public struct Wave
    {
        public float WaveBuffer; //extra time between waves
        public List<Mob> Mobs; //list of mobs
    }

    [System.Serializable]
    public struct Mob
    {
        public GameObject Prefab;
        public Vector2 Location;
        public float EjectSpeed; //if EjectSpeed <= 0, spawn on ceiling
    }
}
