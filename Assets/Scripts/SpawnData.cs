using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Contains a list of Stages: each Stage determines the mobs spawning in a room
 * Each Stage has a list of WavesData (and a spawn cooldown variable, default time between each wave)
 * Each WavesData has information on what waves, how many waves, and timings for waves for a room
 */

[CreateAssetMenu(fileName = "New SpawnData", menuName = "SpawnData")]
public class SpawnData : ScriptableObject
{
    public List<Stage> StageList; //The stages contained in a level, each Stage determines the mobs spawning in a room

    [System.Serializable]
    public struct Stage
    {
        [Header("Stage")] //For readability in inspector
        public float SpawnCooldown; //How long between spawning of each wave
        public List<WaveData> WaveList; //Waves for a stage
    }

    [System.Serializable]
    public struct WaveData
    {
        [Tooltip("Extra time between waves (positive numbers mean later waves)")]
        public float WaveBuffer; //extra time between waves
        public Wave Wave; //Wave object
    }
}

