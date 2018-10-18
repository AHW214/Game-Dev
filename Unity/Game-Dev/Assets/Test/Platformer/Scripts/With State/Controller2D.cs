﻿using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class Controller2D : MonoBehaviour
    {
        public LayerMask collisionMask;
        public float targetDistBetweenRays = 0.25F;

        internal RaycastHits hits = new RaycastHits();
        internal const float skinWidth = 0.015F;

        private new Collider2D collider;

        private readonly int[] rayCounts = new int[2];
        private readonly float[] raySpacings = new float[2];

        private void Start()
        {
            collider = GetComponent<Collider2D>();

            CalculateRaySpacing();           
        }

        private void Update()
        {
            hits.Reset();
        }

        public void DetectCollisions(Vector2 displacement)
        {
            for (int i = 0; i < 2; i++)
            {
                float magnitude = Mathf.Abs(displacement[i]);

                if (magnitude > 0)
                {
                    int sign = (int)Mathf.Sign(displacement[i]);

                    RaycastHit2D hit = RaycastRow(i, magnitude + skinWidth, sign, collisionMask);

                    if (hit)
                    {
                        hits[i][sign] = hit;                        
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

        public class RaycastHits
        {
            private readonly IDictionary<int, RaycastHit2D>[] components = new IDictionary<int, RaycastHit2D>[2];

            public RaycastHits()
            {
                for (int i = 0; i < 2; i++)
                {
                    components[i] = new Dictionary<int, RaycastHit2D>
                    {
                        {  1, new RaycastHit2D() },
                        { -1, new RaycastHit2D() }
                    };               
                }               
            }

            public void Reset()
            {
                for (int i = 0; i < 2; i++)
                {
                    components[i][1] = new RaycastHit2D();
                    components[i][-1] = new RaycastHit2D();                   
                }
            }

            public IDictionary<int, RaycastHit2D> X
            {
                get { return components[0]; }
            }

            public IDictionary<int, RaycastHit2D> Y
            {
                get { return components[1]; }
            }

            public IDictionary<int, RaycastHit2D> this[int index]
            {
                get { return components[index]; }
            }
        }
    }
}