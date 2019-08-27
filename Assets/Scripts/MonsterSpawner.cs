using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public float SpawnDelay;
    public List<Waves> WaveData;

    private bool _spawnMobs;
    private int _waveNumber;
    private float _last_spawn_time;
    private Waves _waves;
    private List<GameObject> _mobs;
    private Transform _player_transform;
    private DungeonManager _dungeon_manager;

    void Start()
    {
        _player_transform = FindObjectOfType<PlayerInventory>().transform;
        _dungeon_manager = FindObjectOfType<DungeonManager>();
        _mobs = new List<GameObject>();
    }

    void Update()
    {
        if (_spawnMobs)
        {
            Waves.Wave wave = _waves.WaveArray[_waveNumber];

            if (Time.time - _last_spawn_time > _waves.SpawnCooldown + wave.WaveBuffer || _mobs.Count == 0)
            {
                for (int i = 0; i < wave.Mobs.Count; i++)
                {
                    Waves.Mob mob = wave.Mobs[i];
                    float x = Random.Range(mob.Location.x, mob.Location.y);
                    float y = mob.EjectSpeed > 0 ? -3 : 3;
                    GameObject myMob = Instantiate(wave.Mobs[i].Prefab, new Vector2(x + _player_transform.position.x, y), new Quaternion());
                    myMob.GetComponent<Rigidbody2D>().velocity = new Vector2(0, mob.EjectSpeed);
                    _mobs.Add(myMob);
                }

                _waveNumber++;
                _last_spawn_time = Time.time;

                if (_waveNumber >= _waves.WaveArray.Count)
                {
                    _spawnMobs = false;
                    StartCoroutine(SpawnBoss());
                }
            }
        }
    }

    public void SpawnMobs(int level)
    {
        _waves = WaveData[level];
        _waveNumber = 0;
        StartCoroutine(StartSpawnDelay());
    }

    IEnumerator StartSpawnDelay()
    {
        yield return new WaitForSeconds(SpawnDelay);
        _spawnMobs = true;
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
            yield return new WaitForSeconds(2);
        }
        _dungeon_manager.SpawnBoss = true;
    }
}
