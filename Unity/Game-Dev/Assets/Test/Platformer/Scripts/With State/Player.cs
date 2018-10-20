using System;
using UnityEngine;

namespace FSM
{
    public class Player : MonoBehaviour
    {
        public Vector2 jumpHeightRange = new Vector2(1, 4);
        public float timeToJumpApex = 0.4F;
        public float accelerationTimeAirborne = 0.2F;
        public float accelerationTimeGrounded = 0.1F;
        public float movementSpeed = 6;

        internal Vector2 input;
        internal Vector2 velocity;
        internal Vector2 displacement;

        internal Vector2 jumpVelocityRange;

        private float gravity;     
        private float velocityXSmoothing;

        private SpriteRenderer spriteRenderer;
        internal Animator animator;
        internal Controller2D controller;
        
        public Superstate currentState;
                              
        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();           
            controller = GetComponent<Controller2D>();

            gravity = -(2 * jumpHeightRange[1]) / Mathf.Pow(timeToJumpApex, 2);

            jumpVelocityRange[0] = Mathf.Sqrt(2 * Mathf.Abs(gravity) * jumpHeightRange[0]);
            jumpVelocityRange[1] = Mathf.Abs(gravity) * timeToJumpApex;
            
            SetState(new Idle(this));
        }

        private void Update()
        {
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            FaceForward();

            CalculateVelocity();

            displacement = velocity * Time.deltaTime;
            controller.DetectCollisions(displacement);

            currentState.HandleCollisions();
            currentState.Tick();           
        }

        public void SetState(Superstate state)
        {
            currentState?.OnStateExit();
            (currentState = state)?.OnStateEnter();
        }

        public void SetState(State state)
        {
            SetState((Superstate)Activator.CreateInstance(state.TSuperstate, state));
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

            if (sign == 1 && spriteRenderer.flipX)
            {
                spriteRenderer.flipX = false;
            }

            else if (sign == -1 && !spriteRenderer.flipX)
            {
                spriteRenderer.flipX = true;
            }
        }
    }
}

