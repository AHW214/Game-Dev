using UnityEngine;
using FSMRev3;

namespace PlatformerFSM
{
    public class IdleCrouching : State<Player>, ICoreState
    {
        public string AnimName => "crouching";
        public bool CollisionsEnabled => true;

        public IdleCrouching(Player entity) : base(entity)
        {

        }

        public override void Tick()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                entity.StateMachine.SetState("Sliding");
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