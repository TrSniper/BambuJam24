﻿using InspectorLogger;
using ScriptableObjects;

namespace CatNamespace
{
    public class CatLocomotionState : CatBaseState
    {
        public CatLocomotionState(Cat cat, GameConstants gameConstants, CatStateMachine stateMachine) : base(cat, gameConstants, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            cat.Log("Entering Locomotion State", LogStyles.StatePositive);
        }

        public override void Update()
        {
            base.Update();

            if (cat.IsInteractKeyDown() && cat.CanInteract()) stateMachine.ChangeState(CatState.Interacting);
            else if (cat.IsEatKeyDown() && cat.CanEat()) stateMachine.ChangeState(CatState.Eating);
            else if (cat.IsJumpKeyDown() && cat.GetCurrentSpeed() == 0f) stateMachine.ChangeState(CatState.IdleJumping);
            else if (cat.IsJumpKeyDown()) stateMachine.ChangeState(CatState.RunJumping);
        }

        public override bool Exit()
        {
            base.Exit();
            cat.Log("Exiting Locomotion State", LogStyles.StateNegative);
            return true;
        }
    }
}