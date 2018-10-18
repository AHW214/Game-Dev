using UnityEngine;

namespace FSM
{
    public class Player : MonoBehaviour
    {
        public float gravity = -9.81F;

        internal Vector2 input;
        internal Vector2 velocity;
        internal Vector2 displacement;

        internal Controller2D controller;
        private State currentState;
                              
        private void Start()
        {
            controller = GetComponent<Controller2D>();
            SetState(new Idle(this));
        }

        private void Update()
        {
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            velocity.x = input.x;
            velocity.y += gravity * Time.deltaTime;

            displacement = velocity * Time.deltaTime;

            controller.DetectCollisions(displacement);

            currentState.Tick();

            transform.Translate(displacement);            
        }

        public void SetState(State state)
        {
            currentState?.OnStateExit();
            (currentState = state)?.OnStateEnter();
        }
    }
}

