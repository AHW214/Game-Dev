using UnityEngine;

namespace PlatformerNoFSM
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class RaycastController : MonoBehaviour
    {
        public LayerMask collisionMask;
        public float targetDistBetweenRays = 0.25F;
        
        new internal BoxCollider2D collider;

        protected const float skinWidth = 0.015F;

        protected int horizontalRayCount;
        protected int verticalRayCount;
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

            float boundsWidth = bounds.size.x;
            float boundsHeight = bounds.size.y;

            targetDistBetweenRays = Mathf.Min(targetDistBetweenRays, boundsWidth, boundsHeight);

            horizontalRayCount = Mathf.FloorToInt(boundsHeight / targetDistBetweenRays) + 1;
            verticalRayCount = Mathf.FloorToInt(boundsWidth / targetDistBetweenRays) + 1;

            horizontalRaySpacing = boundsHeight / (horizontalRayCount - 1);
            verticalRaySpacing = boundsWidth / (verticalRayCount - 1);
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