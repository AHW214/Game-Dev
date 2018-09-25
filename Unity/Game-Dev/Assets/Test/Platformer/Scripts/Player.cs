using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    [RequireComponent(typeof(Controller2D))]
    public class Player : MonoBehaviour
    {
        public float jumpHeight = 4.0F;
        public float timeToJumpApex = 0.4F;
        public float accelerationTimeAirborne = 0.2F;
        public float accelerationTimeGrounded = 0.1F;
        public float movementSpeed = 6.0F;

        private float gravity;
        private float jumpSpeed;
        private float velocityXSmoothing;

        private Vector2 velocity = Vector2.zero;
        private Controller2D controller;

        private void Start()
        {
            controller = GetComponent<Controller2D>();

            gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
            jumpSpeed = Mathf.Abs(gravity) * timeToJumpApex;

            Debug.Log($"Gravity: {gravity}\nJump Speed: {jumpSpeed}");
        }

        private void Update()
        {
            if (controller.collisions.above || controller.collisions.below)
            {
                velocity.y = 0;
            }

            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below)
            {
                velocity.y = jumpSpeed;
            }

            float targetVelocityX = input.x * movementSpeed;

            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, 
                                          controller.collisions.below ? accelerationTimeGrounded : accelerationTimeAirborne);

            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);
        }
    }
}

