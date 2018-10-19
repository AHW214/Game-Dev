using UnityEngine;

namespace FSM
{
    public class Idle : State
    {
        public Idle(Player player) : base(player)
        {
            TSuperstate = typeof(Grounded);
        }

        public override void Tick()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                player.SetState(new Jumping(player));
            }
        }

        public override void OnStateEnter()
        {
            Debug.Log("Enter: Idle");
        }

        public override void OnStateExit()
        {
            Debug.Log("Exited: Idle");
        }
    }
}