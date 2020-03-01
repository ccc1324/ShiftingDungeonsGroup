using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Behavior script that causes character to move towards the player as some speed
 * Enabling Delayed will cause character to conitnue oving a a certain direction
 *  for DelayTime even when player moved to the other side
 */
public class OnMoveTowardsPlayer : StateMachineBehaviour
{
    //Be careful not to modify these variables, as changes aren't reset on OnStateEnter
    public float MoveSpeed;
    public bool Delayed;
    public float DelayTime;
    public Vector2 RaycastOffset = new Vector2(.15f, -.5f);

    private string _state;
    private bool _frozen = false;
    private float _time_of_last_direction_change;
    private GameObject _player;
    private GameObject _gameObject;
    private Rigidbody2D _rigidbody;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        _player = GameObject.FindWithTag("Player"); //May cause performance issues, consider optimizing if there is a bottleneck
        _gameObject = animator.gameObject;
        _rigidbody = _gameObject.GetComponent<Rigidbody2D>();
        _time_of_last_direction_change = Time.time - DelayTime;
        if (_player.transform.position.x > _gameObject.transform.position.x)
            _state = "right";
        else
            _state = "left";
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        //Debug.DrawRay(new Vector2(_gameObject.transform.position.x + RaycastOffset.x, _gameObject.transform.position.y + RaycastOffset.y), Vector2.down, Color.blue);
        //Debug.DrawRay(new Vector2(_gameObject.transform.position.x - RaycastOffset.x, _gameObject.transform.position.y + RaycastOffset.y), Vector2.down, Color.blue);

        if (_state == "left") //if currently moving left
        {
            if (!_frozen)
            {
                MoveLeft();
            }
            if (_player.transform.position.x > _gameObject.transform.position.x) //if player is to the right
            {
                if (Delayed)
                {
                    _state = "transition_to_right";
                    _time_of_last_direction_change = Time.time;
                }
                else
                    _state = "right";
            }
        }

        else if (_state == "transition_to_right")
        {
            if (!_frozen)
            {
                MoveLeft();
            }
            if (Time.time > _time_of_last_direction_change + DelayTime)
            {
                _frozen = false;
                _state = "right";
            }
        }

        else if (_state == "right")//if currently moving right
        {
            if (!_frozen)
            {
                MoveRight();
            }
            if (_player.transform.position.x < _gameObject.transform.position.x)
            {
                if (Delayed)
                {
                    _state = "transition_to_left";
                    _time_of_last_direction_change = Time.time;
                }
                else
                    _state = "left";
            }
        }

        else if (_state == "transition_to_left")
        {
            if (!_frozen)
            {
                MoveRight();
            }
            if (Time.time > _time_of_last_direction_change + DelayTime) 
            { 
                _state = "left";
                _frozen = false;
            }
        }
    }

    private void MoveRight()
    {
        _rigidbody.velocity = new Vector2(MoveSpeed * Time.fixedDeltaTime, _rigidbody.velocity.y);
        _gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
        CheckForLedge();
    }

    private void MoveLeft()
    {
        _rigidbody.velocity = new Vector2(-MoveSpeed * Time.fixedDeltaTime, _rigidbody.velocity.y);
        _gameObject.transform.eulerAngles = new Vector3(0, 180, 0);
        CheckForLedge();
    }

    private void CheckForLedge()
    {   bool _onSurface = false; //Boolean to store whether either the left cast or right cast hit a platform or the ground
        if (_gameObject.transform.eulerAngles.y == 180) //If moving left
        {
            RaycastHit2D[] _leftHitArray = Physics2D.RaycastAll(new Vector2(_gameObject.transform.position.x - RaycastOffset.x, _gameObject.transform.position.y + RaycastOffset.y), Vector2.down, 1f);//Create an array of raycasts hits cast slightly to the left and straight down
            foreach (RaycastHit2D hit in _leftHitArray) //Check all hits
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("EnemyPlatforms") || hit.collider.gameObject.layer == LayerMask.NameToLayer("Default")) //
                {
                    _onSurface = true; //If any hits are on the ground layer, or enemy platform layer, we confirm we are on a surface
                }
            }
            if (!_onSurface) //If not on a surface, freeze the enemy
            {
                _frozen = true;
                _rigidbody.velocity = Vector2.zero;
            }
        }
        else //If moving right
        {
            RaycastHit2D[] _rightHitArray = Physics2D.RaycastAll(new Vector2(_gameObject.transform.position.x + RaycastOffset.x, _gameObject.transform.position.y + RaycastOffset.y), Vector2.down, 1f); //Create an array of raycasts hits cast slightly to the right and straight down
            foreach (RaycastHit2D hit in _rightHitArray)
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("EnemyPlatforms") || hit.collider.gameObject.layer == LayerMask.NameToLayer("Default"))
                {
                    _onSurface = true; //If any hits are on the ground layer, or enemy platform layer, we confirm we are on a surface
                }
            }
            if (!_onSurface) //If not on a surface, freeze the enemy
            {
                _frozen = true;
                _rigidbody.velocity = Vector2.zero;
            }
        }
    }

}
