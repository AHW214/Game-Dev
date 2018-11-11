using UnityEngine;
using FSMRev3;

namespace PlatformerFSM
{
    public class OneWayPlatform : State<Player>
    {
        private Collider2D platform;

        public OneWayPlatform(Player entity) : base(entity)
        {

        }

        public override void Tick()
        {           
            if (!entity.controller.collisions.Contains(platform))
            {
                entity.StateMachine.SetState("Airborne");
            }           
        }

        public override void OnEnter()
        {
            platform = entity.controller.collisions[1][(int)Mathf.Sign(entity.velocity.y)].Value.collider;

            entity.EnableCollisions(false);
        }

        public override void OnExit()
        {
            entity.EnableCollisions();
        }
    }
}
