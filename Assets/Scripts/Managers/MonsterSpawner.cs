using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public float SpawnDelay;
    public List<Waves> WaveData;

    private bool _spawnMobs;
    private int _waveNumber;
    private int _stageNumber;
    private float _last_spawn_time;
    private Waves _waves;
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
        if (_spawnMobs)
        {
            Waves.Wave wave = _waves.StageList[_stageNumber].WaveList[_waveNumber]; //for readability
            if (Time.time - _last_spawn_time > _waves.StageList[_stageNumber].SpawnCooldown + wave.WaveBuffer || _mobs.Count == 0)
            {
                for (int i = 0; i < wave.Mobs.Count; i++)
                {
                    Waves.Mob mob = wave.Mobs[i];
                    float x = Random.Range(mob.Location.x, mob.Location.y);
                    float y = mob.EjectSpeed > 0 ? -3.4f : 3.3f;
                    GameObject myMob = Instantiate(wave.Mobs[i].Prefab, new Vector2(x + _room_transform.position.x, y), new Quaternion());
                    myMob.GetComponent<Rigidbody2D>().velocity = new Vector2(0, mob.EjectSpeed);
                    _mobs.Add(myMob);
                }

                _waveNumber++;
                _last_spawn_time = Time.time;
                StartCoroutine(UpdateMobsList());

                if (_waveNumber >= _waves.StageList[_stageNumber].WaveList.Count)
                {
                    _spawnMobs = false;
                    if (_stageNumber == _waves.StageList.Count - 1)
                        StartCoroutine(SpawnBoss());
                    else
                        StartCoroutine(TransitionToShifting());
                }
            }
        }
    }

    public void SpawnMobs(int level, int stage)
    {
        _waves = WaveData[level];
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
}
