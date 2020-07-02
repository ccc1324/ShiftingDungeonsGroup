using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Helps Skeleton Boss spawn in minions on platforms using a Wave ScriptableObject
 * Contains a lot of overlapping logic as MonsterSpawner
 */
public class SkeletonBossMinionManager : MonoBehaviour
{
    public Wave Mobs;
    const float CEILING_HEIGHT = 3.3f;
    const float FLOOR_HEIGHT = -3.4f;

    GameObject[] MobList;
    DungeonManager _dungeon_manager;

    void Start()
    {
        _dungeon_manager = FindObjectOfType<DungeonManager>();
        MobList = new GameObject[Mobs.MobsList.Count];
    }

    public void SpawnMinions()
    {
        for (int i = 0; i < MobList.Length; i++)
        {
            if (MobList[i] == null)
            {
                float x = Random.Range(Mobs.MobsList[i].LeftmostPosition, Mobs.MobsList[i].RightmostPosition);
                float y = Mobs.MobsList[i].EjectSpeed > 0 ? FLOOR_HEIGHT : CEILING_HEIGHT;
                MobList[i] = Instantiate(Mobs.MobsList[i].Prefab, new Vector2(x + _dungeon_manager.RoomStage.transform.position.x, y), new Quaternion(), _dungeon_manager.RoomStage.transform);
                MobList[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0, Mobs.MobsList[i].EjectSpeed);
            }
        }
    }
}
