using UnityEngine;
using FSMRev3;

namespace PlatformerFSM
{
    public class Falling : State<Player>, ICoreState
    {
        public string AnimName => "falling";
        public bool CollisionsEnabled => true;

        private Collider2D platform;

        public Falling(Player entity) : base(entity)
        {

        }

        public override void Tick()
        {
            if ((platform = entity.controller.collisions[1][-1]?.collider) != null)
            {
                if (entity.input.y < 0 && platform.CompareTag("One Way Platform"))
                {
                    entity.StateMachine.SetState("DescendingPlatform");
                }

                else
                {
                    entity.StateMachine.SetState("Idle");
                }              
            }           
        }

        public override void OnEnter()
        {
            Debug.Log("Entered: Falling");
            entity.animator.Play(AnimName);
        }

        public override void OnExit()
        {
            Debug.Log("Exited: Falling");
        }
    }
}
