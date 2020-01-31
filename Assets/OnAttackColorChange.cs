using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAttackColorChange : StateMachineBehaviour
{
    public Color FinalColor;
    public float TransitionTime;

    private Color _initialColor;
    private SpriteRenderer _renderer;
    private float _initialTime;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _renderer = animator.gameObject.GetComponent<SpriteRenderer>();
        _initialColor = _renderer.color;
        _initialTime = Time.time;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float t = (Time.time - _initialTime)/TransitionTime; //Sets value of t between 0 and 1 based on far into the animation
        _renderer.color = Color.Lerp(_initialColor, FinalColor, t);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
