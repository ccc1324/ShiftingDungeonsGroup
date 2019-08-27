using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    //prefabs to instantiate
    public GameObject Spawn;
    public List<GameObject> Filler;
    public List<GameObject> Dungeons;
    public List<GameObject> BossRooms;
    public List<WallSet> Walls;

    [System.Serializable]
    public struct WallSet //Struct that contains references to the set of walls necessary for a dungeon level
    {
        public GameObject ShiftingWalls;
        public GameObject ShiftingWallsAnimated;
        public GameObject SpawnWallsLeft;
        public GameObject SpawnWallsRight;
        public GameObject StaticWalls;
        public GameObject StaticWallsLeft;
        public GameObject StaticWallsRight;
    }

    //stats of various rooms
    private float _player_offset; //-player's y coordinate
    private int _dungeon_size;
    private int _level;
    private int _spawn_size;

    //references to rooms and wall
    private GameObject _room_left; //also used to store filler room (dirt)
    private GameObject _room_current;
    private GameObject _room_right;
    private GameObject _room_spawn;
    private GameObject _room_boss;
    private GameObject _wall;
    private GameObject _wall_another;

    //Dungeon States
    public string DungeonState;
    public bool SpawnBoss;
    public bool BossDefeated;

    //Private variables
    private GameObject Player;
    private GameObject Grid;
    private GameObject CameraObject;
    private MonsterSpawner MonsterSpawner;

    private void Start()
    {
        _player_offset = 3.5f;
        _dungeon_size = 18;
        _spawn_size = 40;
        _level = 0;

        MonsterSpawner = FindObjectOfType<MonsterSpawner>().GetComponent<MonsterSpawner>();
        if (MonsterSpawner == null)
            Debug.Log("DungeonManager could not find MonsterSpawner");
        Player = FindObjectOfType<PlayerMovement>().gameObject;
        if (Player == null)
            Debug.Log("DungeonManager could not find Player");
        Grid = FindObjectOfType<Grid>().gameObject;
        if (Grid == null)
            Debug.Log("DungeonManager could not find Grid");
        CameraObject = FindObjectOfType<Camera>().gameObject;
        if (CameraObject == null)
            Debug.Log("DungeonManager could not find Camera");
        CameraObject.GetComponent<CameraMovement>().CameraState = "Follow";

        //Instantiate Spawn for the first time
        _room_spawn = MyInstantiate(
                    Spawn,
                    Player.transform.position.x,
                    Player.transform.position.y + _player_offset);
        _room_left = MyInstantiate(
                    Filler[_level],
                    Player.transform.position.x - _spawn_size/2 - _dungeon_size/2,
                    Player.transform.position.y + _player_offset);
        _room_current = MyInstantiate(
                    Dungeons[_level],
                    Player.transform.position.x + _spawn_size / 2 + _dungeon_size / 2,
                    Player.transform.position.y + _player_offset);
        _wall = MyInstantiate(
                    Walls[_level].SpawnWallsRight,
                    Player.transform.position.x,
                    Player.transform.position.y + _player_offset);
        _wall_another = MyInstantiate(
                    Walls[_level].StaticWallsLeft,
                    _room_current.transform.position.x,
                    Player.transform.position.y + _player_offset);

        //Update Dungeon State
        DungeonState = "Spawn";
    }

    void Update()
    {
        switch (DungeonState)
        {
            #region Spawn
            case "Spawn":
                if (Player.transform.position.x > _room_current.transform.position.x)
                {
                    //Destroy previous rooms
                    Destroy(_room_spawn);
                    Destroy(_room_left);

                    //Instatiate new rooms
                    _room_left = MyInstantiate(
                        Dungeons[_level],
                        _room_current.transform.position.x - _dungeon_size,
                        _room_current.transform.position.y);
                    _room_right = MyInstantiate(
                        Dungeons[_level],
                        _room_current.transform.position.x + _dungeon_size,
                        _room_current.transform.position.y);

                    //Update Walls
                    Destroy(_wall);
                    Destroy(_wall_another);
                    _wall = MyInstantiate(
                        Walls[_level].ShiftingWalls,
                        _room_current.transform.position.x,
                        _room_current.transform.position.y);
                    _wall_another = MyInstantiate(
                        Walls[_level].ShiftingWallsAnimated,
                        _room_current.transform.position.x,
                        _room_current.transform.position.y);

                    DungeonState = "Shifting";

                    MonsterSpawner.SpawnMobs(_level);
                }
                break;
            #endregion
            #region Shifting
            case "Shifting":
                if (Player.transform.position.x > _room_current.transform.position.x + 10)
                {
                    Destroy(_room_left);
                    _room_left = _room_current;
                    _room_current = _room_right;
                    _room_right = MyInstantiate(
                        Dungeons[_level],
                        _room_current.transform.position.x + _dungeon_size,
                        _room_current.transform.position.y);
                }
                else if (Player.transform.position.x < _room_current.transform.position.x - 10)
                {
                    Destroy(_room_right);
                    _room_right = _room_current;
                    _room_current = _room_left;
                    _room_left = MyInstantiate(
                        Dungeons[_level],
                        _room_current.transform.position.x - _dungeon_size,
                        _room_current.transform.position.y);
                }

                //When conditions for boss fight are met, Wait for certain conditions then transition to boss fight state
                if (SpawnBoss)
                {
                    if (Player.transform.position.x <= _room_current.transform.position.x + 0.05 &&
                        Player.transform.position.x >= _room_current.transform.position.x - 0.05)
                    {
                        //Lock Camera
                        CameraObject.GetComponent<CameraMovement>().CameraState = "Locked";
                        CameraObject.transform.position = new Vector3(_room_current.transform.position.x, CameraObject.transform.position.y, -10);

                        Destroy(_room_left);
                        Destroy(_room_right);

                        _room_boss = MyInstantiate(
                            BossRooms[_level],
                            _room_current.transform.position.x + _dungeon_size,
                            _room_current.transform.position.y);

                        Destroy(_wall);
                        Destroy(_wall_another);
                        _wall = MyInstantiate(
                            Walls[_level].StaticWallsRight,
                            _room_current.transform.position.x,
                            _room_current.transform.position.y);
                        _wall_another = MyInstantiate(
                            Walls[_level].StaticWallsLeft,
                            _room_current.transform.position.x + _dungeon_size,
                            _room_current.transform.position.y);

                        DungeonState = "TransitionToBossA";
                        SpawnBoss = false;
                    }
                }
                break;
            #endregion
            #region TransitionToBoss
            case "TransitionToBossA": //Player is in current room
                //Move Camera to boss room
                if (Player.transform.position.x > _room_current.transform.position.x + _dungeon_size / 2)
                {
                    CameraObject.GetComponent<CameraMovement>().ShiftRight(_dungeon_size);
                    DungeonState = "TransitionToBossB";
                }
                break;

            case "TransitionToBossB": //Player is in boss room
                //Move Camera to curr room
                if (Player.transform.position.x < _room_boss.transform.position.x - _dungeon_size / 2)
                {
                    CameraObject.GetComponent<CameraMovement>().ShiftLeft(_dungeon_size);
                    DungeonState = "TransitionToBossA";
                }
                //Transition to boss fight
                if (Player.transform.position.x > _room_boss.transform.position.x - _dungeon_size / 4)
                {
                    Destroy(_room_current);

                    Destroy(_wall);
                    Destroy(_wall_another);

                    _wall = MyInstantiate(
                        Walls[_level].StaticWalls,
                        _room_boss.transform.position.x,
                        _room_boss.transform.position.y);

                    DungeonState = "BossFight";
                }
                break;
            #endregion
            #region BossFight
            case "BossFight":
                if (BossDefeated)
                {
                    _room_spawn = MyInstantiate(
                        Spawn,
                        _room_boss.transform.position.x + _dungeon_size / 2 + _spawn_size / 2,
                        _room_boss.transform.position.y);
                    _room_current = MyInstantiate(
                        Dungeons[_level],
                        _room_boss.transform.position.x + _dungeon_size + _spawn_size,
                        _room_boss.transform.position.y);

                    Destroy(_wall);

                    _wall = MyInstantiate(
                        Walls[_level].SpawnWallsLeft,
                        _room_spawn.transform.position.x,
                        _room_spawn.transform.position.y);
                    _wall_another = MyInstantiate(
                        Walls[_level].StaticWallsRight,
                        _room_boss.transform.position.x,
                        _room_boss.transform.position.y);

                    DungeonState = "TransitionToSpawnA";
                    BossDefeated = false;
                }
                break;
            #endregion
            #region TransitionToSpawn
            case "TransitionToSpawnA": //Player is in boss room
                //Move Camera to spawn room
                if (Player.transform.position.x > _room_boss.transform.position.x + _dungeon_size / 2)
                {
                    CameraObject.GetComponent<CameraMovement>().ShiftRight(_dungeon_size);
                    DungeonState = "TransitionToSpawnB";
                }
                break;

            case "TransitionToSpawnB": //Player is in spawn room
                //Move Camera to boss room
                if (Player.transform.position.x < _room_spawn.transform.position.x - _spawn_size / 2)
                {
                    CameraObject.GetComponent<CameraMovement>().ShiftLeft(_dungeon_size);
                    DungeonState = "TransitionToSpawnA";
                }

                //Transition to Game State Spawn
                if (Player.transform.position.x > _room_spawn.transform.position.x - _spawn_size / 2 
                        + CameraObject.GetComponent<Camera>().orthographicSize * 2)
                {
                    CameraObject.GetComponent<CameraMovement>().CameraState = "Follow";

                    Destroy(_room_boss);

                    _room_left = MyInstantiate(
                        Filler[_level],
                        _room_spawn.transform.position.x - _spawn_size / 2 - _dungeon_size / 2,
                        _room_spawn.transform.position.y);

                    Destroy(_wall);
                    Destroy(_wall_another);

                    _wall = MyInstantiate(
                        Walls[_level].SpawnWallsRight,
                        _room_spawn.transform.position.x,
                        _room_spawn.transform.position.y);
                    _wall_another = MyInstantiate(
                        Walls[_level].StaticWallsLeft, 
                        _room_current.transform.position.x, 
                        _room_current.transform.position.y);

                    DungeonState = "Spawn";
                }
                break;
                #endregion
            
        }
    }

    //used to decrease lines of code
    private GameObject MyInstantiate(GameObject obj, float x, float y)
    {
        return Instantiate(obj, new Vector3(x, y, 0), new Quaternion(0, 0, 0, 0), Grid.transform);
    }
}
