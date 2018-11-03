using UnityEngine;
using FSMRev3;

namespace PlatformerFSM
{
    public class DescendingPlatform : State<Player>, ICoreState
    {
        public string AnimName => "falling";
        public bool CollisionsEnabled => false;

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
                entity.StateMachine.SetState("Falling");
            }
        }

        public override void OnEnter()
        {
            Debug.Log("Entered: Descending Platform");

            platform = entity.controller.collisions[1][-1].Value.collider;

            entity.animator.Play(AnimName);
        }

        public override void OnExit()
        {
            Debug.Log("Exited: Descending Platform");
        }
    }
}
