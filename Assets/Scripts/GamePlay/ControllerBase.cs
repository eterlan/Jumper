using UnityEngine;

namespace GamePlay
{
    public abstract class ControllerBase : MonoBehaviour
    {
        // Collision
        public Rigidbody2D     rb2d;
        public ContactFilter2D groundFilter;
        public bool            isGround;
        // Animator
        public          Animator animator;  
        public          int      animOnGroundHash;
        public          int      animDashHash;

        public virtual void OnGround()
        {
            
        }
    }
}