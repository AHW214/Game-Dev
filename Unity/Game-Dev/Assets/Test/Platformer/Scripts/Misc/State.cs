using UnityEngine;
using System;
using System.Collections.Generic;

namespace StateTest
{
    public class StateMachine
    {
        private StateInfo[] mStateStack;
        private StateInfo[] mTempStateStack;

        private int mStateStackTopIndex = -1;
        private int mTempStateStackCount;

        private readonly IDictionary<State, StateInfo> mStateDict =
            new Dictionary<State, StateInfo>();

        private State mInitialState;

        public StateInfo CurrentStateInfo => mStateStack[mStateStackTopIndex];
        public State CurrentState => CurrentStateInfo.state;
        
        public void TransitionTo(State destState)
        {
            State _destState = null;
            while (destState != null)
            {
                _destState = destState;
                destState = null;

                StateInfo commonStateInfo = SetupTempStateStackWithStatesToEnter(_destState); //android devs with the clutch method names
                InvokeExitMethods(commonStateInfo);
                int stateStackEnteringIndex = MoveTempStateStackToStateStack();
                InvokeEnterMethods(stateStackEnteringIndex);
            }
        }

        private StateInfo SetupTempStateStackWithStatesToEnter(State destState)
        {
            mTempStateStackCount = 0;
            StateInfo curStateInfo = mStateDict[destState];

            do
            {
                mTempStateStack[mTempStateStackCount++] = curStateInfo;
                curStateInfo = curStateInfo.parentStateInfo;
            } while ((curStateInfo != null) && !curStateInfo.active);

            return curStateInfo;
        }

        private void SetupInitialStateStack()
        {
            StateInfo curStateInfo = mStateDict[mInitialState];

            for (mTempStateStackCount = 0; curStateInfo != null; mTempStateStackCount++)
            {
                mTempStateStack[mTempStateStackCount] = curStateInfo;
                curStateInfo = curStateInfo.parentStateInfo;
            }

            mStateStackTopIndex = -1;

            MoveTempStateStackToStateStack();
        }

        private int MoveTempStateStackToStateStack()
        {
            int startingIndex = mStateStackTopIndex + 1;
            int i = mTempStateStackCount - 1;
            int j = startingIndex;

            while (i >= 0)
            {
                mStateStack[j] = mTempStateStack[i];
                j++; i--;
            }

            mStateStackTopIndex = j - 1;

            return startingIndex;
        }

        public void Start()
        {
            int maxDepth = 0;
            foreach(StateInfo si in mStateDict.Values)
            {
                int depth = 0;
                for (StateInfo i = si; i != null; depth++)
                {
                    i = i.parentStateInfo;
                }

                if (maxDepth < depth)
                {
                    maxDepth = depth;
                }
            }

            mStateStack = new StateInfo[maxDepth];
            mTempStateStack = new StateInfo[maxDepth];

            SetupInitialStateStack();
        }

        private void InvokeExitMethods(StateInfo commonStateInfo)
        {
            while ((mStateStackTopIndex >= 0) &&
                    (mStateStack[mStateStackTopIndex] != commonStateInfo))
            {
                State curState = mStateStack[mStateStackTopIndex].state;

                curState.OnExit();
                mStateStack[mStateStackTopIndex--].active = false;
            }
        }

        private void InvokeEnterMethods(int stateStackEnteringIndex)
        {
            for (int i = stateStackEnteringIndex; i <= mStateStackTopIndex; i++)
            {
                mStateStack[i].state.OnEnter();
                mStateStack[i].active = true;
            }
        }

        public void Tick()
        {
            Tick(mStateStack.Length);
        }

        public void Tick(int count)
        {
            StateInfo si = CurrentStateInfo;

            for(int i = 0; si != null && i < count; i++)
            {
                si.state.Tick();
                si = si.parentStateInfo;
            }
        }

        public StateInfo AddState(State state)
        {
            return AddState(state, null);
        }

        public StateInfo AddState(State state, State parent)
        {
            StateInfo parentStateInfo = null;
            if (parent != null)
            {
                if (!mStateDict.TryGetValue(parent, out parentStateInfo))
                {
                    parentStateInfo = AddState(parent, null);
                }
            }

            StateInfo stateInfo;
            if (!mStateDict.TryGetValue(state, out stateInfo))
            {
                stateInfo = new StateInfo();
                mStateDict.Add(state, stateInfo);
            }

            if ((stateInfo.parentStateInfo != null) &&
                    (stateInfo.parentStateInfo != parentStateInfo))
            {
                throw new Exception("State already added.");
            }

            stateInfo.state = state;
            stateInfo.parentStateInfo = parentStateInfo;
            stateInfo.active = false;

            return stateInfo;
        }

        public void SetInitialState(State initialState)
        {
            mInitialState = initialState;
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

    public interface IState
    {
        string Name { get; }

        void Tick();

        void OnEnter();
        void OnExit();      
    }

    public abstract class State : IState, IEquatable<State>
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

        public override bool Equals(object obj)
        {
            return Equals(obj as State);
        }

        public bool Equals(State state)
        {
            if (ReferenceEquals(state, null))
            {
                return false;
            }

            if (ReferenceEquals(state, this))
            {
                return true;
            }

            return GetType() == state.GetType();
        }

        public override int GetHashCode()
        {
            return GetType().GetHashCode();
        }

        public static bool operator ==(State lhs, State rhs)
        {
            if (ReferenceEquals(lhs, null))
            {
                if (ReferenceEquals(rhs, null))
                {
                    return true;
                }

                return false;
            }

            return lhs.Equals(rhs);
        }

        public static bool operator !=(State lhs, State rhs)
        {
            return !(lhs == rhs);
        }
    }

    public class A : State
    {
        public override void Tick()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                entity.stateMachine.TransitionTo(new B(entity));
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

        public A(Entity entity): base(entity)
        {

        }
    }

    public class AP : State
    {
        public override void Tick()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                entity.stateMachine.TransitionTo(new B(entity));
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
                entity.stateMachine.TransitionTo(new A(entity));               
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