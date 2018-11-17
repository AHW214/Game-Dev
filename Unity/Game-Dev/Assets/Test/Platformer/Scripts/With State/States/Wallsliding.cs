using UnityEngine;
using FSMRev3;

namespace PlatformerFSM
{
    public class WallSliding : State<Player>
    {
        private float timeToUnstick;
        private int wallFacing;

        public WallSliding(Player player) : base(player)
        {

        }

        public override void Tick()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                entity.velocity.y = Mathf.Min(entity.velocity.y, entity.jumpVelocityRange[0]);
            }

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

            else if (timeToUnstick > 0)
            {
                entity.velocity.y = Mathf.Max(entity.velocity.y, -entity.maxWallslideSpeed);

                timeToUnstick = (entity.input.x == 0 || entity.facing != wallFacing) 
                                ? timeToUnstick - Time.deltaTime : entity.wallStickTime;            
            }

            else
            {
                entity.StateMachine.SetState("Airborne");
            }
        }

        public override void OnEnter()
        {
            timeToUnstick = entity.wallStickTime;
            wallFacing = entity.facing;
            
            entity.LockVelocityX(wallFacing);
            entity.animator.Play("wallsliding");           
        }

        public override void OnExit()
        {
            entity.LockVelocityX(false);
        }
    }
}
