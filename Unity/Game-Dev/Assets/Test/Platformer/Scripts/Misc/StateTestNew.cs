using UnityEngine;
using System;
using System.Collections.Generic;

namespace StateTestNew // cleaner code vs efficiency [lists and stacks vs arrays] also queue transitions for lateupdate maybe?
{
    public class StateMachine
    {
        private readonly Stack<StateInfo> mStateStack = new Stack<StateInfo>();
        private readonly List<StateInfo> mTempStateList = new List<StateInfo>();

        private readonly IDictionary<string, StateInfo> mStateDict =
            new Dictionary<string, StateInfo>();

        private string mCurrentStateName;

        public StateInfo CurrentStateInfo => mStateStack.Peek();
        public State CurrentState => CurrentStateInfo.state;

        private void SetupInitialStateStack()
        {
            StateInfo curStateInfo = mStateDict[mCurrentStateName];
 
            while (curStateInfo != null)
            {
                mTempStateList.Add(curStateInfo);
                curStateInfo = curStateInfo.parentStateInfo;
            }

            MoveTempList();
        }

        private StateInfo PrepareStatesToEnter(string destStateName)
        {
            StateInfo curStateInfo = mStateDict[destStateName];
            mTempStateList.Clear();

            do
            {
                mTempStateList.Add(curStateInfo);
                curStateInfo = curStateInfo.parentStateInfo;
            } while ((curStateInfo != null) && !curStateInfo.active);

            return curStateInfo;
        }

        private void MoveTempList()
        {
            for(int i = mTempStateList.Count - 1; i >= 0; i--)
            {
                mStateStack.Push(mTempStateList[i]);
            }
        }

        public void Tick()
        {
            foreach (StateInfo stateInfo in mStateStack)
            {
                stateInfo.state.Tick();
            }

            PerformTransitions();
        }

        public void PerformTransitions()
        {
            while (CurrentState.Name != mCurrentStateName)
            {
                StateInfo commonStateInfo = PrepareStatesToEnter(mCurrentStateName);

                while (mStateStack.Count > 0 && CurrentStateInfo != commonStateInfo)
                {
                    StateInfo curStateInfo = mStateStack.Pop();

                    curStateInfo.state.OnExit();
                    curStateInfo.active = false;
                }

                MoveTempList();

                foreach (StateInfo stateInfo in mStateStack)
                {
                    stateInfo.state.OnEnter();
                    stateInfo.active = true;
                }
            }           
        }

        public StateInfo AddState(params State[] states)
        {
            if (states.Length == 0)
            {
                return null;
            }

            StateInfo stateInfo = AddState(states[states.Length - 1]);

            StateInfo parentStateInfo;
            for (int i = states.Length - 2; i >= 0; i--)
            {
                parentStateInfo = mStateDict[states[i + 1].Name];

                stateInfo = AddState(states[i]);
                if ((stateInfo.parentStateInfo != null) &&
                    (stateInfo.parentStateInfo != parentStateInfo))
                {
                    throw new Exception("State already added.");
                }

                stateInfo.parentStateInfo = parentStateInfo;
            }

            return stateInfo;
        }

        public StateInfo AddState(State state)
        {
            StateInfo stateInfo;
            if (!mStateDict.TryGetValue(state.Name, out stateInfo))
            {
                stateInfo = new StateInfo
                {
                    state = state,
                    parentStateInfo = null,
                    active = false
                };

                mStateDict.Add(state.Name, stateInfo);
            }

            return stateInfo;
        }

        public void SetState(string name)
        {
            mCurrentStateName = name;
        }

        public void Start()
        {
            SetupInitialStateStack();
        }
    }

    public interface IState
    {
        string Name { get; }

        void Tick();

        void OnEnter();
        void OnExit();
    }

    public abstract class State : IState
    {
        protected readonly Entity entity;
        public string Name => GetType().Name;

        public abstract void Tick();

        public virtual void OnEnter()
        {

        }

        public virtual void OnExit()
        {

        }

        public State(Entity entity)
        {
            this.entity = entity;
        }
    }

    public class StateInfo
    {
        public State state;
        public StateInfo parentStateInfo;

        public bool active;

        public override string ToString()
        {
            return $"state: {state.Name}, active: {active}, parent: {parentStateInfo?.state.Name}";
        }
    }




    public class A : State
    {
        public override void Tick()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                entity.stateMachine.SetState("B");
            }
        }

        public override void OnEnter()
        {
            Debug.Log($"Entered: {Name}");
        }

        public override void OnExit()
        {
            Debug.Log($"Exited: {Name}");
        }

        public A(Entity entity) : base(entity)
        {

        }
    }

    public class AP : State
    {
        public override void Tick()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                entity.stateMachine.SetState("B");
            }
        }

        public override void OnEnter()
        {
            Debug.Log($"Entered: {Name}");
        }

        public override void OnExit()
        {
            Debug.Log($"Exited: {Name}");
        }

        public AP(Entity entity) : base(entity)
        {

        }
    }

    public class B : State
    {
        public override void Tick()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                entity.stateMachine.SetState("A");
            }
        }

        public override void OnEnter()
        {
            Debug.Log($"Entered: {Name}");
        }

        public override void OnExit()
        {
            Debug.Log($"Exited: {Name}");
        }

        public B(Entity entity) : base(entity)
        {

        }
    }

    public class BP : State
    {
        public override void Tick()
        {

        }

        public override void OnEnter()
        {
            Debug.Log($"Entered: {Name}");
        }

        public override void OnExit()
        {
            Debug.Log($"Exited: {Name}");
        }

        public BP(Entity entity) : base(entity)
        {

        }
    }
}