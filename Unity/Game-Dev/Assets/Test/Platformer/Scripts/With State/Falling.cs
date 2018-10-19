using UnityEngine;

namespace FSM
{
    public class Falling : State
    {
        public Falling(Player player) : base(player)
        {

        }

        protected override void CollisionHandler(int component, int dir, RaycastHit2D hit)
        {
            base.CollisionHandler(component, dir, hit);

            if (component == 1 && dir == -1)
            {
                player.SetState(new Idle(player));
            }
        }

        public override void Tick()
        {

        }

        public override void OnStateEnter()
        {
            Debug.Log("Entered: Falling");
        }

        public override void OnStateExit()
        {
            Debug.Log("Exited: Falling");
        }
    }
}
