using UnityEngine;
using FSMRev3;

namespace PlatformerFSM
{
    public class Dashing : State<Player>
    {
        public Dashing(Player entity) : base(entity)
        {

        }

        public override void Tick()
        {
            if (entity.controller.collisions.Horizontal)
            {
                entity.animator.Play("falling");
                entity.velocity = new Vector2(-entity.facing * entity.wallJumpVector.x, entity.wallJumpVector.y);
                entity.StateMachine.SetState("Airborne");
            }

            else if (entity.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                entity.StateMachine.SetState(entity.controller.collisions.Below ? "Grounded" : "Airborne");
            }
        }

        public override void OnEnter()
        {
            entity.movementSpeed = 0;

            entity.LockFacing();
            entity.LockVelocity(new Vector2(entity.facing * 20, 0));

            entity.animator.Play("dashing");
        }

        public override void OnExit()
        {
            entity.LockFacing(false);
            entity.LockVelocity(false);
        }
    }
}
