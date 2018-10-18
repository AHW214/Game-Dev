using UnityEngine;

namespace FSM
{
    public class Idle : State
    {
        public Idle(Player player) : base(player)
        {

        }

        public override void Tick()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                player.SetState(new Jumping(player));
            }
        }
    }
}