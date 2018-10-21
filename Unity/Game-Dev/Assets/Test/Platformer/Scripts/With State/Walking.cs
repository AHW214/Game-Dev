using System;
using UnityEngine;

namespace FSM
{
    public class Walking : State
    {
        public override Type TSuperstate => typeof(Grounded);
        public override string AnimName => "walking";

        public Walking(Player player) : base(player)
        {

        }

        public override void Tick()
        {
            if (Mathf.Abs(player.displacement.x) < 1E-3)
            {
                player.currentState.SetState(new Idle(player));
            }
        }

        public override void OnStateEnter()
        {
            Debug.Log("Enter: Walking");
            player.animator.Play(AnimName);
        }

        public override void OnStateExit()
        {
            Debug.Log("Exited: Walking");
        }
    }
}
