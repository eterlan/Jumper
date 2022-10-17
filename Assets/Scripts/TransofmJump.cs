using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TransformJump : MonoBehaviour
{
    public GroundCheckWithSnapping groundCheck;
    public float                   jumpForce    =20;
    public float                   gravity      = -9.81f;
    public float                   gravityScale = 5;

    [SerializeField]
    private float velocity;
    private float playerHeight;
    private int   jumpCount;

    void Update()
    {
        velocity += gravity * gravityScale * Time.deltaTime;
        if (groundCheck.isGrounded && velocity < 0)
        {
            jumpCount          = 0;
            velocity           = 0;
            transform.position = new Vector3(transform.position.x, groundCheck.surfacePosition.y + playerHeight, transform.position.z);
        }
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2) 
        {
            jumpCount++;
            velocity = jumpForce;
        }
        transform.Translate(new Vector3(0, velocity, 0) * Time.deltaTime);
    }

    private void Awake()
    {
        groundCheck  = GetComponent<GroundCheckWithSnapping>();
        playerHeight = transform.GetComponent<BoxCollider2D>().size.y / 2;
    }
}