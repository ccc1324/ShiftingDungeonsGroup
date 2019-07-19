using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MoveSpeed;
    public float JumpForce;
    private Rigidbody2D _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (Input.GetKey("a"))
        {
            _rigidbody.velocity = new Vector2(-MoveSpeed * Time.fixedDeltaTime, _rigidbody.velocity.y);
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (Input.GetKey("d"))
        {
            _rigidbody.velocity = new Vector2(MoveSpeed * Time.fixedDeltaTime, _rigidbody.velocity.y);
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
            _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);

    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, JumpForce);
        }
    }
}
