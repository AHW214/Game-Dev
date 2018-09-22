using UnityEngine;
using System;

namespace Game
{ 
    public class AnimatorStateMachine : MonoBehaviour
    {
        private float stateStartTime;

        [HideInInspector]
        public Animator animator;

        public State state;
        
        public Action<State> EnterStateAction = null;
        public Action<State> ContinueStateAction = null;

        public float CurrentStateTime
        {
            get { return Time.time - stateStartTime; }
        }

	    private void Start()
        {
            if(EnterStateAction == null || ContinueStateAction == null)
            {
                throw new Exception("Enter and continue state actions must be assigned.");
            }

            animator = GetComponent<Animator>();
	    }
	
	    private void Update()
        {
            ContinueState();
	    }

        public void UpdateState(State state)
        {
            if (this.state == state)
            {
                return;
            }

            EnterState(state);
        }

        public void EnterState(State state)
        {
            EnterStateAction(state);

            this.state = state;
            stateStartTime = Time.time;
        }

        private void ContinueState()
        {
            ContinueStateAction(state);
        }
    }
}
