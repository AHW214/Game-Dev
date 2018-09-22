using UnityEngine;
using System;

namespace Game
{
    public class Direction
    {
        public static readonly int amount = 8;
        public static readonly float angleIncrement = 360.0F / amount;
        public static readonly float halfAngleIncrement = angleIncrement / 2.0F;

        public readonly Vector2 v2;

        private static Vector2 GetDir(float inAngle)
        {
            float floored = (int)(inAngle / angleIncrement) * angleIncrement;
            float outAngle = inAngle - floored > halfAngleIncrement ? floored + angleIncrement : floored;

            Vector2 outDir = Quaternion.AngleAxis(outAngle, Vector3.back) * Vector2.up;

            return outDir;
        }

        public Direction(float inAngle)
        {
            v2 = GetDir(inAngle);
        }

        public Direction(Vector2 inDir)
        {
            if (inDir == Vector2.zero)
            {
                throw new Exception("Direction cannot be a 0 vector.");
            }

            float acuteAngle = Vector2.Angle(Vector2.up, inDir);
            float inAngle = inDir.x < 0 ? 360 - acuteAngle : acuteAngle;

            v2 = GetDir(inAngle);
        }           
    }
}