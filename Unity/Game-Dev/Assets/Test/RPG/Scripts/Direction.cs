using UnityEngine;
using System;

namespace Game
{
    public class Direction
    {
        public static readonly int amount = 8;
        public static readonly float angleIncrement = 360.0F / amount;
        public static readonly float inverseAngleIncrement = 1 / angleIncrement;

        public readonly Vector2 pixelDirection;

        public Direction(float angle)
        {
            float rounded = Mathf.Round(angle * inverseAngleIncrement);
            float snappedAngle = rounded * angleIncrement;

            Vector2 dir = Quaternion.AngleAxis(snappedAngle, Vector3.back) * Vector2.up;
            pixelDirection = Vector2Int.RoundToInt(dir);
        }

        public Direction(Vector2 dir) : this(GetVectorAngle(dir))
        {
            
        }
        
        private static float GetVectorAngle(Vector2 vector)
        {
            if (vector == Vector2.zero)
            {
                throw new Exception("Direction cannot be a 0 vector.");
            }

            float acuteAngle = Vector2.Angle(Vector2.up, vector);
            float angle = vector.x < 0 ? 360 - acuteAngle : acuteAngle;

            return angle;
        }
    }
}