using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Behavior that finds all sprites of a gameobject and makes them more transparent over time
 * To be used as a Behavior, attached to the death state of the character's animator
 * Also drops an item determined by the ItemDropManager component of the gameObject
 *  and Destroy the gameObject
 */
public class OnDeath : StateMachineBehaviour
{
    //Be careful not to modify these variables, as changes aren't reset on OnStateEnter
    public float DecayStartBuffer;
    public float DecayDuration;

    [System.Serializable]
    public struct ItemSets
    {
        public ItemSet Common;
        public ItemSet Uncommon;
        public ItemSet Epic;
    }
    public ItemSets ItemDropSets;
    public GameObject Item;

    private SpriteRenderer[] _spriteRenderers;
    private float _startTime;
    private GameObject _gameObject;
    private ItemDropManager _item_drop_manager;
    private float decayProgress;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        _gameObject = animator.gameObject;
        _spriteRenderers = _gameObject.GetComponentsInChildren<SpriteRenderer>();
        _startTime = Time.time;
        _item_drop_manager = GameObject.FindGameObjectWithTag("ItemDropManager").GetComponent<ItemDropManager>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        decayProgress = (Time.time - _startTime - DecayStartBuffer) / DecayDuration;
        foreach (SpriteRenderer sprite in _spriteRenderers)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1 - decayProgress);
        }

        if (decayProgress > 1)
        {
            Item item = _item_drop_manager.GetDrop(ItemDropSets.Common, ItemDropSets.Uncommon, ItemDropSets.Epic);
            EnemyFunctions.SpawnItem(Item, item, _gameObject.transform.position);
            Destroy(_gameObject);
        }
    }
}

