using UnityEngine;
using FSMRev3;

namespace PlatformerFSM
{
    public class Running : State<Player>, ICoreState
    {
        public string AnimName => "running";
        public bool CollisionsEnabled => true;

        public Running(Player entity) : base(entity)
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

            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                entity.StateMachine.SetState("Walking");
            }
        }

        public override void OnEnter()
        {
            entity.movementSpeed = entity.runSpeed;
            entity.animator.Play(AnimName);
        }

        public override void OnExit()
        {
 
        }
    }
}
