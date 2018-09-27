using UnityEngine;

namespace Platformer
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class RaycastController : MonoBehaviour
    {
        public LayerMask collisionMask;
        public int horizontalRayCount = 4;
        public int verticalRayCount = 4;
        
        new internal BoxCollider2D collider;

        protected const float skinWidth = 0.015F;

        protected float horizontalRaySpacing;
        protected float verticalRaySpacing;

        protected RaycastOrigins raycastOrigins;

        protected virtual void Awake()
        {
            collider = GetComponent<BoxCollider2D>();
        }

        protected virtual void Start()
        {          
            CalculateRaySpacing();
        }

        protected void UpdateRaycastOrigins()
        {
            Bounds bounds = GetRaycastBounds();

            raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
            raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
            raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
            raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
        }

        private void CalculateRaySpacing()
        {
            Bounds bounds = GetRaycastBounds();

            horizontalRayCount = Mathf.Max(2, horizontalRayCount);
            verticalRayCount = Mathf.Max(2, verticalRayCount);

            horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
            verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
        }

        private Bounds GetRaycastBounds()
        {
            Bounds bounds = collider.bounds;
            bounds.Expand(-2 * skinWidth);

            return bounds;
        }

        protected struct RaycastOrigins
        {
            public Vector2 topLeft, topRight;
            public Vector2 bottomLeft, bottomRight;
        }
    }
}