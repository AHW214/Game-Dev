using UnityEngine;
using FSMRev3;

namespace PlatformerFSM
{
    public class AscendingPlatform : State<Player>
    {
        private Collider2D platform;

        public AscendingPlatform(Player entity) : base(entity)
        {

        }

        public override void Tick()
        {
            bool above = entity.controller.collisions[1][1]?.collider.Equals(platform) ?? false;
            bool side = entity.controller.collisions[0][entity.facing]?.collider.Equals(platform) ?? false;

            if (!above && !side)
            {
                entity.StateMachine.SetState("Airborne");
            }
        }

        public override void OnEnter()
        {
            platform = entity.controller.collisions[1][1].Value.collider;

            entity.EnableCollisions(false);
            entity.animator.Play("jumping");
        }

        public override void OnExit()
        {
            entity.EnableCollisions();
        }
    }
}
