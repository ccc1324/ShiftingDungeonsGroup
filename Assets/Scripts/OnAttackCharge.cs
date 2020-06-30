using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAttackCharge : StateMachineBehaviour
{
    public float ChargeSpeed = 1f;
    public float ChargeDelay = 1f;
    public bool FollowPlayer; //whether or not to update direction of player during delay
    public bool Inverse; //Is sprite inverted (facing left)
    
    private float _direction;
    private float _entryTime;
    private GameObject _gameObject;
    private GameObject _player;
    private Rigidbody2D _rigidBody2D;
    private float new_direction;
    private bool _flip_once = false;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _gameObject = animator.gameObject;
        _player = GameObject.FindWithTag("Player"); 
        _rigidBody2D = _gameObject.GetComponent<Rigidbody2D>();
        _direction = Mathf.Sign(_player.transform.position.x - _gameObject.transform.position.x);
        _entryTime = Time.time;

        FlipSprite();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (FollowPlayer && Time.time <= _entryTime + ChargeDelay)
        {
            new_direction = Mathf.Sign(_player.transform.position.x - _gameObject.transform.position.x);
            if (new_direction != _direction)
            {
                _direction = new_direction;
                FlipSprite();
            }
        }

        if (Time.time > _entryTime + ChargeDelay)
        {
            _rigidBody2D.velocity = new Vector2(ChargeSpeed * _direction, _rigidBody2D.velocity.y);
        }
    }

    private void FlipSprite()
    {
        _gameObject.transform.eulerAngles = _direction < 0 ?
                    Inverse ? _gameObject.transform.eulerAngles = new Vector3(0, 0, 0) :
                        _gameObject.transform.eulerAngles = new Vector3(0, 180, 0) :
                    Inverse ? _gameObject.transform.eulerAngles = new Vector3(0, 180, 0) :
                        _gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
    }
}
