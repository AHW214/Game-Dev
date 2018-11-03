using UnityEngine;
using FSMRev3;

namespace PlatformerFSM
{
    public class Jumping : State<Player>, ICoreState
    {
        public string AnimName => "jumping";
        public bool CollisionsEnabled => true;

        public Jumping(Player entity) : base(entity)
        {

        }

        public override void Tick()
        {
            if (entity.controller.collisions[1][1]?.collider.CompareTag("One Way Platform") ?? false)
            {
                entity.StateMachine.SetState("AscendingPlatform");
            }

            else if (entity.velocity.y < 0)
            {
                entity.StateMachine.SetState("Falling");
            }
        }

        public override void OnEnter()
        {
            entity.velocity.y = entity.jumpVelocityRange[1];
            Debug.Log("Entered: Jumping");
            entity.animator.Play(AnimName);
        }

        public override void OnExit()
        {
            Debug.Log("Exited: Jumping");
        }
    }
}
