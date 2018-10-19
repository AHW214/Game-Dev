using UnityEngine;

namespace FSM
{
    public class Grounded : Superstate
    {
        public Grounded(Player player) : base(player)
        {

        }

        public Grounded(State state) : base(state)
        {

        }

        protected override State DetermineSubstate()
        {
            return new Idle(player);
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            Debug.Log("Enter: Grounded");
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            Debug.Log("Exited: Grounded");
        }
    }
}