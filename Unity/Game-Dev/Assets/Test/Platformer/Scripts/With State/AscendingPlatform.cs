using System;
using UnityEngine;

namespace FSM
{
    public class AscendingPlatform : State
    {
        public override Type TSuperstate => typeof(Airborne);
        public override string AnimName => "jumping";

        private Collider2D platform;

        public AscendingPlatform(Player player) : base(player)
        {

        }

        protected override void CollisionHandler(int component, int dir, RaycastHit2D hit)
        {
            // maybe just have a bool condition for disabling "HandleCollisions()"
        }

        public override void Tick()
        {
            bool above = player.controller.collisions[1][1]?.collider.Equals(platform) ?? false;
            bool side = player.controller.collisions[0][player.facing]?.collider.Equals(platform) ?? false;

            if (!above && !side)
            {
                player.currentState.SetState(new Falling(player)); // need pushdown automata to resume Jumping state without actually jumping
            }
        }

        public override void OnStateEnter()
        {
            Debug.Log("Entered: Ascending Platform");

            platform = player.controller.collisions[1][1].Value.collider;

            player.animator.Play(AnimName);
        }

        public override void OnStateExit()
        {
            Debug.Log("Exited: Ascending Platform");
        }
    }
}
