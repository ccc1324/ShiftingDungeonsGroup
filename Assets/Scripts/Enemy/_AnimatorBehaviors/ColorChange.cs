using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Lerps between current color and a given color for a given sprite renderer, 
 * or of sprite componenet on the object the animator is attached to
 * SPRITE RENDERER COMPONENT MUST BE ON THE OBJECT THE ANIMATOR IS ATTACHED TO, OR A CHILD OF THAT OBJECT
 */

public class ColorChange : StateMachineBehaviour
{
    public string Name; //Name of object sprite renderer is attached to
    public Color NewColor;
    public float StartDelay; //Time between entering state and color change, not included in loop time
    public float LerpTime; //Time it takes to lerp to NewColor
    public float NewColorTime; //Time spent with Color = NewColor
    public float UnLerpTime; //Time it takes to lerp to original color
    public bool RevertOnExit; //Revert to original color when exiting the state
    public bool Loop; //Loop the color change, ignores start delay

    private float _start_time;
    private Color _originial_color;
    private SpriteRenderer _sprite_renderer;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        _start_time = Time.time;
        if (Name == "")
            _sprite_renderer = animator.GetComponent<SpriteRenderer>();
        else
            _sprite_renderer = animator.transform.Find(Name).GetComponent<SpriteRenderer>();
        _originial_color = _sprite_renderer.color;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        //Check if we should start lerping
        if (Time.time < _start_time + StartDelay)
            return;

        //Lerp to new color
        else if (Time.time < _start_time + StartDelay + LerpTime)
            _sprite_renderer.color = Color.Lerp(_originial_color, NewColor, (Time.time - _start_time - StartDelay) / LerpTime);

        //Keeping Color as NewColor
        else if (Time.time < _start_time + StartDelay + LerpTime + NewColorTime)
            return;

        //Lerp to original color
        else if (Time.time < _start_time + StartDelay + LerpTime + NewColorTime + UnLerpTime)
            _sprite_renderer.color = Color.Lerp(NewColor, _originial_color,
                (Time.time - _start_time - StartDelay - LerpTime - NewColorTime) / UnLerpTime);

        else
        {
            _sprite_renderer.color = _originial_color;
            if (Loop)
                _start_time = Time.time - StartDelay;
        }

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (RevertOnExit)
            _sprite_renderer.color = _originial_color;
    }
}
