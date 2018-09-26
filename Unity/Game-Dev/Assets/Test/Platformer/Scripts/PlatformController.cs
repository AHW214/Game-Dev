using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class PlatformController : RaycastController
    {
        public LayerMask passengerMask;
        public Vector2 move;

        private List<PassengerMovement> passengerMovement;
        private IDictionary<Transform, Controller2D> passengerDictionary = new Dictionary<Transform, Controller2D>();

        protected override void Start()
        {
            base.Start();

        }

        private void Update()
        {
            UpdateRaycastOrigins();

            Vector2 displacement = move * Time.deltaTime;

            CalculatePassengerMovement(displacement);

            MovePassengers(true);
            transform.Translate(displacement);
            MovePassengers(false);
        }

        private void MovePassengers(bool beforeMovePlatform)
        {
            passengerMovement.ForEach((passenger) =>
            {
                if (!passengerDictionary.ContainsKey(passenger.transform))
                {
                    passengerDictionary.Add(passenger.transform, passenger.transform.GetComponent<Controller2D>());
                }

                if (passenger.moveBeforePlatform == beforeMovePlatform)
                {
                    passengerDictionary[passenger.transform].Move(passenger.displacement, passenger.standingOnPlatform);
                }
            });
        }

        private void CalculatePassengerMovement(Vector2 displacement)
        {
            HashSet<Transform> movedPassengers = new HashSet<Transform>();
            passengerMovement = new List<PassengerMovement>();

            float directionX = Mathf.Sign(displacement.x);
            float directionY = Mathf.Sign(displacement.y);

            if (displacement.y != 0)
            {
                float rayLength = Mathf.Abs(displacement.y) + skinWidth;
                Vector2 rayOrigin = directionY == -1 ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;

                for (int i = 0; i < verticalRayCount; i++)
                {
                    Vector2 offsetOrigin = rayOrigin + Vector2.right * (i * verticalRaySpacing);
                    RaycastHit2D hit = Physics2D.Raycast(offsetOrigin, directionY * Vector2.up, rayLength, passengerMask);

                    if (hit && !movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);

                        float pushX = directionY == 1 ? displacement.x : 0;
                        float pushY = displacement.y - directionY * (hit.distance - skinWidth);

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector2(pushX, pushY), directionY == 1, true));
                    }
                }
            }

            if (displacement.x != 0)
            {
                float rayLength = Mathf.Abs(displacement.x) + skinWidth;
                Vector2 rayOrigin = directionX == -1 ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;

                for (int i = 0; i < horizontalRayCount; i++)
                {
                    Vector2 offsetOrigin = rayOrigin + Vector2.up * (i * horizontalRaySpacing);
                    RaycastHit2D hit = Physics2D.Raycast(offsetOrigin, directionX * Vector2.right, rayLength, passengerMask);

                    if (hit && !movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);

                        float pushX = displacement.x - directionX * (hit.distance - skinWidth);
                        float pushY = -float.Epsilon;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector2(pushX, pushY), false, true));
                    }
                }
            }

            if(directionY == -1 || (displacement.y == 0 && displacement.x != 0))
            {
                float rayLength = 2 * skinWidth;
                Vector2 rayOrigin = raycastOrigins.topLeft;

                for (int i = 0; i < verticalRayCount; i++)
                {
                    Vector2 offsetOrigin = rayOrigin + Vector2.right * (i * verticalRaySpacing);
                    RaycastHit2D hit = Physics2D.Raycast(offsetOrigin, Vector2.up, rayLength, passengerMask);

                    if (hit && !movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);

                        float pushX = displacement.x;
                        float pushY = displacement.y;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector2(pushX, pushY), true, false));
                    }
                }
            }
        }

        private struct PassengerMovement
        {
            public Transform transform;
            public Vector2 displacement;

            public bool standingOnPlatform;
            public bool moveBeforePlatform;

            public PassengerMovement(Transform _transform, Vector2 _displacement, bool _standingOnPlatform, bool _moveBeforePlatform)
            {
                transform = _transform;
                displacement = _displacement;

                standingOnPlatform = _standingOnPlatform;
                moveBeforePlatform = _moveBeforePlatform;
            }
        }
    }
}