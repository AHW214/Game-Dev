using System;
using UnityEngine;

namespace FSM
{
    public class Falling : State
    {
        public override Type TSuperstate => typeof(Airborne);
        public override string animName => "falling";

        public Falling(Player player) : base(player)
        {

        }

        public override void Tick()
        {
            if (player.input.x != 0 && player.controller.collisions[0][player.facing] != null)
            {
                player.currentState.SetState(new WallSliding(player));
            }
        }

        public override void OnStateEnter()
        {
            Debug.Log("Entered: Falling");
            player.animator.Play(animName);
        }

        public override void OnStateExit()
        {
            Debug.Log("Exited: Falling");
        }
    }
}
