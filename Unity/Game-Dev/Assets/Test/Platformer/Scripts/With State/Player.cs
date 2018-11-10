using System;
using UnityEngine;
using FSMRev3;

namespace PlatformerFSM
{
    public class Player : MonoBehaviour
    {
        public Vector2 jumpHeightRange = new Vector2(1, 4);
        public Vector2 wallJumpVector = new Vector2(3.75F, 11.0F);
        public float timeToJumpApex = 0.4F;
        public float maxWallslideSpeed = 0.7F;
        public float wallStickTime = 0.1F;
        public float accelerationTimeAirborne = 0.2F;
        public float accelerationTimeGrounded = 0.1F;
        public float normalSpeed = 6;
        public float runSpeed = 9;

        public bool LogState = false;

        internal bool facingLocked = false;

        internal int facing = 1;
        internal float movementSpeed = 6;
        internal float accelerationTime = 0;

        internal Vector2 input;
        internal Vector2 velocity;
        internal Vector2 displacement;

        internal Vector2 jumpVelocityRange;

        private float gravity;     
        private float velocityXSmoothing; // need to zero this value when externally zeroing the velocity x component (public property setter might work?)

        private SpriteRenderer spriteRenderer;
        internal Animator animator;
        internal Controller2D controller;

        public StateMachine<Player> StateMachine => new StateMachine<Player>();
                              
        private void Awake()
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

            if (!facingLocked)
            {
                FaceForward();
            }
            
            CalculateVelocity();

            StateMachine.Tick();

            displacement = velocity * Time.deltaTime;
            controller.DetectCollisions(displacement);

            if ((StateMachine.CurrentState as ICoreState).CollisionsEnabled) // need to change function call order to prevent "race conditions" (see Ascending Platform for example)
            {
                controller.HandleCollisions();
            }

            transform.Translate(displacement);                 
        }

        private void CalculateVelocity()
        {
            float targetVelocityX = input.x * movementSpeed;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTime);

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

