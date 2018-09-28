using System;
using UnityEngine;

namespace Platformer
{
    [RequireComponent(typeof(Controller2D))]
    public class Player : MonoBehaviour
    {
        public float maxJumpHeight = 4.0F;
        public float minJumpHeight = 1.0F;
        public float timeToJumpApex = 0.4F;
        public float accelerationTimeAirborne = 0.2F;
        public float accelerationTimeGrounded = 0.1F;
        public float movementSpeed = 6.0F;       

        public Vector2 wallJumpClimb;
        public Vector2 wallJumpOff;
        public Vector2 wallLeapOff;

        public float wallSlideSpeedMax = 3.0F;
        public float wallStickTime = 0.25F;

        private float gravity;
        private float maxJumpVelocity;
        private float minJumpVelocity;
        private float timeToWallUnstick;
        private float velocityXSmoothing;

        private Vector2 directionalInput = Vector2.zero;
        private Vector2 velocity = Vector2.zero;
        private Controller2D controller;

        private bool wallSliding;
        private int inputDirX, wallDirX;

        private void Start()
        {
            controller = GetComponent<Controller2D>();

            gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);

            maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
            minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);

            Debug.Log($"Gravity: {gravity}\nJump Speed: {maxJumpVelocity}");
        }

        private void Update()
        {
            CalculateVelocity();
            HandleWallSliding();

            controller.Move(velocity * Time.deltaTime, directionalInput);

            if (controller.collisions.above || controller.collisions.below)
            {
                velocity.y = 0;
            }
        }

        private void HandleWallSliding()
        {
            inputDirX = Math.Sign(directionalInput.x);
            wallDirX = controller.collisions.left ? -1 : 1;

            wallSliding = (controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0;

            if (wallSliding)
            {
                velocity.y = Mathf.Max(velocity.y, -wallSlideSpeedMax);

                if (timeToWallUnstick > 0)
                {
                    velocity.x = 0;
                    velocityXSmoothing = 0;

                    timeToWallUnstick = inputDirX == -wallDirX ? timeToWallUnstick - Time.deltaTime : wallStickTime;
                }

                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
        }

        private void CalculateVelocity()
        {
            float targetVelocityX = directionalInput.x * movementSpeed;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,
                                          controller.collisions.below ? accelerationTimeGrounded : accelerationTimeAirborne);

            velocity.y += gravity * Time.deltaTime;
        }

        public void SetDirectionalInput(Vector2 input)
        {
            directionalInput = input;
        }

        public void OnJumpInputDown()
        {
            if (wallSliding)
            {
                if (inputDirX == wallDirX)
                {
                    velocity = new Vector2(-wallDirX * wallJumpClimb.x, wallJumpClimb.y);
                }

                else if (inputDirX == 0)
                {
                    velocity = new Vector2(-wallDirX * wallJumpOff.x, wallJumpOff.y);
                }

                else
                {
                    velocity = new Vector2(-wallDirX * wallLeapOff.x, wallLeapOff.y);
                }
            }

            if (controller.collisions.below)
            {
                velocity.y = maxJumpVelocity;
            }
        }

        public void OnJumpInputUp()
        {
            velocity.y = Mathf.Min(velocity.y, minJumpVelocity);
        }
    }
}

