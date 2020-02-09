using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Level")]
public class Level : ScriptableObject
{
    /*
     * Scriptable Object that contains all room prefabs for a level
     * Also gives instructions on number and order of stages to instantiate before boss room
     */

    //Rooms
    public GameObject Spawn;
    public GameObject Filler; //Filled room
    public List<GameObject> Stages;
    public GameObject ShiftingRoom; //Room used when in shifting state
    public GameObject BossRoom;
    public WallSet Walls;

    [System.Serializable]
    public struct WallSet
    {
        public GameObject SpawnWallsLeft;
        public GameObject SpawnWallsRight;
        public GameObject StaticWalls;
        public GameObject StaticWallsLeft;
        public GameObject StaticWallsRight;
    }
}
