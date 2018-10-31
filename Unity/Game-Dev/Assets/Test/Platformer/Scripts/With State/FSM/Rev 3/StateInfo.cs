namespace FSMRev3
{
    public class StateInfo<T>
    {
        public State<T> state;
        public StateInfo<T> parentStateInfo;

        public bool active;

        public override string ToString()
        {
            return $"state: {state.Name}, active: {active}, parent: {parentStateInfo?.state.Name ?? "null"}";
        }
    }
}
