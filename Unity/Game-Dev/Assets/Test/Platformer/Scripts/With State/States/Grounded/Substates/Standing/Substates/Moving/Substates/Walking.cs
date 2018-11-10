using UnityEngine;
using FSMRev3;

namespace PlatformerFSM
{
    public class Walking : State<Player>, ICoreState
    {
        public string AnimName => "walking";
        public bool CollisionsEnabled => true;

        public Walking(Player entity) : base(entity)
        {

        }

        public override void Tick()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                entity.StateMachine.SetState("Running");
            }
        }

        public override void OnEnter()
        {
            entity.movementSpeed = entity.normalSpeed;
            entity.animator.Play(AnimName);
        }

        public override void OnExit()
        {

        }
    }
}
