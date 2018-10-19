using UnityEngine;

namespace FSM
{
    public class Jumping : State
    {
        public Jumping(Player player) : base(player)
        {
            TSuperstate = typeof(Airborne);
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
        }

        public override void OnStateExit()
        {
            Debug.Log("Exited: Jumping");
        }
    }
}
