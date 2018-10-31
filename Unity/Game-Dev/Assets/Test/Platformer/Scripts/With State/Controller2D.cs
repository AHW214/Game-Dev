using UnityEngine;
using System;

namespace PlatformerFSM
{
    public class Controller2D : MonoBehaviour
    {
        public LayerMask collisionMask;
        public float targetDistBetweenRays = 0.25F;

        internal Collisions collisions = new Collisions();
        internal const float skinWidth = 0.015F;

        internal new Collider2D collider;
        private Player player;

        private readonly int[] rayCounts = new int[2];
        private readonly float[] raySpacings = new float[2];

        private void Start()
        {
            collider = GetComponent<Collider2D>();
            player = GetComponent<Player>();

            CalculateRaySpacing();           
        }

        public void DetectCollisions(Vector2 displacement)
        {
            for (int i = 0; i < 2; i++)
            {
                collisions.Reset(i);

                float magnitude = Mathf.Abs(displacement[i]);
                
                if (magnitude > 0)
                {
                    int sign = (int)Mathf.Sign(displacement[i]);

                    RaycastHit2D hit = RaycastRow(i, magnitude + skinWidth, sign, collisionMask);

                    if (hit)
                    {
                        collisions[i][sign] = hit;
                    }
                }
            }
        }

        public void HandleCollisions()
        {
            for (int i = 0; i < 2; i++)
            {
                foreach (int dir in collisions[i].Keys)
                {
                    RaycastHit2D? hit = collisions[i][dir];

                    if (hit != null)
                    {
                        float disp = hit.Value.distance - skinWidth;

                        player.displacement[i] = dir * disp;

                        if (disp < 1E-5)
                        {
                            player.velocity[i] = 0;
                        }
                    }
                }
            }
        }

        private RaycastHit2D RaycastRow(int compIndex, float initDist, int sign, LayerMask mask)
        {
            Bounds bounds = GetRaycastBounds();

            Vector2 direction = new Vector2();
            direction[compIndex] = sign;

            Vector2 normal = new Vector2();
            normal[compIndex ^ 1] = -sign;

            Vector2 origin = (Vector2)bounds.center + (direction * bounds.extents[compIndex]) + (normal * bounds.extents[compIndex ^ 1]);

            float dist = Mathf.Max(initDist, skinWidth) + skinWidth;

            RaycastHit2D hit = new RaycastHit2D();
            for (int i = 0; i < rayCounts[compIndex]; i++)
            {
                RaycastHit2D tmp = Physics2D.Raycast(origin - i * normal * raySpacings[compIndex], direction, dist, mask);

                Debug.DrawRay(origin - i * normal * raySpacings[compIndex], direction, Color.red);

                if (tmp)
                {
                    hit = tmp;
                    dist = hit.distance;
                }
            }

            return hit;
        }

        private void CalculateRaySpacing()
        {
            Bounds bounds = GetRaycastBounds();

            targetDistBetweenRays = Mathf.Min(targetDistBetweenRays, bounds.size.x, bounds.size.y);

            for (int i = 0; i < 2; i++)
            {
                rayCounts[i] = Mathf.FloorToInt(bounds.size[i ^ 1] / targetDistBetweenRays) + 1;
                raySpacings[i] = bounds.size[i ^ 1] / (rayCounts[i] - 1);
            }
        }

        private Bounds GetRaycastBounds()
        {
            Bounds bounds = collider.bounds;
            bounds.Expand(-2 * skinWidth);

            return bounds;
        }
    }
}