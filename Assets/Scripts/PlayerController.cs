using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private IFsmState m_currentState;

    private Rigidbody2D m_rb2d;
    private Collider2D m_collider2D;
    private int m_groundLayer;

    private void Awake()
    {
        m_rb2d        = GetComponent<Rigidbody2D>();
        m_collider2D  = GetComponent<Collider2D>();
        m_groundLayer = LayerMask.NameToLayer("Ground");
        //m_rb2d.useFullKinematicContacts = true; 
    }

    private void Update()
    {
        m_rb2d.useFullKinematicContacts = true; 

        m_groundLayer = LayerMask.NameToLayer("Ground");
        var isGround = Physics2D.IsTouchingLayers(m_collider2D); 
        Debug.Log(isGround); 
    }
}
