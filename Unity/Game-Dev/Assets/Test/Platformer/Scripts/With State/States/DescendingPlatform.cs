using UnityEngine;
using FSMRev3;

namespace PlatformerFSM
{
    public class DescendingPlatform : State<Player>
    {
        private Collider2D platform;

        public DescendingPlatform(Player entity) : base(entity)
        {

        }

        public override void Tick()
        {
            bool below = entity.controller.collisions[1][-1]?.collider.Equals(platform) ?? false;
            bool side = entity.controller.collisions[0][entity.facing]?.collider.Equals(platform) ?? false;

            if (!below && !side)
            {
                entity.StateMachine.SetState("Airborne");
            }
        }

        public override void OnEnter()
        {
            platform = entity.controller.collisions[1][-1].Value.collider;

            entity.EnableCollisions(false);
            entity.animator.Play("falling");
        }

        public override void OnExit()
        {
            entity.EnableCollisions();
        }
    }
}
