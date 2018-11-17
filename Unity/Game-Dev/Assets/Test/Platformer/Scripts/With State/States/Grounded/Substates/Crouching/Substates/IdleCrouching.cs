using UnityEngine;
using FSMRev3;

namespace PlatformerFSM
{
    public class IdleCrouching : State<Player>
    {
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
            entity.animator.Play("crouching");          
        }

        public override void OnExit()
        {

        }
    }
}