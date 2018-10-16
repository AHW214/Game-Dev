using UnityEngine;

namespace FSM
{
    public class Player : MonoBehaviour
    {
        public float gravity = -9.81F;

        internal Vector2 input;
        internal Vector2 velocity;

        private Controller2D controller;
        private State currentState;
                              
        private void Start()
        {
            controller = GetComponent<Controller2D>();
            //SetState(new Idle(this));
        }

        private void Update()
        {
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            Vector2 displacement = Time.deltaTime * input;
            controller.HandleCollisions(ref displacement);

            transform.Translate(displacement);

            //currentState.Tick();
        }

        public void SetState(State state)
        {
            currentState?.OnStateExit();
            (currentState = state)?.OnStateEnter();
        }
    }
}

