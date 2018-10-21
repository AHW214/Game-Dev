using System;
using UnityEngine;

namespace FSM
{
    public class Idle : State
    {
        public override Type TSuperstate => typeof(Grounded);
        public override string AnimName => "idle";

        public Idle(Player player) : base(player)
        {

        }

        public override void Tick()
        {
            if (Mathf.Abs(player.displacement.x) > 1E-3)
            {
                player.currentState.SetState(new Walking(player));
            }
        }

        public override void OnStateEnter()
        {
            Debug.Log("Enter: Idle");
            player.animator.Play(AnimName);
        }

        public override void OnStateExit()
        {
            Debug.Log("Exited: Idle");
        }
    }
}