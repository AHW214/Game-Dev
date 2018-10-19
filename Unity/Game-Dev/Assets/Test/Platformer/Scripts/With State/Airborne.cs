using UnityEngine;

namespace FSM
{
    public class Airborne : Superstate
    {
        public Airborne(Player player) : base(player)
        {

        }

        public Airborne(State state) : base(state)
        {

        }

        protected override State DetermineSubstate()
        {
            return new Jumping(player);
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            Debug.Log("Enter: Airborne");
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            Debug.Log("Exited: Airborne");
        }
    }
}
