using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class PlatformController : RaycastController
    {
        public LayerMask passengerMask;
        public bool cyclicLoop;

        public float speed;
        public float waitTime;

        [Range(0, 2)]
        public float easeAmount;

        public Vector2[] localWaypoints;
        private Vector2[] globalWaypoints;

        private List<PassengerMovement> passengerMovement;
        private IDictionary<Transform, Controller2D> passengerDictionary = new Dictionary<Transform, Controller2D>();

        protected override void Start()
        {
            base.Start();

            globalWaypoints = new Vector2[localWaypoints.Length];
            for (int i = 0; i < localWaypoints.Length; i++)
            {
                globalWaypoints[i] = localWaypoints[i] + (Vector2)transform.position;
            }

            StartCoroutine(WaypointLoop());
        }

        private float Ease(float x)
        {
            float a = easeAmount + 1;
            return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
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

                    if (hit && hit.distance != 0 && !movedPassengers.Contains(hit.transform))
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

                    if (hit && hit.distance != 0 && !movedPassengers.Contains(hit.transform))
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

                    if (hit && hit.distance != 0 && !movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);

                        float pushX = displacement.x;
                        float pushY = displacement.y;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector2(pushX, pushY), true, false));
                    }
                }
            }
        }

        private IEnumerator WaypointLoop()
        {
            bool movingForward = true;

            while (true)
            {
                yield return StartCoroutine(movingForward ? MoveForward() : MoveBack());

                if (cyclicLoop && movingForward)
                {
                    yield return StartCoroutine(MoveBetween(globalWaypoints.Length - 1, 0));
                }

                else if (cyclicLoop && !movingForward)
                {
                    yield return StartCoroutine(MoveBetween(0, globalWaypoints.Length - 1));
                }

                else
                {
                    movingForward = !movingForward;
                }             
            }
        }

        private IEnumerator MoveForward()
        {
            for (int i = 0; i < globalWaypoints.Length - 1; i++)
            {
                yield return StartCoroutine(MoveBetween(i, i + 1));              
            }
        }

        private IEnumerator MoveBack()
        {
            for (int i = globalWaypoints.Length - 1; i > 0; i--)
            {
                yield return StartCoroutine(MoveBetween(i, i - 1));
            }
        }

        private IEnumerator MoveBetween(int fromIndex, int toIndex)
        {
            Vector2 fromWaypoint = globalWaypoints[fromIndex];
            Vector2 toWaypoint = globalWaypoints[toIndex];

            float distanceBetweenWaypoints = Vector2.Distance(fromWaypoint, toWaypoint);

            float percentBetweenWaypoints = 0.0F;
            while (percentBetweenWaypoints < 1)
            {
                percentBetweenWaypoints += (speed * Time.deltaTime) / distanceBetweenWaypoints;
                float easedPercentBetweenWaypoints = Ease(Mathf.Clamp01(percentBetweenWaypoints));

                Vector2 newPos = Vector2.Lerp(fromWaypoint, toWaypoint, easedPercentBetweenWaypoints);
                Vector2 displacement = newPos - (Vector2)transform.position;

                PassengerRoutine(displacement);

                yield return null;
            }

            yield return new WaitForSeconds(waitTime);
        }

        private void PassengerRoutine(Vector2 displacement)
        {
            UpdateRaycastOrigins();

            CalculatePassengerMovement(displacement);

            MovePassengers(true);
            transform.Translate(displacement);
            MovePassengers(false);
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

        private void OnDrawGizmos()
        {
            if (localWaypoints != null)
            {
                Gizmos.color = Color.red;
                float size = 0.3F;

                for (int i = 0; i < localWaypoints.Length; i++)
                {
                    Vector2 globalWaypointPos = Application.isPlaying ? globalWaypoints[i] : localWaypoints[i] + (Vector2)transform.position;
                    Gizmos.DrawLine(globalWaypointPos + size * Vector2.down, globalWaypointPos + size * Vector2.up);
                    Gizmos.DrawLine(globalWaypointPos + size * Vector2.left, globalWaypointPos + size * Vector2.right);
                }
            }
        }
    }
}