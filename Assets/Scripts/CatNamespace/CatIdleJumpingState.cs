﻿using Cysharp.Threading.Tasks;
using InspectorLogger;
using ScriptableObjects;

namespace CatNamespace
{
    public class CatIdleJumpingState : CatBaseState
    {
        public CatIdleJumpingState(Cat cat, GameConstants gameConstants, CatStateMachine stateMachine) : base(cat, gameConstants, stateMachine) { }

        private bool isJumping;

        public override async void Enter()
        {
            base.Enter();
            cat.Log("Entering Jumping State", LogStyles.StatePositive);
            cat.PlayRunJumpAnimation();

            isJumping = true;
            await UniTask.WaitForSeconds(gameConstants.idleJumpAnimationDuration);
            isJumping = false;

            stateMachine.ChangeState(CatState.Locomotion);
        }

        public override void Update()
        {
            base.Update();
        }

        public override bool Exit()
        {
            base.Exit();
            return !isJumping;
        }
    }
}