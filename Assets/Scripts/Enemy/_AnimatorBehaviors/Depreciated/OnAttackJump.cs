using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Behaviour that causes the object to jump and move in the player's direction
 * Expects a rigidbody component to be attached to the object
 * Modifies the animator's Attack trigger and Grounded flag
 * Plays an attack sound effect if availabe
 */
public class OnAttackJump : StateMachineBehaviour
{
    [Tooltip("Amount of time that passes before attacking starts (waiting for animation)")]
    public float AttackStartBuffer;
    [Tooltip("Determines height of jump")]
    public float JumpForce;
    [Tooltip("Horizontal Speed during jump")]
    public float MoveSpeed;
    public AudioClip AttackSFX; //Can be null
    public float AttackVolume = 1;

    private float _startTime;
    private GameObject _gameObject;
    private GameObject _player;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private bool _jumped = false;
    private float _direction = 0;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _startTime = Time.time;
        _gameObject = animator.gameObject;
        _player = GameObject.FindWithTag("Player"); //May cause performance issues, consider optimizing if there is a bottleneck
        _rigidbody = animator.GetComponent<Rigidbody2D>();
        _animator = animator;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Time.time > _startTime + AttackStartBuffer)
        {
            if (!_jumped)
            {
                if (AttackSFX != null)
                    _gameObject.GetComponent<AudioSource>().PlayOneShot(AttackSFX);

                _rigidbody.velocity = new Vector2(0, JumpForce);
                _animator.ResetTrigger("Attack");
                _animator.SetBool("Grounded", false);

                _direction = Mathf.Sign(_player.transform.position.x - _gameObject.transform.position.x);
                _jumped = true;
            }

            _animator.SetFloat("VerticalSpeed", _rigidbody.velocity.y);
            _rigidbody.velocity = new Vector2(MoveSpeed * Time.fixedDeltaTime * _direction, _rigidbody.velocity.y);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
    }
}
