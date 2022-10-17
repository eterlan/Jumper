using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GroundCheckWithSnapping : MonoBehaviour
{
    public bool     isGrounded;
    public float    offset = 0.1f;
    public Vector2  surfacePosition;
    ContactFilter2D filter;
    Collider2D[]    results = new Collider2D[1];
    private int     groundLayer;

    private void Update()
    {
        Vector2 point = transform.position + Vector3.down * offset;
        Vector2 size  = new Vector2(transform.localScale.x, transform.localScale.y);
        filter.SetLayerMask(1<<groundLayer);  

        if (Physics2D.OverlapBox(point, size, 0, filter, results) > 0)
        {
            isGrounded      = true; 
            surfacePosition = Physics2D.ClosestPoint(transform.position, results[0]);
        }
        else {
            isGrounded = false;
        }
    }

    private void Awake()
    {
        groundLayer = LayerMask.NameToLayer("Ground");
        filter      = new ContactFilter2D();
        Debug.Log("filter");
    }
}