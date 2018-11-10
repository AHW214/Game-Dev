using UnityEngine;
using FSMRev3;

namespace PlatformerFSM
{
    public class Grounded : State<Player>
    {
        public Grounded(Player entity) : base(entity)
        {

        }

        public override void Tick()
        {
            if (!entity.controller.collisions.Below)
            {
                entity.StateMachine.SetState("Airborne");
            }

            else if (Input.GetKeyDown(KeyCode.Space))
            {
                entity.velocity.y = entity.jumpVelocityRange[1];
                entity.StateMachine.SetState("Airborne");
            }

            else if (entity.input.y < 0 && (entity.controller.collisions[1][-1]?.collider.CompareTag("One Way Platform") ?? false))
            {
                entity.StateMachine.SetState("DescendingPlatform");
            }          
        }

        public override void OnEnter()
        {
            entity.accelerationTime = entity.accelerationTimeGrounded;

            entity.StateMachine.SetState(entity.input.y < 0 ? "Crouching" : "Standing");           
        }

        public override void OnExit()
        {

        }
    }
}