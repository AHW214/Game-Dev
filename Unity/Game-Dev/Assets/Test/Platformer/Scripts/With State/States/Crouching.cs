using UnityEngine;
using FSMRev3;

namespace PlatformerFSM
{
    public class Crouching : State<Player>, ICoreState
    {
        public string AnimName => "crouching";
        public bool CollisionsEnabled => true;

        public Crouching(Player entity) : base(entity)
        {

        }

        public override void Tick()
        {
            if (entity.input.y >= 0)
            {
                entity.StateMachine.SetState("Idle");
            }

            else if (Input.GetKeyDown(KeyCode.E))
            {
                entity.velocity.x = entity.facing * 10;
                entity.StateMachine.SetState("Sliding");
            }
        }

        public override void OnEnter()
        {
            entity.movementSpeed = 0;
            entity.animator.Play(AnimName);          
        }

        public override void OnExit()
        {

        }
    }
}