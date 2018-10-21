using System;
using UnityEngine;

namespace FSM
{
    public class WallSliding : State
    {
        public override Type TSuperstate => typeof(Airborne);
        public override string AnimName => "idle";

        public WallSliding(Player player) : base(player)
        {

        }

        public override void Tick()
        {
            if (player.controller.collisions[1][-1] != null)
            {
                player.SetState(new Idle(player));
            }

            else if (Input.GetKeyDown(KeyCode.Space))
            {
                player.currentState.SetState(new WallJumping(player));
            }

            else if (player.input.x == 0 || player.controller.collisions[0][player.facing] == null)
            {
                player.currentState.SetState(new Falling(player));
            }

            else
            {
                player.velocity.y = Mathf.Max(player.velocity.y, -player.maxWallslideSpeed);
            }           
        }

        public override void OnStateEnter()
        {
            Debug.Log("Entered: Wallsliding");
            player.animator.Play(AnimName);
        }

        public override void OnStateExit()
        {
            Debug.Log("Exited: Wallsliding");
        }
    }
}
