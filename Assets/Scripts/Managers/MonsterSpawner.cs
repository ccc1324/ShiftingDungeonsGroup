using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Spawns mobs, and communicates with the dungeon manager when all mobs have been cleared
 */
public class MonsterSpawner : MonoBehaviour
{
    public enum DifficultyEnum
    {
        Easy, Normal, Hard
    }
    public DifficultyEnum Difficulty = DifficultyEnum.Hard;
    [Tooltip("Time between entering the room and mobs spawning")]
    public float SpawnDelay;
    [System.Serializable]
    public struct SpawnDataLists
    {
        public List<SpawnData> Easy;
        public List<SpawnData> Normal;
        public List<SpawnData> Hard;
    }
    [Tooltip("Spawn Data for each level (should match number of levels in dungeon manager)")]
    public SpawnDataLists SpawnDataInfo;

    const float CEILING_HEIGHT = 3.3f;
    const float FLOOR_HEIGHT = -3.4f;


    private bool _spawnMobs;
    private int _waveNumber;
    private int _stageNumber;
    private float _last_spawn_time;
    private SpawnData _spawn_data;
    private List<GameObject> _mobs;
    private Transform _room_transform;
    private DungeonManager _dungeon_manager;

    void Start()
    {
        _dungeon_manager = FindObjectOfType<DungeonManager>();
        _mobs = new List<GameObject>();
    }

    void Update()
    {
        if (_spawnMobs && _spawn_data == null)
        {
            _spawnMobs = false;
            StartCoroutine(SpawnBoss());
        }

        if (_spawnMobs)
        {
            SpawnData.WaveData current_wave = _spawn_data.StageList[_stageNumber].WaveList[_waveNumber]; //for readability

            if (Time.time - _last_spawn_time > _spawn_data.StageList[_stageNumber].SpawnCooldown + current_wave.WaveBuffer || _mobs.Count == 0)
            {
                //Spawn each mob in current wave and add it to _mobs list
                for (int i = 0; i < current_wave.Wave.MobsList.Count; i++)
                {
                    Wave.Mob mob = current_wave.Wave.MobsList[i];
                    float x = Random.Range(mob.LeftmostPosition, mob.RightmostPosition);
                    float y = mob.EjectSpeed > 0 ? FLOOR_HEIGHT : CEILING_HEIGHT;
                    GameObject myMob = Instantiate(current_wave.Wave.MobsList[i].Prefab, new Vector2(x + _room_transform.position.x, y), new Quaternion());
                    myMob.GetComponent<Rigidbody2D>().velocity = new Vector2(0, mob.EjectSpeed);
                    _mobs.Add(myMob);
                }

                _waveNumber++;
                _last_spawn_time = Time.time;
                StartCoroutine(UpdateMobsList());

                //End condition for mob spawning
                if (_waveNumber >= _spawn_data.StageList[_stageNumber].WaveList.Count)
                {
                    _spawnMobs = false;
                    if (_stageNumber == _spawn_data.StageList.Count - 1)
                        StartCoroutine(SpawnBoss());
                    else
                        StartCoroutine(TransitionToShifting());
                }
            }
        }
    }

    //Used by DungeonManager to start mob spawning
    public void SpawnMobs(int level, int stage)
    {
        List<SpawnData> list;
        switch (Difficulty)
        {
            case DifficultyEnum.Easy:
                list = SpawnDataInfo.Easy;
                break;
            case DifficultyEnum.Normal:
                list = SpawnDataInfo.Normal;
                break;
            case DifficultyEnum.Hard:
                list = SpawnDataInfo.Hard;
                break;
            default:
                list = SpawnDataInfo.Hard;
                break;
        }

        _spawn_data = list[level];
        _waveNumber = 0;
        _stageNumber = stage;
        _room_transform = _dungeon_manager.RoomStage.transform;
        StartCoroutine(StartSpawnDelay());
    }

    IEnumerator StartSpawnDelay()
    {
        yield return new WaitForSeconds(SpawnDelay);
        _spawnMobs = true;
    }

    //Remove dead mobs from mobs list
    IEnumerator UpdateMobsList()
    {
        while (_mobs.Count != 0)
        {
            for (int i = _mobs.Count - 1; i >= 0; i--)
            {
                if (_mobs[i] == null)
                    _mobs.RemoveAt(i);
            }
            yield return new WaitForSeconds(1.5f);
        }
    }

    //Should be called when final wave has been spawned and the next level is the boss level
    //Remove dead mobs from mobs list and set MobsCleared and SpawnBoss flag when all mobs are dead
    IEnumerator SpawnBoss()
    {
        while (_mobs.Count != 0)
        {
            for (int i = _mobs.Count - 1; i >= 0; i--)
            {
                if (_mobs[i] == null)
                    _mobs.RemoveAt(i);
            }
            yield return new WaitForSeconds(1.5f);
        }
        _dungeon_manager.MobsCleared = true;
        _dungeon_manager.SpawnBoss = true;
    }

    //Should be called when final wave has been spawned
    //Remove dead mobs from mobs list and set MobsCleared flag when all mobs are dead
    IEnumerator TransitionToShifting()
    {
        while (_mobs.Count != 0)
        {
            for (int i = _mobs.Count - 1; i >= 0; i--)
            {
                if (_mobs[i] == null)
                    _mobs.RemoveAt(i);
            }
            yield return new WaitForSeconds(1.5f);
        }
        _dungeon_manager.MobsCleared = true;
    }

    public void DestoryAllMobs()
    {
        foreach (GameObject mob in _mobs)
        {
            if (mob == null)
                continue;
            Destroy(mob);
        }
        _mobs = new List<GameObject>();
        _spawnMobs = false;
        _waveNumber = 0;
        _stageNumber = 0;
        StopAllCoroutines();
    }

    //2/22, Used for Demo Difficulty Selection
    public void SetDifficulty(int difficulty) //int because the editer can't serialize enums for button events
    {
        switch (difficulty)
        {
            case 0:
                Difficulty = DifficultyEnum.Easy;
                break;
            case 1:
                Difficulty = DifficultyEnum.Normal;
                break;
            case 2:
                Difficulty = DifficultyEnum.Hard;
                break;
            default:
                Difficulty = DifficultyEnum.Hard;
                break;
        }
    }
}
