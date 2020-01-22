using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Behaviour that updates the animator's VerticalSpeed and velocity every update cycle
 * Expects a rigidbody component to be attached to the object
 */
public class OnAttackJumpDown : StateMachineBehaviour
{
    public float MoveSpeed;

    private Animator _animator;
    private Rigidbody2D _rigidbody;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        _animator = animator;
        _rigidbody = animator.GetComponent<Rigidbody2D>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        _animator.SetFloat("VerticalSpeed", _rigidbody.velocity.y);
        //_rigidbody.velocity = new Vector2(MoveSpeed * Time.fixedDeltaTime * Mathf.Sign(_rigidbody.velocity.x), _rigidbody.velocity.y);
    }
}
