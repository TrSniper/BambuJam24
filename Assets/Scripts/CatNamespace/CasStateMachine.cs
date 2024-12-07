using System.Collections.Generic;

namespace CatNamespace
{
    public class CatStateMachine
    {
        private CatStateBase currentState;
        private Dictionary<CatState, CatStateBase> states = new();

        public void RegisterState(CatState stateKey, CatStateBase state)
        {
            states[stateKey] = state;
        }

        public void ChangeState(CatState newState)
        {
            currentState?.Exit();
            currentState = states[newState];
            currentState.Enter();
        }

        public void Update()
        {
            currentState?.Update();
        }
    }
}