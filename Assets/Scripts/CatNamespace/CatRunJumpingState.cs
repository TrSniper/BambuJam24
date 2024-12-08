using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using InspectorLogger;
using ScriptableObjects;
using UnityEngine;

namespace CatNamespace
{
    public class CatRunJumpingState : CatBaseState
    {
        public CatRunJumpingState(Cat cat, GameConstants gameConstants, CatStateMachine stateMachine) : base(cat, gameConstants, stateMachine) { }

        private bool isJumping;

        public override async void Enter()
        {
            base.Enter();
            cat.Log("Entering Jumping State", LogStyles.StatePositive);
            cat.PlayRunJumpAnimation();
            JumpExtra();

            isJumping = true;
            await UniTask.WaitForSeconds(gameConstants.runJumpAnimationDuration);
            isJumping = false;

            stateMachine.ChangeState(CatState.Locomotion);
        }

        private async UniTaskVoid JumpExtra()
        {
            await UniTask.WaitForSeconds(gameConstants.runJumpAnimationForceDelay);
            cat.GetRigidbody().AddForce(gameConstants.runJumpAnimationForwardForce * cat.transform.forward, ForceMode.Impulse);
            cat.GetRigidbody().AddForce(gameConstants.runJumpAnimationUpForce * cat.transform.up, ForceMode.Impulse);
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