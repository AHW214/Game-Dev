using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Controller2D : MonoBehaviour
    {
        public LayerMask collisionMask;
        public int horizontalRayCount = 4;
        public int verticalRayCount = 4;
        public float maximumClimbAngle = 80.0F;
        public float maximumDescendAngle = 75.0F;
        public bool drawRaycasts = false;

        public CollisionInfo collisions;

        private const float skinWidth = 0.015F;

        private float horizontalRaySpacing;
        private float verticalRaySpacing;

        new private BoxCollider2D collider;
        private RaycastOrigins raycastOrigins;

        private void Start()
        {
            collider = GetComponent<BoxCollider2D>();

            CalculateRaySpacing();
        }

        private void Update()
        {

        }

        public void Move(Vector2 displacement)
        {
            UpdateRaycastOrigins();
            collisions.Reset(displacement);

            if (displacement.y < 0)
            {
                DescendSlope(ref displacement);
            }

            if (displacement.x != 0)
            {
                HorizontalCollisions(ref displacement);
            }

            if (displacement.y != 0)
            {
                VerticalCollisions(ref displacement);
            }

            transform.Translate(displacement);
        }

        private void HorizontalCollisions(ref Vector2 displacement)
        {
            float directionX = Mathf.Sign(displacement.x);
            float rayLength = Mathf.Abs(displacement.x) + skinWidth;

            Vector2 rayOrigin = directionX == -1 ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;

            for (int i = 0; i < horizontalRayCount; i++)
            {
                Vector2 offsetOrigin = rayOrigin + Vector2.up * (i * horizontalRaySpacing);
                RaycastHit2D hit = Physics2D.Raycast(offsetOrigin, directionX * Vector2.right, rayLength, collisionMask);

                if (hit)
                {
                    float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                    if (i == 0 && slopeAngle <= maximumClimbAngle)
                    {
                        if (collisions.descendingSlope)
                        {
                            collisions.descendingSlope = false;
                            displacement = collisions.displacementOld;
                        }

                        float distanceToSlopeStart = 0.0F;
                        if (slopeAngle != collisions.slopeAngleOld)
                        {
                            distanceToSlopeStart = hit.distance - skinWidth;
                            displacement.x -= distanceToSlopeStart * directionX;
                        }

                        ClimbSlope(ref displacement, slopeAngle);
                        displacement.x += distanceToSlopeStart * directionX;
                    }

                    if (!collisions.climbingSlope || slopeAngle > maximumClimbAngle)
                    { 
                        displacement.x = directionX * (hit.distance - skinWidth);
                        rayLength = hit.distance;

                        if (collisions.climbingSlope)
                        {
                            displacement.y = Mathf.Tan(Mathf.Deg2Rad * collisions.slopeAngle) * Mathf.Abs(displacement.x);
                        }

                        collisions.left = directionX == -1;
                        collisions.right = directionX == 1;
                    }
                }

                if (drawRaycasts)
                {
                    Debug.DrawRay(offsetOrigin, rayLength * directionX * Vector2.right, Color.red);
                }
            }
        }

        private void VerticalCollisions(ref Vector2 displacement)
        {
            float directionY = Mathf.Sign(displacement.y);
            float rayLength = Mathf.Abs(displacement.y) + skinWidth;

            Vector2 rayOrigin = directionY == -1 ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;

            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 offsetOrigin = rayOrigin + Vector2.right * (i * verticalRaySpacing + displacement.x);
                RaycastHit2D hit = Physics2D.Raycast(offsetOrigin, directionY * Vector2.up, rayLength, collisionMask);

                if (hit)
                {
                    displacement.y = directionY * (hit.distance - skinWidth);
                    rayLength = hit.distance;

                    if (collisions.climbingSlope)
                    {
                        displacement.x = (displacement.y / Mathf.Tan(Mathf.Deg2Rad * collisions.slopeAngle)) * Mathf.Sign(displacement.x);
                    }

                    collisions.below = directionY == -1;
                    collisions.above = directionY == 1;
                }

                if (drawRaycasts)
                {
                    Debug.DrawRay(offsetOrigin, rayLength * directionY * Vector2.up, Color.red);
                }
            }

            if (collisions.climbingSlope)
            {
                float directionX = Mathf.Sign(displacement.x);
                rayLength = Mathf.Abs(displacement.x) + skinWidth;
                rayOrigin = directionX == -1 ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;

                Vector2 offsetOrigin = rayOrigin + displacement.y * Vector2.up;
                RaycastHit2D hit = Physics2D.Raycast(offsetOrigin, directionX * Vector2.right, rayLength, collisionMask);

                if (hit)
                {
                    float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                    if (slopeAngle != collisions.slopeAngle)
                    {
                        displacement.x = directionX * (hit.distance - skinWidth);
                        collisions.slopeAngle = slopeAngle;
                    }
                }
            }
        }

        private void ClimbSlope(ref Vector2 displacement, float slopeAngle)
        {
            float moveDistance = Mathf.Abs(displacement.x);
            float climbDisplacementY = Mathf.Sin(Mathf.Deg2Rad * slopeAngle) * moveDistance;

            if (displacement.y <= climbDisplacementY)
            {
                displacement.y = climbDisplacementY;
                displacement.x = Mathf.Sign(displacement.x) * Mathf.Cos(Mathf.Deg2Rad * slopeAngle) * moveDistance;

                collisions.below = true;
                collisions.climbingSlope = true;

                collisions.slopeAngle = slopeAngle;
            }           
        }

        private void DescendSlope(ref Vector2 displacement)
        {
            float directionX = Mathf.Sign(displacement.x);
            Vector2 rayOrigin = directionX == -1 ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity, collisionMask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != 0 && slopeAngle <= maximumDescendAngle)
                {
                    if (Mathf.Sign(hit.normal.x) == directionX &&
                        hit.distance - skinWidth <= Mathf.Tan(Mathf.Deg2Rad * slopeAngle) * Mathf.Abs(displacement.x))
                    {
                        float moveDistance = Mathf.Abs(displacement.x);
                        float descendDisplacementY = Mathf.Sin(Mathf.Deg2Rad * slopeAngle) * moveDistance;

                        displacement.x = Mathf.Cos(Mathf.Deg2Rad * slopeAngle) * moveDistance * Mathf.Sign(displacement.x);
                        displacement.y -= descendDisplacementY;

                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;
                        collisions.below = true;
                    }
                }
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

        private struct RaycastOrigins
        {
            public Vector2 topLeft, topRight;
            public Vector2 bottomLeft, bottomRight;
        }

        public struct CollisionInfo
        {
            public bool above, below;
            public bool left, right;

            public bool climbingSlope, descendingSlope;
            public float slopeAngle, slopeAngleOld;

            public Vector2 displacementOld;

            public void Reset(Vector2 displacement)
            {
                above = below = false;
                left = right = false;

                climbingSlope = false;
                descendingSlope = false;

                slopeAngleOld = slopeAngle;
                slopeAngle = 0.0F;

                displacementOld = displacement;
            }
        }
    }
}