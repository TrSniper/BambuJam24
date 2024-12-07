using System.Collections.Generic;

namespace CatNamespace
{
    public class CatStateMachine
    {
        private Cat cat;
        private CatBaseState currentState;
        private Dictionary<CatState, CatBaseState> states = new();

        public CatStateMachine(Cat cat)
        {
            this.cat = cat;
        }

        public void RegisterState(CatState stateKey, CatBaseState state)
        {
            states[stateKey] = state;
        }

        public void ChangeState(CatState newState)
        {
            if (currentState != null) if (!currentState.Exit()) return;

            cat.StateMachineOnly_SetCurrentState(newState);
            currentState = states[newState];
            currentState.Enter();
        }

        public void Update()
        {
            currentState?.Update();
        }
    }
}