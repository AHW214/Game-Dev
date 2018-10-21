using System;
using UnityEngine;

namespace FSM
{
    public class DescendingPlatform : State
    {
        public override Type TSuperstate => typeof(Airborne);
        public override string AnimName => "falling";

        private Collider2D platform;

        public DescendingPlatform(Player player) : base(player)
        {

        }

        protected override void CollisionHandler(int component, int dir, RaycastHit2D hit)
        {
            // maybe just have a bool condition for disabling "HandleCollisions()"
        }

        public override void Tick()
        {
            bool below = player.controller.collisions[1][-1]?.collider.Equals(platform) ?? false;
            bool side = player.controller.collisions[0][player.facing]?.collider.Equals(platform) ?? false;

            if (!below && !side)
            {
                player.currentState.SetState(new Falling(player));
            }
        }

        public override void OnStateEnter()
        {
            Debug.Log("Entered: Descending Platform");

            platform = player.controller.collisions[1][-1].Value.collider;

            player.animator.Play(AnimName);
        }

        public override void OnStateExit()
        {
            Debug.Log("Exited: Descending Platform");
        }
    }
}
