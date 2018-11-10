using UnityEngine;
using FSMRev3;

namespace PlatformerFSM
{
    public class Idle : State<Player>, ICoreState
    {
        public string AnimName => "idle";
        public bool CollisionsEnabled => true;

        public Idle(Player entity) : base(entity)
        {

        }

        public override void Tick()
        {
            if (entity.input.y < 0)
            {
                entity.StateMachine.SetState("Crouching");
            }

            else if (entity.input.x != 0 && !entity.controller.collisions.Horizontal)
            {
                entity.StateMachine.SetState("Walking");
            }          
        }

        public override void OnEnter()
        {
            entity.animator.Play(AnimName);
        }

        public override void OnExit()
        {

        }
    }
}