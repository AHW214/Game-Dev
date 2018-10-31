using PlatformerFSM;

namespace FSMRev1
{
    /*
    public abstract class Superstate<Substate> : Superstate
        where Substate : State, new()
    {

    }
    */

    public abstract class Superstate
    {
        public Player player;
        protected State substate;

        protected abstract State DetermineSubstate();

        public virtual void Tick()
        {
            substate.Tick();
        }

        public virtual void OnStateEnter()
        {
            substate.OnStateEnter();
        }

        public virtual void OnStateExit()
        {
            substate.OnStateExit();
        }

        public void SetState(State state)
        {
            substate.OnStateExit();
            (substate = state).OnStateEnter();
        }

        public void HandleCollisions()
        {
            substate.HandleCollisions();
        }

        public Superstate(Player _player)
        {
            player = _player;
            substate = DetermineSubstate();
        }

        public Superstate(State state)
        {
            player = state.player;
            substate = state;
        }
    }
}
