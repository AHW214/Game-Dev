using UnityEngine;
using FSMRev3;

namespace PlatformerFSM
{
    public class Running : State<Player>
    {
        public Running(Player entity) : base(entity)
        {

        }

        public override void Tick()
        {
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                entity.StateMachine.SetState("Walking");
            }
        }

        public override void OnEnter()
        {
            entity.movementSpeed = entity.runSpeed;
            entity.animator.Play("running");
        }

        public override void OnExit()
        {
 
        }
    }
}
