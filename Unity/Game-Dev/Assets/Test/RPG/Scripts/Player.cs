using UnityEngine;

namespace Game
{
    public class Player : MovingObject
    {
        private AnimatorStateMachine stateMachine;
        private Vector2 input;

        private void Awake()
        {
            stateMachine = GetComponent<AnimatorStateMachine>();
            stateMachine.EnterStateAction = EnterStateAction;
            stateMachine.ContinueStateAction = ContinueStateAction;
        }

        private new void Start()
        {
            base.Start();    
        }

        private void Update()
        {
            ReadInput();
        }

        private void ReadInput()
        {
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (input.magnitude >= 0.1F)
            {
                heading = new Direction(input);

                Vector2 pos = 0.1F * Vector2.up + (Vector2)transform.position;
                Debug.DrawRay(pos, 7.0F * input.normalized, Color.red, Time.deltaTime);
                Debug.DrawRay(pos, 7.0F * heading.pixelDirection.normalized, Color.blue, Time.deltaTime);

                stateMachine.animator.SetFloat("FaceX", heading.pixelDirection.x);
                stateMachine.animator.SetFloat("FaceY", heading.pixelDirection.y);
            }
        }

        #region Animator State Machine Actions
        private void EnterStateAction(State state)
        {
            switch (state)
            {
                case State.Idle:
                    stateMachine.animator.Play(StateNames.IdleAnim);
                    break;
                case State.Walking:
                    stateMachine.animator.Play(StateNames.WalkAnim);
                    Move();
                    break;
                case State.Running:
                    stateMachine.animator.Play(StateNames.WalkAnim); // different anim later
                    stateMachine.animator.speed = 2.0F; // different anim later
                    Move(14.0F);
                    break;
            }
        }

        private void ContinueStateAction(State state)
        {
            switch (state)
            {
                case State.Idle:
                    if (input.magnitude >= 0.7F)
                    {
                        stateMachine.EnterState(Input.GetButton("B") ? State.Running : State.Walking);
                    }
                    break;
                case State.Walking:
                    if (input.magnitude >= 0.7F)
                    {
                        Move();

                        if (Input.GetButton("B"))
                        {
                            stateMachine.EnterState(State.Running);
                        }
                    }

                    else
                    {
                        stateMachine.EnterState(State.Idle);
                    }
                    break;
                case State.Running:
                    if (input.magnitude >= 0.7F)
                    {
                        Move(14.0F);

                        if (Input.GetButtonUp("B"))
                        {
                            stateMachine.animator.speed = 1.0F; // delete later
                            stateMachine.EnterState(State.Walking);
                        }
                    }

                    else
                    {
                        stateMachine.animator.speed = 1.0F; // delete later
                        stateMachine.EnterState(State.Idle);
                    }
                    break;
            }
        }
        #endregion
    }
}