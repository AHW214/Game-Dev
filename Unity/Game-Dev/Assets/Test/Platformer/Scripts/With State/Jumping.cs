using UnityEngine;

namespace FSM
{
    public class Jumping : State
    {
        public Jumping(Player player) : base(player)
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
            player.velocity.y = player.jumpVelocityRange[1];
        }
    }
}
