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
            if (Mathf.Abs(entity.displacement.x) < 1E-3)
            {
                entity.StateMachine.SetState("Idle");
            }
        }

        public override void OnEnter()
        {
            Debug.Log("Enter: Walking");
            entity.animator.Play(AnimName);
        }

        public override void OnExit()
        {
            Debug.Log("Exited: Walking");
        }
    }
}
