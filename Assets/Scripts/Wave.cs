using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Contain information about what mobs to spawn, where to spawn them, and how to spawn them for a wave
 */
[CreateAssetMenu(fileName = "New WaveData", menuName = "WaveData")]
public class Wave : ScriptableObject
{
    public List<Mob> MobsList; //list of mobs

    [System.Serializable]
    public struct Mob
    {
        public GameObject Prefab;

        [Tooltip("The leftmost x position the mob will spawn relative to the center of the room")]
        [Range(-7, 7)] //given room sizes, -7 <-> 7 will spawn a mob inside the room
        public float LeftmostPosition;

        [Tooltip("The rightmost x position the mob will spawn relative to the center of the room")]
        [Range(-7, 7)] //given room sizes, -7 <-> 7 will spawn a mob inside the room
        public float RightmostPosition;

        [Tooltip("Speed mob is ejected from the ground/ceiling (if <=0, spawn on ceiling)")]
        public float EjectSpeed; //if EjectSpeed <= 0, spawn on ceiling
    }
}
