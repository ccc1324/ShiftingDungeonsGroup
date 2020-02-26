﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float MaxSpeed;
    public float Acceleration;
    public float JumpForce;
    public AudioClip JumpSFX;
    public float JumpSFXVolume;
    public AudioClip LandSFX;
    public float LandSFXVolume;
    public bool Grounded;
    public bool Stunned;
    public bool InCombat;
    public float MaxJumpCoefficient;
    public float MinimumJumpCoefficient;

    private bool _isJumping;
    private float _jumpTimeStart;
    private float _moveSpeed;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private AudioSource _audio_source;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _audio_source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Stunned)
        {
            return;
        }

        _animator.SetFloat("VerticalSpeed", _rigidbody.velocity.y);

        if (InCombat && Grounded)
        {
            Stop();
            if (Input.GetKey("a"))
                transform.eulerAngles = new Vector3(0, 180, 0);
            else if (Input.GetKey("d"))
                transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            #region Movement
            if (Input.GetKey("a"))
            {
                _moveSpeed = _moveSpeed > 0 ? 0 : _moveSpeed - Acceleration;
                _moveSpeed = _moveSpeed < -MaxSpeed ? -MaxSpeed : _moveSpeed;

                _rigidbody.velocity = new Vector2(_moveSpeed * Time.fixedDeltaTime, _rigidbody.velocity.y);
                transform.eulerAngles = new Vector3(0, 180, 0);
                _animator.SetBool("Running", true);
            }
            else if (Input.GetKey("d"))
            {
                _moveSpeed = _moveSpeed < 0 ? 0 : _moveSpeed + Acceleration;
                _moveSpeed = _moveSpeed > MaxSpeed ? MaxSpeed : _moveSpeed;

                _rigidbody.velocity = new Vector2(_moveSpeed * Time.fixedDeltaTime, _rigidbody.velocity.y);
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
        }


        #region Jumping
        if (Input.GetKeyDown(";") && !InCombat)
        {
            if (Grounded == true)
            {
                Grounded = false;
                _isJumping = true;
                _jumpTimeStart = Time.time;
            }
        }

        if (_isJumping && (Input.GetKeyUp(";") || Time.time - _jumpTimeStart >= MaxJumpCoefficient))
        {
            _animator.SetBool("Running", false);
            _animator.SetBool("Grounded", false);
            _audio_source.volume = JumpSFXVolume;
            _audio_source.PlayOneShot(JumpSFX);

            float JumpCoefficient = Time.time - _jumpTimeStart;
            if (JumpCoefficient > MaxJumpCoefficient)
                JumpCoefficient = MaxJumpCoefficient;
            else if (JumpCoefficient < MinimumJumpCoefficient)
                JumpCoefficient = MinimumJumpCoefficient;

            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, JumpForce*JumpCoefficient);
            _isJumping = false;
        }
        #endregion
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Room" || collision.tag == "Platform")
        {
            if (Grounded) //avoid case of sound playing when moving between rooms
                return;
            Grounded = true;
            _animator.SetBool("Grounded", true);
            _audio_source.volume = LandSFXVolume;
            _audio_source.PlayOneShot(LandSFX);
        }
    }

    public void Stop() //Stops player horizontal movement
    {
        _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
    }
}
