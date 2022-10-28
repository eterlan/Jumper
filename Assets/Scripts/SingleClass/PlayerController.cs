using System;
using System.Collections;
using System.Collections.Generic;
using FSM;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public  float     gravity = -3;
    private FsmState m_currentState;

    private Rigidbody2D     m_rb2d;
    private BoxCollider2D   m_collider2D;
    private int             m_groundLayer;
    private float           timeInAir;
    private bool            isGround;
    private ContactFilter2D filter2D;
    private Collider2D[]    contacts = new Collider2D[1];
    private float           jumpForce = 10; 
 
    private void Awake()
    {
        m_rb2d                          = GetComponent<Rigidbody2D>();
        m_collider2D                    = GetComponent<BoxCollider2D>();
        m_groundLayer                   = LayerMask.NameToLayer("Ground");
        filter2D                        = new ContactFilter2D { layerMask = m_groundLayer };
    }

    private void Update()
    {


    }

    private void FixedUpdate()
    {
        isGround      = Physics2D.IsTouchingLayers(m_collider2D);
        if (!isGround)
        {
            timeInAir += Time.fixedDeltaTime;
            var position = m_rb2d.position;
            var nextPos  = new Vector2(position.x, position.y + 0.5f * gravity * Mathf.Pow(timeInAir, 2));
            m_rb2d.MovePosition(nextPos);
        }
        else
        {
            if (timeInAir > 0)
            {
                timeInAir = 0;
                m_rb2d.GetContacts(filter2D, contacts);
                var surfacePos   = Physics2D.ClosestPoint(m_rb2d.position, contacts[0]);
                var pos          = m_rb2d.position;
                var posOnSurface = new Vector2(pos.x, surfacePos.y + m_collider2D.size.y / 2);
                m_rb2d.position = posOnSurface;
            }

            if (Input.GetButtonDown("Jump"))
            {
                m_rb2d.AddForce(new Vector2(0, jumpForce));
            }
        }
    }
}
