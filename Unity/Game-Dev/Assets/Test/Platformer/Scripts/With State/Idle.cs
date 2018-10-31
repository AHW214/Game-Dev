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
            if (Mathf.Abs(entity.displacement.x) > 1E-3)
            {
                entity.StateMachine.SetState("Walking");
            }
        }

        public override void OnEnter()
        {
            Debug.Log("Enter: Idle");
            entity.animator.Play(AnimName);
        }

        public override void OnExit()
        {
            Debug.Log("Exited: Idle");
        }
    }
}