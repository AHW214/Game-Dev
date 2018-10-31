using System;
using UnityEngine;

namespace PlatformerNoFSM
{
    public class CameraFollow : MonoBehaviour
    {
        public Controller2D target;
        public Vector2 focusAreaDimensions;

        public float verticalOffset;
        public float lookAheadDistX;
        public float lookSmoothTimeX;
        public float verticalSmoothTime;

        private FocusArea focusArea;

        private float currentLookAheadX;
        private float targetLookAheadX;
        private float lookAheadDirectionX;
        private float smoothLookVelocityX;
        private float smoothVelocityY;

        private bool lookAheadStopped;

        private void Start()
        {
            focusArea = new FocusArea(target.collider.bounds, focusAreaDimensions);
        }

        private void LateUpdate()
        {
            focusArea.Update(target.collider.bounds);

            Vector2 focusPosition = focusArea.center + verticalOffset * Vector2.up;

            if (focusArea.displacement.x != 0)
            {
                lookAheadDirectionX = Mathf.Sign(focusArea.displacement.x);

                if (Math.Sign(target.playerInput.x) == Math.Sign(focusArea.displacement.x))
                {
                    lookAheadStopped = false;
                    targetLookAheadX = lookAheadDirectionX * lookAheadDistX;
                }

                else if(!lookAheadStopped)
                {
                    lookAheadStopped = true;
                    targetLookAheadX = currentLookAheadX + (lookAheadDirectionX * lookAheadDistX - currentLookAheadX) / 4.0F;
                }
            }

            currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX);

            focusPosition.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref smoothVelocityY, verticalSmoothTime);
            focusPosition += currentLookAheadX * Vector2.right;

            transform.position = new Vector3(focusPosition.x, focusPosition.y, transform.position.z);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 0, 0, 0.5F);
            Gizmos.DrawCube(focusArea.center, focusAreaDimensions);
        }

        struct FocusArea
        {
            public Vector2 center;
            public Vector2 displacement;

            private float left, right;
            private float top, bottom;

            public FocusArea(Bounds targetBounds, Vector2 dimensions)
            {
                left = targetBounds.center.x - dimensions.x / 2;
                right = targetBounds.center.x + dimensions.x / 2;
                bottom = targetBounds.min.y;
                top = bottom + dimensions.y;

                center = new Vector2(left + right, top + bottom) / 2;
                displacement = Vector2.zero;
            }

            public void Update(Bounds targetBounds)
            {
                float shiftX = 0;

                if (targetBounds.min.x < left)
                {
                    shiftX = targetBounds.min.x - left;
                }

                else if (targetBounds.max.x > right)
                {
                    shiftX = targetBounds.max.x - right;
                }

                left += shiftX;
                right += shiftX;

                float shiftY = 0;

                if (targetBounds.min.y < bottom)
                {
                    shiftY = targetBounds.min.y - bottom;
                }

                else if (targetBounds.max.y > top)
                {
                    shiftY = targetBounds.max.y - top;
                }

                top += shiftY;
                bottom += shiftY;

                displacement = new Vector2(shiftX, shiftY);
                center += displacement;
            }
        }
    }
}