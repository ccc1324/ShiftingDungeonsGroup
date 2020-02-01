using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDashAttack : StateMachineBehaviour
{
    //Be careful not to modify these variables in the script, as changes aren't reset on OnStateEnter
    [Tooltip("Amount of time that passes before attacking starts (waiting for animation)")]
    public float AttackStartBuffer;
    public AudioClip AttackSFX; //Can be null
    public float AttackVolume = 1;

    [Tooltip("How many times to dash")]
    public int AttackCount = 1;
    [Tooltip("Delay between dashes (if enemy is going to dash multiple times)")]
    public float DashDelay;

    [Tooltip("Velocity added to the slime when dashing")]
    public float DashVelocityX = 1f;
    public float DashVelocityY = 0f; //This is in case we want the slime to hop a little bit when dashing. If we want a dash to be the slime sliding, leave this at 0

    private int _attack_count;
    private float _startTime;
    private float _time_of_last_dash;
    private GameObject _gameObject;
    private GameObject _player;
    private Rigidbody2D _rigidBody2D;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {
        _startTime = Time.time;
        _time_of_last_dash = Time.time - DashDelay; //first dash will never be delayed by this condition
        _attack_count = AttackCount;
        _gameObject = animator.gameObject;
        _player = GameObject.FindWithTag("Player"); //May cause performance issues, consider optimizing if there is a bottleneck
        _rigidBody2D = _gameObject.GetComponent<Rigidbody2D>();

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Time.time > _startTime + AttackStartBuffer && Time.time > _time_of_last_dash + DashDelay && _attack_count > 0)
        {
            if (AttackSFX != null)
                animator.GetComponent<AudioSource>().PlayOneShot(AttackSFX, AttackVolume);
            if (_player.transform.position.x > _gameObject.transform.position.x) //if player is to the right
            {
                _rigidBody2D.velocity = new Vector2(DashVelocityX, DashVelocityY);
            }
            else if (_player.transform.position.x < _gameObject.transform.position.x) //if player is to the left
            {
                _rigidBody2D.velocity = new Vector2(-DashVelocityX, DashVelocityY);
            }
            _time_of_last_dash = Time.time;
            _attack_count--;
        }
    }

}
