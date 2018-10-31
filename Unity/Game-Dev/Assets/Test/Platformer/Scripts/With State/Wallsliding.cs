using UnityEngine;
using FSMRev3;

namespace PlatformerFSM
{
    public class WallSliding : State<Player>, ICoreState
    {
        public string AnimName => "idle";
        public bool CollisionsEnabled => true;

        public WallSliding(Player player) : base(player)
        {

        }

        public override void Tick()
        {
            if (entity.controller.collisions[1][-1] != null)
            {
                entity.StateMachine.SetState("Idle");
            }

            else if (Input.GetKeyDown(KeyCode.Space))
            {
                entity.StateMachine.SetState("WallJumping");
            }

            else if (entity.input.x == 0 || entity.controller.collisions[0][entity.facing] == null)
            {
                entity.StateMachine.SetState("Falling");
            }

            else
            {
                entity.velocity.y = Mathf.Max(entity.velocity.y, -entity.maxWallslideSpeed);
            }           
        }

        public override void OnEnter()
        {
            Debug.Log("Entered: Wallsliding");
            entity.animator.Play(AnimName);
        }

        public override void OnExit()
        {
            Debug.Log("Exited: Wallsliding");
        }
    }
}
