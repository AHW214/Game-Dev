using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Controller2D : MonoBehaviour
    {     
        public int horizontalRayCount = 4;
        public int verticalRayCount = 4;

        public bool drawRaycasts = false;

        private const float skinWidth = 0.015F;

        private float horizontalRaySpacing;
        private float verticalRaySpacing;

        new private BoxCollider2D collider;
        private RaycastOrigins raycastOrigins;

        private void Start()
        {
            collider = GetComponent<BoxCollider2D>();
        }

        private void Update()
        {
            UpdateRaycastOrigins();
            CalculateRaySpacing();

            if (drawRaycasts)
            {
                DrawRaycasts();
            }
        }

        private Bounds GetRaycastBounds()
        {
            Bounds bounds = collider.bounds;
            bounds.Expand(-2 * skinWidth);

            return bounds;
        }

        private void UpdateRaycastOrigins()
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

        private void DrawRaycasts()
        {
            Vector2 horizontalOffset = horizontalRaySpacing * Vector2.down;
            Vector2 verticalOffset = verticalRaySpacing * Vector2.right;

            for (int i = 0; i < horizontalRayCount; i++)
            {
                Debug.DrawRay(raycastOrigins.topRight + i * horizontalOffset, 2 * Vector2.right, Color.red);
            }

            for (int i = 0; i < verticalRayCount; i++)
            {
                Debug.DrawRay(raycastOrigins.bottomLeft + i * verticalOffset, 2 * Vector2.down, Color.red);
            }
        }

        struct RaycastOrigins
        {
            public Vector2 topLeft, topRight;
            public Vector2 bottomLeft, bottomRight;
        }
    }
}