using UnityEngine;
using FSMRev3;

namespace PlatformerFSM
{
    public class WallSliding : State<Player>, ICoreState
    {
        public string AnimName => "wallsliding";
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
                entity.StateMachine.SetState("Grounded");
            }

            else if (entity.controller.collisions[0][wallFacing] == null)
            {
                entity.StateMachine.SetState("Airborne");
            }

            else if (Input.GetKeyDown(KeyCode.Space))
            {
                entity.velocity = new Vector2(-wallFacing * entity.wallJumpVector.x, entity.wallJumpVector.y);
                entity.StateMachine.SetState("Airborne");
            }

            else
            {
                entity.velocity.y = Mathf.Max(entity.velocity.y, -entity.maxWallslideSpeed);

                if (timeToUnstick > 0)
                {
                    entity.velocity.x = wallFacing; //fix

                    timeToUnstick = (entity.input.x == 0 || entity.facing != wallFacing) 
                                  ? timeToUnstick - Time.deltaTime : entity.wallStickTime;
                }

                else
                {
                    entity.StateMachine.SetState("Airborne");
                }               
            }           
        }

        public override void OnEnter()
        {
            entity.animator.Play(AnimName);

            timeToUnstick = entity.wallStickTime;
            wallFacing = entity.facing;
        }

        public override void OnExit()
        {

        }
    }
}
