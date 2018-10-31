using System;
using UnityEngine;
using FSMRev3;

namespace PlatformerFSM
{
    public class Player : MonoBehaviour
    {
        public Vector2 jumpHeightRange = new Vector2(1, 4);
        public float timeToJumpApex = 0.4F;
        public float maxWallslideSpeed = 0.7F;
        public float accelerationTimeAirborne = 0.2F;
        public float accelerationTimeGrounded = 0.1F;
        public float movementSpeed = 6;

        internal int facing = 1;

        internal Vector2 input;
        internal Vector2 velocity;
        internal Vector2 displacement;

        internal Vector2 jumpVelocityRange;

        private float gravity;     
        private float velocityXSmoothing;

        private SpriteRenderer spriteRenderer;
        internal Animator animator;
        internal Controller2D controller;

        public StateMachine<Player> StateMachine { get; private set; } = new StateMachine<Player>();
                              
        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();           
            controller = GetComponent<Controller2D>();

            gravity = -(2 * jumpHeightRange[1]) / Mathf.Pow(timeToJumpApex, 2);

            jumpVelocityRange[0] = Mathf.Sqrt(2 * Mathf.Abs(gravity) * jumpHeightRange[0]);
            jumpVelocityRange[1] = Mathf.Abs(gravity) * timeToJumpApex;

            StateMachine.Initialize(this);
        }

        private void Update()
        {
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            FaceForward();
            CalculateVelocity();

            displacement = velocity * Time.deltaTime;
            controller.DetectCollisions(displacement);

            if ((StateMachine.CurrentState as ICoreState).CollisionsEnabled)
            {
                controller.HandleCollisions();
            }

            transform.Translate(displacement);

            StateMachine.Tick();           
        }

        private void CalculateVelocity()
        {
            float targetVelocityX = input.x * movementSpeed;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,
                                          controller.collisions.Grounded ? accelerationTimeGrounded : accelerationTimeAirborne);

            velocity.y += gravity * Time.deltaTime;
        }

        private void FaceForward()
        {
            int sign = Math.Sign(input.x);

            if (sign == -facing)
            {
                facing = sign;
                spriteRenderer.flipX = !spriteRenderer.flipX;
            }
        }
    }
}

