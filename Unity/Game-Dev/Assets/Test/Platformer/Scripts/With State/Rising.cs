using UnityEngine;
using FSMRev3;

namespace PlatformerFSM
{
    public class Rising : State<Player>, ICoreState
    {
        public string AnimName => "jumping";
        public bool CollisionsEnabled => true;

        public Rising(Player entity) : base(entity)
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
            Debug.Log("Entered: Rising");
            entity.animator.Play(AnimName);
        }

        public override void OnExit()
        {
            Debug.Log("Exited: Rising");
        }
    }
}
