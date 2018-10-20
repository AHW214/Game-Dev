using System;
using UnityEngine;

namespace FSM
{
    public class Jumping : State
    {
        public override Type TSuperstate => typeof(Airborne);
        public override string animName => "jumping";

        public Jumping(Player player) : base(player)
        {

        }

        public override void Tick()
        {
            if (player.velocity.y < 0)
            {
                player.currentState.SetState(new Falling(player));
            }
        }

        public override void OnStateEnter()
        {
            player.velocity.y = player.jumpVelocityRange[1];
            Debug.Log("Entered: Jumping");
            player.animator.Play(animName);
        }

        public override void OnStateExit()
        {
            Debug.Log("Exited: Jumping");
        }
    }
}
