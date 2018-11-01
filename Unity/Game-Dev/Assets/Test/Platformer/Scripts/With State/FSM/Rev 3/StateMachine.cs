using System;
using System.Collections.Generic;

namespace FSMRev3
{
    public class StateMachine<T>
    {
        private readonly Stack<StateInfo<T>> mStateStack = new Stack<StateInfo<T>>();
        private readonly Stack<StateInfo<T>> mTempStateStack = new Stack<StateInfo<T>>();

        private readonly IDictionary<string, StateInfo<T>> mStateDict =
            new Dictionary<string, StateInfo<T>>();

        private string mCurrentStateName;

        public StateInfo<T> CurrentStateInfo => mStateStack.Peek();
        public State<T> CurrentState => CurrentStateInfo.state;

        private void SetupInitialStateStack()
        {
            StateInfo<T> curStateInfo = mStateDict[mCurrentStateName];
 
            while (curStateInfo != null)
            {
                curStateInfo.active = true;
                mTempStateStack.Push(curStateInfo);
                curStateInfo = curStateInfo.parentStateInfo;
            }

            MoveTempStack();
        }

        private StateInfo<T> PrepareStatesToEnter(string destStateName)
        {
            StateInfo<T> curStateInfo = mStateDict[destStateName];
            mTempStateStack.Clear();

            do
            {
                mTempStateStack.Push(curStateInfo);
                curStateInfo = curStateInfo.parentStateInfo;
            } while ((curStateInfo != null) && !curStateInfo.active);

            return curStateInfo;
        }

        private void MoveTempStack()
        {
            foreach(StateInfo<T> stateInfo in mTempStateStack)
            {
                mStateStack.Push(stateInfo);
            }
        }

        public void Tick()
        {
            foreach (StateInfo<T> stateInfo in mStateStack)
            {
                stateInfo.state.Tick();
            }

            PerformTransitions();
        }

        public void PerformTransitions()
        {
            while (CurrentState.Name != mCurrentStateName)
            {
                StateInfo<T> commonStateInfo = PrepareStatesToEnter(mCurrentStateName);

                while (mStateStack.Count > 0 && CurrentStateInfo != commonStateInfo)
                {
                    StateInfo<T> curStateInfo = mStateStack.Pop();

                    curStateInfo.state.OnExit();
                    curStateInfo.active = false;
                }

                foreach (StateInfo<T> stateInfo in mTempStateStack)
                {
                    stateInfo.state.OnEnter();
                    stateInfo.active = true;
                }

                MoveTempStack();             
            }           
        }

        public StateInfo<T>[] AddState(State<T> parent, params State<T>[] children)
        {
            StateInfo<T> parentStateInfo = AddState(parent);

            StateInfo<T>[] stateInfos = new StateInfo<T>[children.Length];
            for (int i = 0; i < stateInfos.Length; i++)
            {
                stateInfos[i] = AddState(children[i]);
                if ((stateInfos[i].parentStateInfo != null) &&
                    (stateInfos[i].parentStateInfo != parentStateInfo))
                {
                    throw new Exception("State already added.");
                }

                stateInfos[i].parentStateInfo = parentStateInfo;
            }

            return stateInfos;
        }

        public StateInfo<T> AddState(State<T> state)
        {
            StateInfo<T> stateInfo;
            if (!mStateDict.TryGetValue(state.Name, out stateInfo))
            {
                stateInfo = new StateInfo<T>
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
}