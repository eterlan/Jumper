using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TransformJump : MonoBehaviour
{
    public GroundCheckWithSnapping groundCheck;
    public float                   jumpForceY      =20;
    public float                   jumpForceX      =20;
    public float                   gravity         = -9.81f;
    public float                   gravityScale    = 5;
    public float                   xSpeed          = 5;
    public float                   xSpeedLerpSpeed = 0.2f;
    
    [SerializeField]
    private float yVelocity;

    [SerializeField]
    private float xRuntimeSpeed;
    
    private float playerHeight;
    private int   jumpCount;

    void Update()
    {
        yVelocity += gravity * gravityScale * Time.deltaTime;
        if (groundCheck.isGrounded && yVelocity < 0)
        {
            jumpCount          = 0;
            yVelocity           = 0;
            transform.position = new Vector3(transform.position.x, groundCheck.surfacePosition.y + playerHeight, transform.position.z);
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2) 
        {
            jumpCount ++;
            yVelocity     = jumpForceY;
            xRuntimeSpeed = jumpForceX;
        }

        xRuntimeSpeed = Mathf.Lerp(xRuntimeSpeed, xSpeed, xSpeedLerpSpeed);
        var horizontalInput    = Input.GetAxis("Horizontal");
        var horizontalMovement = horizontalInput * xRuntimeSpeed * Time.deltaTime;

        transform.Translate(new Vector3(horizontalMovement, yVelocity, 0) * Time.deltaTime);
    }

    private void Awake()
    {
        groundCheck  = GetComponent<GroundCheckWithSnapping>();
        playerHeight = transform.GetComponent<BoxCollider2D>().size.y / 2;
    }
}