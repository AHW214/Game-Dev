using UnityEngine;
using FSMRev3;

namespace PlatformerFSM
{
    public class Sliding : State<Player>, ICoreState
    {
        public string AnimName => "sliding";
        public bool CollisionsEnabled => true;

        public Sliding(Player entity) : base(entity)
        {

        }

        public override void Tick()
        {
            Debug.Log(entity.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);

            if (entity.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                entity.StateMachine.SetState("Crouching");
            }
        }

        public override void OnEnter()
        {
            entity.animator.Play(AnimName);

            Debug.Log("Enter: Sliding");
        }

        public override void OnExit()
        {
            Debug.Log("Exited: Sliding");
        }
    }
}