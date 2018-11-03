using UnityEngine;
using FSMRev3;

namespace PlatformerFSM
{
    public class WallSliding : State<Player>, ICoreState
    {
        public string AnimName => "idle";
        public bool CollisionsEnabled => true;

        private float timeToUnstick;
        private int wallFacing;

        public WallSliding(Player player) : base(player)
        {

        }

        public override void Tick()
        {
            if (entity.controller.collisions[1][-1] != null)
            {
                entity.StateMachine.SetState("Idle");
            }

            else if (entity.controller.collisions[0][wallFacing] == null)
            {
                entity.StateMachine.SetState("Falling");
            }

            else if (Input.GetKeyDown(KeyCode.Space))
            {
                entity.velocity = new Vector2(-wallFacing * entity.wallJumpVector.x, entity.wallJumpVector.y);
                entity.StateMachine.SetState("Rising");
            }

            else
            {
                entity.velocity.y = Mathf.Max(entity.velocity.y, -entity.maxWallslideSpeed);

                if (timeToUnstick > 0)
                {
                    entity.velocity.x = wallFacing;

                    timeToUnstick = (entity.input.x == 0 || entity.facing != wallFacing) 
                                  ? timeToUnstick - Time.deltaTime : entity.wallStickTime;
                }

                else
                {
                    entity.StateMachine.SetState("Falling");
                }               
            }           
        }

        public override void OnEnter()
        {
            Debug.Log("Entered: Wallsliding");
            entity.animator.Play(AnimName);

            timeToUnstick = entity.wallStickTime;
            wallFacing = entity.facing;
        }

        public override void OnExit()
        {
            Debug.Log("Exited: Wallsliding");
        }
    }
}
