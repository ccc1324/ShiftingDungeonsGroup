using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    //Additional objects meant too be in spawn
    public GameObject PostTutorialObjects;

    //levels
    public List<Level> Levels;
    public int MaxLevel; //Level from save data
    public int Level;

    //stats of various rooms
    private int _dungeon_size;
    private int _stage;
    private int _spawn_size;

    //references to rooms and wall
    private GameObject _room_left; //also used to store filler room (dirt)
    private GameObject _room_current;
    private GameObject _room_right;
    private GameObject _room_spawn;
    private GameObject _room_boss;
    public GameObject RoomStage; //should always hold a reference to current stage (if it exists)
    private GameObject _wall;
    private GameObject _wall_another;

    //Dungeon States
    public string DungeonState;
    public bool MobsCleared;
    public bool SpawnBoss;
    public bool BossDefeated;

    //Sound Effects
    public AudioClip DestroySFX;
    public float DestroySFXVolume;
    public AudioClip LockSFX;
    public float LockSFXVolume;
    public AudioClip UnlockSFX;
    public float UnlockSFXVolume;

    //Private variables
    private GameObject Player;
    private GameObject Grid;
    private GameObject CameraObject;
    private MonsterSpawner MonsterSpawner;
    private MusicManager MusicManager;

    private const float ROOM_CENTER_BUFFER = 0.15f; //

    private void Start()
    {
        _dungeon_size = 18;
        _spawn_size = 40;
        Level = 0;

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
        MusicManager = FindObjectOfType<MusicManager>();
        if (MusicManager == null)
            Debug.Log("DungeonManager could not find MusicManager");

        //Instantiate Spawn for the first time
        _room_spawn = MyInstantiate(
                    Levels[Level].Spawn,
                    Player.transform.position.x, 0);
        _room_left = MyInstantiate(
                    Levels[Level].Filler,
                    Player.transform.position.x - _spawn_size / 2 - _dungeon_size / 2, 0);
        RoomStage = _room_current = MyInstantiate(
                    Levels[Level].Stages[_stage],
                    Player.transform.position.x + _spawn_size / 2 + _dungeon_size / 2, 0);

        _wall = MyInstantiate(
                    Levels[Level].Walls.SpawnWallsRight,
                    Player.transform.position.x, 0);
        _wall_another = MyInstantiate(
                    Levels[Level].Walls.StaticWallsLeft,
                    _room_current.transform.position.x, 0);

        DungeonState = "Spawn";
    }

    void Update()
    {
        switch (DungeonState)
        {
            #region Respawn
            case "Respawn":
                //Destory all existing non-layer objects
                Destroy(_room_left);
                Destroy(_room_current);
                Destroy(_room_right);
                Destroy(_room_spawn);
                Destroy(_room_boss);
                Destroy(_wall);
                Destroy(_wall_another);
                MonsterSpawner.DestoryAllMobs();
                foreach (GameItem item in FindObjectsOfType<GameItem>())
                    Destroy(item.gameObject);

                //Reset Stage
                _stage = 0;

                //Instantiate Spawn for the first time
                _room_spawn = MyInstantiate(
                            Levels[Level].Spawn,
                            Player.transform.position.x, 0);
                PostTutorialObjects.SetActive(true);
                PostTutorialObjects.transform.position = _room_spawn.transform.position;
                _room_left = MyInstantiate(
                            Levels[Level].Filler,
                            Player.transform.position.x - _spawn_size / 2 - _dungeon_size / 2, 0);
                RoomStage = _room_current = MyInstantiate(
                            Levels[Level].Stages[_stage],
                            Player.transform.position.x + _spawn_size / 2 + _dungeon_size / 2, 0);

                _wall = MyInstantiate(
                            Levels[Level].Walls.SpawnWallsRight,
                            Player.transform.position.x, 0);
                _wall_another = MyInstantiate(
                            Levels[Level].Walls.StaticWallsLeft,
                            _room_current.transform.position.x, 0);

                //Update Dungeon State
                DungeonState = "Spawn";

                //Music
                StartCoroutine(MusicManager.FadeInMusic(2, MusicManager.SpawnMusic, MusicManager.SpawnMusicVolume));

                break;
            #endregion
            #region Spawn
            case "Spawn":
                if (Player.transform.position.x > _room_current.transform.position.x)
                {
                    //Lock Camera
                    CameraObject.GetComponent<CameraMovement>().CameraState = "Locked";
                    CameraObject.transform.position = new Vector3(_room_current.transform.position.x, CameraObject.transform.position.y, -10);

                    //Destroy previous rooms
                    Destroy(_room_spawn);
                    PostTutorialObjects.SetActive(false);
                    Destroy(_room_left);

                    //Instatiate new rooms
                    _room_left = MyInstantiate(
                        Levels[Level].ShiftingRoom,
                        _room_current.transform.position.x - _dungeon_size,
                        _room_current.transform.position.y);
                    _room_right = MyInstantiate(
                        Levels[Level].ShiftingRoom,
                        _room_current.transform.position.x + _dungeon_size,
                        _room_current.transform.position.y);

                    //Update Walls
                    Destroy(_wall);
                    Destroy(_wall_another);
                    _wall = MyInstantiate(
                        Levels[Level].Walls.StaticWalls,
                        _room_current.transform.position.x,
                        _room_current.transform.position.y);

                    //Play Sounds
                    _room_current.GetComponent<AudioSource>().volume = LockSFXVolume;
                    _room_current.GetComponent<AudioSource>().PlayOneShot(LockSFX);

                    DungeonState = "Stage";

                    MonsterSpawner.SpawnMobs(Level, _stage);

                    //Music
                    StartCoroutine(MusicManager.FadeOutMusic(2));
                    StartCoroutine(MusicManager.PlayMusicDelayed(2.5f, 
                        MusicManager.DungeonMusic[Level].StageMusic, 
                        MusicManager.DungeonMusic[Level].StageVolume));
                }
                break;
            #endregion
            #region Stage
            case "Stage":
                if (MobsCleared)
                {
                        //Play Sounds
                        _room_current.GetComponent<AudioSource>().volume = UnlockSFXVolume;
                        _room_current.GetComponent<AudioSource>().PlayOneShot(UnlockSFX);

                        //Update Walls
                        Destroy(_wall);

                        CameraObject.GetComponent<CameraMovement>().ShiftToPlayer();
                        MobsCleared = false;
                        _stage++;
                        DungeonState = "TransitionToShifting";
                }
                break;
            #endregion
            #region TransitionToShifting
            case "TransitionToShifting":
                //Loop empty rooms
                if (Player.transform.position.x > _room_current.transform.position.x + 10)
                {
                    Destroy(_room_left);
                    _room_left = _room_current;
                    _room_current = _room_right;
                    if (!SpawnBoss)
                    {
                        RoomStage = _room_right = MyInstantiate(
                            Levels[Level].Stages[_stage],
                            _room_current.transform.position.x + _dungeon_size,
                            _room_current.transform.position.y);
                    }
                    else if (SpawnBoss)
                    {
                        _room_right = MyInstantiate(
                            Levels[Level].ShiftingRoom,
                            _room_current.transform.position.x + _dungeon_size,
                            _room_current.transform.position.y);
                    }
                    DungeonState = "Shifting";

                    _room_left.GetComponent<AudioSource>().volume = DestroySFXVolume;
                    _room_left.GetComponent<AudioSource>().PlayOneShot(DestroySFX);
                }
                else if (Player.transform.position.x < _room_current.transform.position.x - 10)
                {
                    Destroy(_room_right);
                    _room_right = _room_current;
                    _room_current = _room_left;
                    if (!SpawnBoss)
                    {
                        RoomStage = _room_left = MyInstantiate(
                            Levels[Level].Stages[_stage],
                            _room_current.transform.position.x - _dungeon_size,
                            _room_current.transform.position.y);
                    }
                    else if (SpawnBoss)
                    {
                        _room_left = MyInstantiate(
                            Levels[Level].ShiftingRoom,
                            _room_current.transform.position.x - _dungeon_size,
                            _room_current.transform.position.y);
                    }
                    DungeonState = "Shifting";

                    _room_right.GetComponent<AudioSource>().volume = DestroySFXVolume;
                    _room_right.GetComponent<AudioSource>().PlayOneShot(DestroySFX);
                }
                break;
            #endregion
            #region Shifting
            case "Shifting":
                //Continue looping rooms (room depends on whether or not to spawn boss)
                if (Player.transform.position.x > _room_current.transform.position.x + 10)
                {
                    if (_room_left == RoomStage)
                        RoomStage = null; //Destroy won't change RoomStage to null before check
                    Destroy(_room_left);
                    _room_left = _room_current;
                    _room_current = _room_right;
                    if (!SpawnBoss)
                    {
                        _room_right = MyInstantiate(
                            Levels[Level].Stages[_stage],
                            _room_current.transform.position.x + _dungeon_size,
                            _room_current.transform.position.y);
                        if (RoomStage == null)
                            RoomStage = _room_right;
                    }
                    else
                        _room_right = MyInstantiate(
                            Levels[Level].ShiftingRoom,
                            _room_current.transform.position.x + _dungeon_size,
                            _room_current.transform.position.y);

                    _room_left.GetComponent<AudioSource>().volume = DestroySFXVolume;
                    _room_left.GetComponent<AudioSource>().PlayOneShot(DestroySFX);
                }
                else if (Player.transform.position.x < _room_current.transform.position.x - 10)
                {
                    if (_room_right == RoomStage)
                        RoomStage = null; //Destroy won't change RoomStage to null before check
                    Destroy(_room_right);
                    _room_right = _room_current;
                    _room_current = _room_left;
                    if (!SpawnBoss)
                    {
                        _room_left = MyInstantiate(
                            Levels[Level].Stages[_stage],
                            _room_current.transform.position.x - _dungeon_size,
                            _room_current.transform.position.y);
                        if (RoomStage == null)
                            RoomStage = _room_left;
                    }
                    else
                        _room_left = MyInstantiate(
                            Levels[Level].ShiftingRoom,
                            _room_current.transform.position.x - _dungeon_size,
                            _room_current.transform.position.y);

                    _room_right.GetComponent<AudioSource>().volume = DestroySFXVolume;
                    _room_right.GetComponent<AudioSource>().PlayOneShot(DestroySFX);
                }

                //If not spawning boss, wait for certain conditions, then transition back to stage (spawn mobs)
                if (!SpawnBoss)
                {
                    if (Player.transform.position.x <= _room_current.transform.position.x + ROOM_CENTER_BUFFER &&
                        Player.transform.position.x >= _room_current.transform.position.x - ROOM_CENTER_BUFFER &&
                        _room_current == RoomStage)
                    {
                        //Lock Camera
                        CameraObject.GetComponent<CameraMovement>().CameraState = "Locked";
                        CameraObject.transform.position = new Vector3(_room_current.transform.position.x, CameraObject.transform.position.y, -10);

                        //Destroy previous rooms
                        Destroy(_room_left);
                        Destroy(_room_right);

                        //Instatiate new rooms
                        _room_left = MyInstantiate(
                            Levels[Level].ShiftingRoom,
                            _room_current.transform.position.x - _dungeon_size,
                            _room_current.transform.position.y);
                        _room_right = MyInstantiate(
                            Levels[Level].ShiftingRoom,
                            _room_current.transform.position.x + _dungeon_size,
                            _room_current.transform.position.y);

                        //Update Walls
                        _wall = MyInstantiate(
                            Levels[Level].Walls.StaticWalls,
                            _room_current.transform.position.x,
                            _room_current.transform.position.y);

                        //Play Sounds
                        _room_current.GetComponent<AudioSource>().volume = LockSFXVolume;
                        _room_current.GetComponent<AudioSource>().PlayOneShot(LockSFX);

                        DungeonState = "Stage";

                        MonsterSpawner.SpawnMobs(Level, _stage);
                        break;
                    }
                }

                //When conditions for boss fight are met, Wait for certain conditions then transition to boss fight state
                else if (SpawnBoss && Levels[Level].BossRoom != null)
                {
                    if (Player.transform.position.x <= _room_current.transform.position.x + ROOM_CENTER_BUFFER &&
                        Player.transform.position.x >= _room_current.transform.position.x - ROOM_CENTER_BUFFER &&
                        RoomStage == null)
                    {
                        StartCoroutine(OpenRight(2));

                        //Lock Camera
                        CameraObject.GetComponent<CameraMovement>().CameraState = "Locked";
                        CameraObject.transform.position = new Vector3(_room_current.transform.position.x, CameraObject.transform.position.y, -10);

                        //Update Rooms
                        Destroy(_room_left);
                        Destroy(_room_right);

                        _room_boss = MyInstantiate(
                            Levels[Level].BossRoom,
                            _room_current.transform.position.x + _dungeon_size,
                            _room_current.transform.position.y);

                        //Updates Walls
                        Destroy(_wall);
                        Destroy(_wall_another);
                        _wall = MyInstantiate(
                            Levels[Level].Walls.StaticWalls,
                            _room_current.transform.position.x,
                            _room_current.transform.position.y);
                        _wall_another = MyInstantiate(
                            Levels[Level].Walls.StaticWallsLeft,
                            _room_current.transform.position.x + _dungeon_size,
                            _room_current.transform.position.y);

                        //Play Sounds
                        _room_current.GetComponent<AudioSource>().volume = LockSFXVolume;
                        _room_current.GetComponent<AudioSource>().PlayOneShot(LockSFX);

                        DungeonState = "TransitionToBossA";
                        SpawnBoss = false;
                        _stage = 0;

                        //Music
                        StartCoroutine(MusicManager.FadeOutMusic(2));
                    }
                }

                //If there are no more stages but there isn't a boss, get ready to transition to spawn
                else
                {
                    if (Player.transform.position.x <= _room_current.transform.position.x + ROOM_CENTER_BUFFER &&
                        Player.transform.position.x >= _room_current.transform.position.x - ROOM_CENTER_BUFFER &&
                        RoomStage == null)
                    {
                        StartCoroutine(OpenBossRoom(2));

                        //Lock Camera
                        CameraObject.GetComponent<CameraMovement>().CameraState = "Locked";
                        CameraObject.transform.position = new Vector3(_room_current.transform.position.x, CameraObject.transform.position.y, -10);

                        //Update Rooms
                        Destroy(_room_left);
                        Destroy(_room_right);

                        _room_boss = _room_current;
                        _room_current = null;
                        _room_spawn = MyInstantiate(
                        Levels[Level].Spawn,
                        _room_boss.transform.position.x + _dungeon_size / 2 + _spawn_size / 2,
                        _room_boss.transform.position.y);
                        PostTutorialObjects.SetActive(true);
                        PostTutorialObjects.transform.position = _room_spawn.transform.position;

                        //Updates Walls
                        Destroy(_wall);
                        Destroy(_wall_another);
                        _wall = MyInstantiate(
                            Levels[Level].Walls.StaticWalls,
                            _room_boss.transform.position.x,
                            _room_boss.transform.position.y);

                        //Play Sounds
                        _room_boss.GetComponent<AudioSource>().volume = LockSFXVolume;
                        _room_boss.GetComponent<AudioSource>().PlayOneShot(LockSFX);

                        DungeonState = "TransitionToSpawnA";
                        SpawnBoss = false;
                        _stage = 0;

                        //Music
                        StartCoroutine(MusicManager.FadeOutMusic(2));
                    }
                }
                break;
            #endregion
            #region TransitionToBoss
            case "TransitionToBossA": //Player is in current room
                //Move Camera to boss room
                if (Player.transform.position.x > _room_current.transform.position.x + _dungeon_size / 2)
                {
                    StartCoroutine(FreezePlayer(1));
                    CameraObject.GetComponent<CameraMovement>().ShiftRight(_dungeon_size);
                    DungeonState = "TransitionToBossB";
                }
                break;

            case "TransitionToBossB": //Player is in boss room
                //Move Camera to curr room
                if (Player.transform.position.x < _room_boss.transform.position.x - _dungeon_size / 2)
                {
                    StartCoroutine(FreezePlayer(1));
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
                        Levels[Level].Walls.StaticWalls,
                        _room_boss.transform.position.x,
                        _room_boss.transform.position.y);

                    _room_boss.GetComponent<AudioSource>().volume = LockSFXVolume;
                    _room_boss.GetComponent<AudioSource>().PlayOneShot(LockSFX);

                    DungeonState = "BossFight";

                    //Music
                    MusicManager.PlayMusic(MusicManager.DungeonMusic[Level].BossMusic, MusicManager.DungeonMusic[Level].BossVolume);
                }
                break;
            #endregion
            #region BossFight
            case "BossFight":
                if (BossDefeated)
                {
                    _room_spawn = MyInstantiate(
                        Levels[Level].Spawn,
                        _room_boss.transform.position.x + _dungeon_size / 2 + _spawn_size / 2,
                        _room_boss.transform.position.y);
                    PostTutorialObjects.SetActive(true);
                    PostTutorialObjects.transform.position = _room_spawn.transform.position;

                    StartCoroutine(OpenBossRoom(2));

                    DungeonState = "TransitionToSpawnA";
                    BossDefeated = false;

                    StartCoroutine(MusicManager.FadeOutMusic(2));
                }
                break;
            #endregion
            #region TransitionToSpawn
            case "TransitionToSpawnA": //Player is in boss room
                //Move Camera to spawn room
                if (Player.transform.position.x > _room_boss.transform.position.x + _dungeon_size / 2)
                {
                    StartCoroutine(FreezePlayer(1));
                    CameraObject.GetComponent<CameraMovement>().ShiftRight(_dungeon_size);
                    DungeonState = "TransitionToSpawnB";
                }
                break;

            case "TransitionToSpawnB": //Player is in spawn room
                //Move Camera to boss room
                if (Player.transform.position.x < _room_spawn.transform.position.x - _spawn_size / 2)
                {
                    StartCoroutine(FreezePlayer(1));
                    CameraObject.GetComponent<CameraMovement>().ShiftLeft(_dungeon_size);
                    DungeonState = "TransitionToSpawnA";
                }

                //Transition to Game State Spawn
                if (Player.transform.position.x >= _room_boss.transform.position.x + _dungeon_size / 2 
                        + CameraObject.GetComponent<Camera>().orthographicSize * CameraObject.GetComponent<Camera>().aspect)
                {
                    CameraObject.GetComponent<CameraMovement>().CameraState = "Follow";

                    Destroy(_room_boss);

                    _room_left = MyInstantiate(
                        Levels[Level].Filler,
                        _room_spawn.transform.position.x - _spawn_size / 2 - _dungeon_size / 2,
                        _room_spawn.transform.position.y);
                    _room_current = MyInstantiate(
                        Levels[Level].Stages[_stage],
                        _room_boss.transform.position.x + _dungeon_size + _spawn_size,
                        _room_boss.transform.position.y);

                    Destroy(_wall);
                    Destroy(_wall_another);

                    _wall = MyInstantiate(
                        Levels[Level].Walls.SpawnWallsRight,
                        _room_spawn.transform.position.x,
                        _room_spawn.transform.position.y);
                    _wall_another = MyInstantiate(
                        Levels[Level].Walls.StaticWallsLeft, 
                        _room_current.transform.position.x, 
                        _room_current.transform.position.y);

                    _room_left.GetComponent<AudioSource>().volume = DestroySFXVolume;
                    _room_left.GetComponent<AudioSource>().PlayOneShot(DestroySFX);

                    RoomStage = _room_current;
                    if (Level + 1 < Levels.Count)
                        Level++;

                    DungeonState = "Spawn";

                    MusicManager.PlayMusic(MusicManager.SpawnMusic, MusicManager.SpawnMusicVolume);
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

    IEnumerator FreezePlayer(float time)
    {
        Player.GetComponent<PlayerMovement>().enabled = false;
        Player.GetComponent<PlayerMovement>().Stop();
        Player.GetComponent<PlayerCombat>().enabled = false;
        yield return new WaitForSeconds(time);

        Player.GetComponent<PlayerMovement>().enabled = true;
        Player.GetComponent<PlayerCombat>().enabled = true;
        Player.GetComponent<Animator>().SetTrigger("Idle");
    }

    IEnumerator OpenRight(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(_wall);
        _wall = MyInstantiate(
            Levels[Level].Walls.StaticWallsRight,
            _room_current.transform.position.x,
            _room_current.transform.position.y);

        _room_current.GetComponent<AudioSource>().volume = UnlockSFXVolume;
        _room_current.GetComponent<AudioSource>().PlayOneShot(UnlockSFX);
    }

    IEnumerator OpenBossRoom(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(_wall);

        _wall = MyInstantiate(
            Levels[Level].Walls.SpawnWallsLeft,
            _room_spawn.transform.position.x,
            _room_spawn.transform.position.y);
        _wall_another = MyInstantiate(
            Levels[Level].Walls.StaticWallsRight,
            _room_boss.transform.position.x,
            _room_boss.transform.position.y);

        _room_boss.GetComponent<AudioSource>().volume = UnlockSFXVolume;
        _room_boss.GetComponent<AudioSource>().PlayOneShot(UnlockSFX);
    }

    public Transform GetSpawnTransform()
    {
        try
        {
            return _room_spawn.transform;
        }
        catch
        {
            Debug.Log("Spawn does not exist");
            return null;
        }
        
    }
}
