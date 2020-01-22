using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Behaviour that removes horizontal speed from the object
 * Expects a rigidbody component to be attached to the object
 */
public class OnAttackJumpLanding : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.SetFloat("VerticalSpeed", 0);
        Rigidbody2D rigidbody = animator.GetComponent<Rigidbody2D>();
        rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
    }
}
