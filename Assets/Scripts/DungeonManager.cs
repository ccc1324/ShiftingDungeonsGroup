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

    private void Start()
    {
        _player_offset = 3.5f;
        _dungeon_size = 20;
        _spawn_size = 40;
        _level = 0;

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
        _room_spawn = Instantiate(
                    Spawn,
                    new Vector3(
                        Player.transform.position.x,
                        Player.transform.position.y + _player_offset,
                        0),
                    new Quaternion(0, 0, 0, 0),
                    Grid.transform);
        _room_left = Instantiate(
                    Filler[_level],
                    new Vector3(
                        Player.transform.position.x - _spawn_size/2 - _dungeon_size/2,
                        Player.transform.position.y + _player_offset,
                        0),
                    new Quaternion(0, 0, 0, 0),
                    Grid.transform);
        _room_current = Instantiate(
                    Dungeons[_level],
                    new Vector3(
                        Player.transform.position.x + _spawn_size / 2 + _dungeon_size / 2,
                        Player.transform.position.y + _player_offset,
                        0),
                    new Quaternion(0, 0, 0, 0),
                    Grid.transform);
        _wall = Instantiate(
                    Walls[_level].SpawnWallsRight,
                    new Vector3(
                        Player.transform.position.x,
                        Player.transform.position.y + _player_offset,
                        0),
                    new Quaternion(0, 0, 0, 0),
                    Grid.transform);
        _wall_another = Instantiate(
                    Walls[_level].StaticWallsLeft,
                    new Vector3(
                        _room_current.transform.position.x,
                        Player.transform.position.y + _player_offset,
                        0),
                    new Quaternion(0, 0, 0, 0),
                    Grid.transform);

        //Update Dungeon State
        DungeonState = "Spawn";
    }

    void Update()
    {
        switch (DungeonState)
        {
            case "Spawn":
                if (Player.transform.position.x > _room_current.transform.position.x)
                {
                    //Destroy previous rooms
                    Destroy(_room_spawn);
                    Destroy(_room_left);

                    //Instatiate new rooms
                    _room_left = Instantiate(
                        Dungeons[_level],
                        new Vector3(
                            _room_current.transform.position.x - _dungeon_size,
                            _room_current.transform.position.y,
                            0),
                        new Quaternion(0, 0, 0, 0),
                        Grid.transform);
                    _room_right = Instantiate(
                        Dungeons[_level],
                        new Vector3(
                            _room_current.transform.position.x + _dungeon_size,
                            _room_current.transform.position.y,
                            0),
                        new Quaternion(0, 0, 0, 0),
                        Grid.transform);

                    //Update Walls
                    Destroy(_wall);
                    Destroy(_wall_another);
                    _wall = Instantiate(
                        Walls[_level].ShiftingWalls,
                        new Vector3(
                            _room_current.transform.position.x,
                            _room_current.transform.position.y,
                            0),
                        new Quaternion(0, 0, 0, 0),
                        Grid.transform);
                    _wall_another = Instantiate(
                        Walls[_level].ShiftingWallsAnimated,
                        new Vector3(
                            _room_current.transform.position.x,
                            _room_current.transform.position.y,
                            0),
                        new Quaternion(0, 0, 0, 0),
                        Grid.transform);

                    DungeonState = "Shifting";
                }
                break;
            case "Shifting":
                if (Player.transform.position.x > _room_current.transform.position.x + 10)
                {
                    Destroy(_room_left);
                    _room_left = _room_current;
                    _room_current = _room_right;
                    _room_right = Instantiate(
                        Dungeons[_level],
                        new Vector3(
                            _room_current.transform.position.x + _dungeon_size,
                            _room_current.transform.position.y,
                            _room_current.transform.position.z),
                        new Quaternion(0, 0, 0, 0),
                        Grid.transform);
                }
                else if (Player.transform.position.x < _room_current.transform.position.x - 10)
                {
                    Destroy(_room_right);
                    _room_right = _room_current;
                    _room_current = _room_left;
                    _room_left = Instantiate(
                        Dungeons[_level],
                        new Vector3(
                            _room_current.transform.position.x - _dungeon_size,
                            _room_current.transform.position.y,
                            _room_current.transform.position.z),
                        new Quaternion(0, 0, 0, 0),
                        Grid.transform);
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

                        _room_boss = Instantiate(
                            BossRooms[_level],
                            new Vector3(
                                _room_current.transform.position.x + _dungeon_size,
                                _room_current.transform.position.y,
                                _room_current.transform.position.z),
                            new Quaternion(0, 0, 0, 0),
                            Grid.transform);

                        Destroy(_wall);
                        Destroy(_wall_another);
                        _wall = Instantiate(
                            Walls[_level].StaticWallsRight,
                            new Vector3(
                                _room_current.transform.position.x,
                                _room_current.transform.position.y,
                                0),
                            new Quaternion(0, 0, 0, 0),
                            Grid.transform);
                        _wall_another = Instantiate(
                            Walls[_level].StaticWallsLeft,
                            new Vector3(
                                _room_current.transform.position.x + _dungeon_size,
                                _room_current.transform.position.y,
                                0),
                            new Quaternion(0, 0, 0, 0),
                            Grid.transform);

                        DungeonState = "TransitionToBossA";
                        SpawnBoss = false;
                    }
                }
                break;

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

                    _wall = Instantiate(
                        Walls[_level].StaticWalls,
                        new Vector3(
                            _room_boss.transform.position.x,
                            _room_boss.transform.position.y,
                            0),
                        new Quaternion(0, 0, 0, 0),
                        Grid.transform);

                    DungeonState = "BossFight";
                }
                break;

            case "BossFight":
                if (BossDefeated)
                {
                    _room_spawn = Instantiate(
                        Spawn,
                        new Vector3(
                            _room_boss.transform.position.x + _dungeon_size / 2 + _spawn_size / 2,
                            _room_boss.transform.position.y,
                            0),
                        new Quaternion(0, 0, 0, 0),
                        Grid.transform);
                    _room_current = Instantiate(
                        Dungeons[_level],
                        new Vector3(
                            _room_boss.transform.position.x + _dungeon_size + _spawn_size,
                            _room_boss.transform.position.y,
                            0),
                        new Quaternion(0, 0, 0, 0),
                        Grid.transform);

                    Destroy(_wall);

                    _wall = Instantiate(
                        Walls[_level].SpawnWallsLeft,
                        new Vector3(
                            _room_spawn.transform.position.x,
                            _room_spawn.transform.position.y,
                            0),
                        new Quaternion(0, 0, 0, 0),
                        Grid.transform);
                    _wall_another = Instantiate(
                        Walls[_level].StaticWallsRight,
                        new Vector3(
                            _room_boss.transform.position.x,
                            _room_boss.transform.position.y,
                            0),
                        new Quaternion(0, 0, 0, 0),
                        Grid.transform);

                    DungeonState = "TransitionToSpawnA";
                    BossDefeated = false;
                }
                break;

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

                    _room_left = Instantiate(
                        Filler[_level],
                        new Vector3(
                            _room_spawn.transform.position.x - _spawn_size / 2 - _dungeon_size / 2,
                            _room_spawn.transform.position.y,
                            0),
                        new Quaternion(0, 0, 0, 0),
                        Grid.transform);

                    Destroy(_wall);
                    Destroy(_wall_another);

                    _wall = Instantiate(
                       Walls[_level].SpawnWallsRight,
                       new Vector3(
                           _room_spawn.transform.position.x,
                           _room_spawn.transform.position.y,
                           0),
                       new Quaternion(0, 0, 0, 0),
                       Grid.transform);
                    _wall_another = Instantiate(
                        Walls[_level].StaticWallsLeft,
                        new Vector3(
                            _room_current.transform.position.x,
                            _room_current.transform.position.y,
                            0),
                        new Quaternion(0, 0, 0, 0),
                        Grid.transform);

                    DungeonState = "Spawn";
                }
                break;
        }
    }

    //used to decrease lines of code
    private GameObject MyInstantiate(GameObject obj, float x, float y)
    {
        return Instantiate(obj, new Vector3(x, y, 0), new Quaternion(0, 0, 0, 0), Grid.transform);
    }
}
