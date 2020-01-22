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

    private string _state;
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
        if (_state == "left") //if currently moving left
        {
            MoveLeft();
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
            MoveLeft();
            if (Time.time > _time_of_last_direction_change + DelayTime)
                _state = "right";
        }

        else if (_state == "right")//if currently moving right
        {
            MoveRight();
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
            MoveRight();
            if (Time.time > _time_of_last_direction_change + DelayTime)
                _state = "left";
        }
    }

    private void MoveRight()
    {
        _rigidbody.velocity = new Vector2(MoveSpeed * Time.fixedDeltaTime, _rigidbody.velocity.y);
        _gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
    }

    private void MoveLeft()
    {
        _rigidbody.velocity = new Vector2(-MoveSpeed * Time.fixedDeltaTime, _rigidbody.velocity.y);
        _gameObject.transform.eulerAngles = new Vector3(0, 180, 0);
    }

}
