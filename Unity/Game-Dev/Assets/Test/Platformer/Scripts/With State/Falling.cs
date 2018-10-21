using System;
using UnityEngine;

namespace FSM
{
    public class Falling : State
    {
        public override Type TSuperstate => typeof(Airborne);
        public override string AnimName => "falling";

        private Collider2D platform;

        public Falling(Player player) : base(player)
        {

        }

        public override void Tick()
        {
            if ((platform = player.controller.collisions[1][-1]?.collider) != null)
            {
                if (player.input.y < 0 && platform.CompareTag("One Way Platform"))
                {
                    player.currentState.SetState(new DescendingPlatform(player));
                }

                else
                {
                    player.SetState(new Idle(player));
                }              
            }

            else if (player.input.x != 0 && player.controller.collisions[0][player.facing] != null)
            {
                player.currentState.SetState(new WallSliding(player));
            }
        }

        public override void OnStateEnter()
        {
            Debug.Log("Entered: Falling");
            player.animator.Play(AnimName);
        }

        public override void OnStateExit()
        {
            Debug.Log("Exited: Falling");
        }
    }
}
