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
            if (!entity.controller.collisions.Grounded)
            {
                entity.StateMachine.SetState("Falling");
            }

            else if (Input.GetKeyDown(KeyCode.Space))
            {
                entity.velocity.y = entity.jumpVelocityRange[1];
                entity.StateMachine.SetState("Rising");
            }

            else if (entity.input.y < 0 && (entity.controller.collisions[1][-1]?.collider.CompareTag("One Way Platform") ?? false))
            {
                entity.StateMachine.SetState("DescendingPlatform");
            }          
        }

        public override void OnEnter()
        {
            Debug.Log("Enter: Grounded");
        }

        public override void OnExit()
        {
            Debug.Log("Exited: Grounded");
        }
    }
}