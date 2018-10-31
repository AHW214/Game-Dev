namespace FSMRev3
{
    public interface IState
    {
        string Name { get; }

        void Tick();

        void OnEnter();
        void OnExit();
    }

    public abstract class State<T> : IState
    {
        protected readonly T entity;
        public string Name => GetType().Name;

        public abstract void Tick();

        public virtual void OnEnter()
        {

        }

        public virtual void OnExit()
        {

        }

        public State(T entity)
        {
            this.entity = entity;
        }
    }
}
