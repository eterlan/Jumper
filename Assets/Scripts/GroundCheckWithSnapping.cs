using Cinemachine;
using UnityEngine;

[SaveDuringPlay]
public class GroundCheckWithSnapping : MonoBehaviour
{
    public bool    isGrounded;
    public float   offset = 0.1f;
    public Vector2 surfacePosition;
    public Vector2 surfaceSize = new(0.1f, 0.1f);
    
    ContactFilter2D       filter;
    Collider2D[]          results = new Collider2D[1];
    private int           groundLayer;
    private BoxCollider2D boxCollider2D;
    private Vector3       point;
    private Vector2       size;
    


    private void Update()
    {
        point = transform.position + Vector3.down * offset;
        size  = boxCollider2D.size;

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(point, size);
        
        Gizmos.DrawCube(surfacePosition, surfaceSize);
    }
}