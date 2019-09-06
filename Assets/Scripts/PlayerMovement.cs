using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MaxSpeed;
    public float Acceleration;
    public float JumpForce;
    public bool Grounded;
    public bool Stunned;

    private float _moveSpeed;
    private Rigidbody2D _rigidbody;
    private Animator _animator;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Stunned)
            return;

        _animator.SetFloat("VerticalSpeed", _rigidbody.velocity.y);

        #region Movement
        if (Input.GetKey("a"))
        {
            _moveSpeed = _moveSpeed > 0 ? 0 : _moveSpeed - Acceleration;
            _moveSpeed = _moveSpeed < -MaxSpeed ? -MaxSpeed : _moveSpeed;

            _rigidbody.velocity = new Vector2(_moveSpeed * Time.deltaTime, _rigidbody.velocity.y);
            transform.eulerAngles = new Vector3(0, 180, 0);
            _animator.SetBool("Running", true);
        }
        else if (Input.GetKey("d"))
        {
            _moveSpeed = _moveSpeed < 0 ? 0 : _moveSpeed + Acceleration;
            _moveSpeed = _moveSpeed > MaxSpeed ? MaxSpeed : _moveSpeed;

            _rigidbody.velocity = new Vector2(_moveSpeed * Time.deltaTime, _rigidbody.velocity.y);
            transform.eulerAngles = new Vector3(0, 0, 0);
            _animator.SetBool("Running", true);
        }
        else
        {
            _moveSpeed = 0;
            _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
            _animator.SetBool("Running", false);
        }
        #endregion

        #region Jumping
        if (Input.GetKeyDown(";"))
        {
            if (Grounded == true)
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, JumpForce);
                Grounded = false;
                _animator.SetBool("Running", false);
                _animator.SetBool("Grounded", false);
            }
        }
        #endregion
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Room" || collision.collider.tag == "Platform")
        {
            Grounded = true;
            _animator.SetBool("Grounded", true);
        }
    }

    public void Stop() //Stops player horizontal movement
    {
        _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
    }
}
