using UnityEngine;
using FSMRev3;

namespace PlatformerFSM
{
    public class Walking : State<Player>, ICoreState
    {
        public string AnimName => "walking";
        public bool CollisionsEnabled => true;

        public Walking(Player entity) : base(entity)
        {

        }

        public override void Tick()
        {
            if (entity.input.y < 0)
            {
                entity.StateMachine.SetState("Crouching");
            }

            else if (entity.input.x == 0 || entity.controller.collisions.Horizontal)
            {
                entity.StateMachine.SetState("Idle");
            }

            else if (Input.GetKey(KeyCode.LeftShift))
            {
                entity.StateMachine.SetState("Running");
            }
        }

        public override void OnEnter()
        {
            entity.movementSpeed = entity.normalSpeed;
            entity.animator.Play(AnimName);
        }

        public override void OnExit()
        {

        }
    }
}
