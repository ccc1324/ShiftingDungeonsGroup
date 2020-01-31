using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAttackColorChange : StateMachineBehaviour
{
    public Color FinalColor = Color.blue;
    public float TransitionTime;
    public float WaitDuration;


    private SpriteRenderer _renderer;

    private Color _initialColor;
    private float _initialTime;
    private float _waitTime = 0;
    private float _initialTime2;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject _gameObject = animator.gameObject;
        GameObject body = _gameObject.transform.Find("Body").gameObject;
        _renderer = body.GetComponent<SpriteRenderer>();
        _initialColor = _renderer.color;
        _initialTime = Time.time;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Time.time - _initialTime < TransitionTime)
        {
            float t = (Time.time - _initialTime) / TransitionTime; //Sets value of t between 0 and 1 based on far into the animation
            _renderer.color = Color.LerpUnclamped(_initialColor, FinalColor, t);
        }
        else if(_waitTime == 0)
        {
            _waitTime = Time.time;
        }

        if(Time.time - _waitTime > WaitDuration)
        {
            _initialTime2 = Time.time;
            if (Time.time - _initialTime2 < TransitionTime)
            {
                float t = (Time.time - _initialTime2) / TransitionTime; //Sets value of t between 0 and 1 based on far into the animation
                _renderer.color = Color.LerpUnclamped(_initialColor, FinalColor, t);
            }

        }


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
