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

        internal int facing = 1;
        internal float movementSpeed = 6;
        internal float accelerationTime = 0;

        internal Vector2 input;
        internal Vector2 velocity;
        internal Vector2 displacement;

        internal Vector2 jumpVelocityRange;

        internal readonly bool[] velocityLocked = { false, false };
        private bool collisionsEnabled = true;       
        private bool facingLocked = false;

        private float gravity;     
        private float velocityXSmoothing; // need to zero this value when externally zeroing the velocity x component (public property setter might work?)

        private SpriteRenderer spriteRenderer;
        internal Animator animator;
        internal Controller2D controller;

        public StateMachine<Player> StateMachine { get; } = new StateMachine<Player>();
                              
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
         
            displacement = velocity * Time.deltaTime;
            controller.DetectCollisions(displacement);

            StateMachine.Tick();

            if (collisionsEnabled) 
            {
                controller.HandleCollisions();
            }

            transform.Translate(displacement);                 
        }

        private void CalculateVelocity()
        {
            if (!velocityLocked[0])
            {
                float targetVelocityX = input.x * movementSpeed;
                velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTime);
            }
            
            if (!velocityLocked[1])
            {
                velocity.y += gravity * Time.deltaTime;
            }           
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

        public void LockVelocity(bool value = true) => velocityLocked[0] = velocityLocked[1] = value;
        public void LockVelocity(Vector2 value) { LockVelocity(); velocity = value; }

        public void LockVelocityX(bool value = true) => velocityLocked[0] = value;
        public void LockVelocityY(bool value = true) => velocityLocked[1] = value;

        public void LockVelocityX(float value) { LockVelocityX(); velocity.x = value; }
        public void LockVelocityY(float value) { LockVelocityY(); velocity.y = value; }

        public void EnableCollisions(bool value = true) => collisionsEnabled = value;
        public void LockFacing(bool value = true) => facingLocked = value;     
    }
}

