using FSMRev3;

namespace PlatformerFSM
{
    public class IdleStanding : State<Player>, ICoreState
    {
        public string AnimName => "idle";
        public bool CollisionsEnabled => true;

        public IdleStanding(Player entity) : base(entity)
        {

        }

        public override void Tick()
        {
            if (entity.input.x != 0 && !entity.controller.collisions.Horizontal)
            {
                entity.StateMachine.SetState("Moving");
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