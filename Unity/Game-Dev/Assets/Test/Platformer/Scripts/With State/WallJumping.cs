using System;
using UnityEngine;

namespace FSM
{
    public class WallJumping : State
    {
        public override Type TSuperstate => typeof(Airborne);
        public override string AnimName => "jumping";

        public WallJumping(Player player) : base(player)
        {

        }

        public override void Tick()
        {
            if (player.controller.collisions[1][-1] != null)
            {
                player.SetState(new Idle(player));
            }

            else if (player.input.x != 0 && player.controller.collisions[0][player.facing] != null)
            {
                player.currentState.SetState(new WallSliding(player));
            }

            else if (player.velocity.y < 0)
            {
                player.currentState.SetState(new Falling(player));
            }
        }

        public override void OnStateEnter()
        {
            player.velocity = new Vector2(-player.facing * 10, player.jumpVelocityRange[0]); //jank
            Debug.Log("Entered: Jumping");
            player.animator.Play(AnimName);
        }

        public override void OnStateExit()
        {
            Debug.Log("Exited: Jumping");
        }
    }
}
