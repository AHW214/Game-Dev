using UnityEngine;
using FSMRev3;

namespace PlatformerFSM
{
    public class Crouching : State<Player>
    {
        public Crouching(Player entity) : base(entity)
        {

        }

        public override void Tick()
        {
            if (entity.input.y >= 0)
            {
                entity.StateMachine.SetState("Standing");
            }
        }

        public override void OnEnter()
        {
            entity.movementSpeed = 0;

            entity.StateMachine.SetState("IdleCrouching");
        }

        public override void OnExit()
        {

        }
    }
}