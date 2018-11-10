using UnityEngine;
using FSMRev3;

namespace PlatformerFSM
{
    public class Dashing : State<Player>, ICoreState
    {
        public string AnimName => "dashing";
        public bool CollisionsEnabled => true;

        public Dashing(Player entity) : base(entity)
        {

        }

        public override void Tick()
        {
            entity.velocity.y = 0;
            entity.velocity.x = entity.facing * 20;

            if (entity.controller.collisions.Horizontal)
            {
                entity.animator.Play("falling");
                entity.velocity = new Vector2(-entity.facing * entity.wallJumpVector.x, entity.wallJumpVector.y);
                entity.StateMachine.SetState("Airborne");
            }

            if (entity.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                entity.StateMachine.SetState(entity.controller.collisions.Below ? "Grounded" : "Airborne");
            }
        }

        public override void OnEnter()
        {
            entity.movementSpeed = 0;
            entity.facingLocked = true;

            entity.animator.Play(AnimName);
        }

        public override void OnExit()
        {
            entity.facingLocked = false;
        }
    }
}
