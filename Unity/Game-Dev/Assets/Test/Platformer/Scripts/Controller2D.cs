using UnityEngine;

namespace Platformer
{ 
    public class Controller2D : RaycastController
    {       
        public float maximumClimbAngle = 80.0F;
        public float maximumDescendAngle = 75.0F;
        public bool drawRaycasts = false;

        public CollisionInfo collisions;
        internal Vector2 playerInput;

        protected override void Start()
        {
            base.Start();
            collisions.faceDirection = 1;
        }

        private void Update()
        {

        }

        public void Move(Vector2 displacement, bool standingOnPlatform)
        {
            Move(displacement, Vector2.zero, standingOnPlatform);
        }

        public void Move(Vector2 displacement, Vector2 input, bool standingOnPlatform = false)
        {
            UpdateRaycastOrigins();
            collisions.Reset(displacement);
            playerInput = input;

            if (displacement.x != 0)
            {
                collisions.faceDirection = (int)Mathf.Sign(displacement.x);
            }

            if (displacement.y < 0)
            {
                DescendSlope(ref displacement);
            }

            HorizontalCollisions(ref displacement);

            if (displacement.y != 0)
            {
                VerticalCollisions(ref displacement);
            }

            transform.Translate(displacement);

            if (standingOnPlatform)
            {
                collisions.below = true;
            }
        }

        private void HorizontalCollisions(ref Vector2 displacement)
        {
            float directionX = collisions.faceDirection;
            float rayLength = Mathf.Max(Mathf.Abs(displacement.x), skinWidth) + skinWidth;

            Vector2 rayOrigin = directionX == -1 ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;

            for (int i = 0; i < horizontalRayCount; i++)
            {
                Vector2 offsetOrigin = rayOrigin + Vector2.up * (i * horizontalRaySpacing);
                RaycastHit2D hit = Physics2D.Raycast(offsetOrigin, directionX * Vector2.right, rayLength, collisionMask);

                if (hit && hit.distance > 0)
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
                    Debug.DrawRay(offsetOrigin, directionX * Vector2.right, Color.red);
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
                    if (hit.collider.tag == "One Way Platform")
                    {
                        if (directionY == 1 || hit.distance == 0)
                        {
                            continue;
                        }

                        if(collisions.fallingThroughPlatform)
                        {
                            continue;
                        }

                        if (playerInput.y == -1)
                        {
                            collisions.fallingThroughPlatform = true;
                            Invoke("ResetFallingThroughPlatform", 0.5F);
                            continue;
                        }
                    }

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
                    Debug.DrawRay(offsetOrigin, directionY * Vector2.up, Color.red);
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

        private void ResetFallingThroughPlatform()
        {
            collisions.fallingThroughPlatform = false;
        }

        public struct CollisionInfo
        {
            public bool above, below;
            public bool left, right;

            public bool climbingSlope, descendingSlope;
            public float slopeAngle, slopeAngleOld;

            public Vector2 displacementOld;

            public int faceDirection;

            public bool fallingThroughPlatform;

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