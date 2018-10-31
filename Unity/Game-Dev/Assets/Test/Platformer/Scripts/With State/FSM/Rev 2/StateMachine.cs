using System;

namespace FSMRev2
{
    public class StateMachine
    {
        public _State state;

        public void SetState(CoreState _state)
        {
            state?.OnStateExit();

            _State outerState = _state;
            while (outerState?.TSuperstate != null)
            {
                outerState = (NestedState)Activator.CreateInstance(outerState.TSuperstate, outerState);
            }

            (state = outerState)?.OnStateEnter();
        }
    }
}
