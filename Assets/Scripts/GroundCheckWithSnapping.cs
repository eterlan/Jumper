using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GroundCheckWithSnapping : MonoBehaviour
{
    public bool           isGrounded;
    public float          offset = 0.1f;
    public Vector2        surfacePosition;
    ContactFilter2D       filter;
    Collider2D[]          results = new Collider2D[1];
    private int           groundLayer;
    private BoxCollider2D boxCollider2D;

    private void Update()
    {
        var point = transform.position + Vector3.down * offset;
        var size  = boxCollider2D.size;

        isGrounded = Physics2D.OverlapBox(point, size, 0, filter, results) > 0; 
        if (isGrounded) surfacePosition = Physics2D.ClosestPoint(transform.position, results[0]);
    }

    private void Awake()
    {
        groundLayer = LayerMask.NameToLayer("Ground");
        filter      = new ContactFilter2D();
        filter.SetLayerMask(1 <<groundLayer);
        Debug.Log("filter");
        boxCollider2D = GetComponent<BoxCollider2D>();
    }
}