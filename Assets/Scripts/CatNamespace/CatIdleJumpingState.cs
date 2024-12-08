using Cysharp.Threading.Tasks;
using DG.Tweening;
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
            cat.PlayIdleJumpAnimation();

            await UniTask.WaitForSeconds(gameConstants.idleJumpAnimationDelay);
            var remainingTime = gameConstants.idleJumpAnimationDuration - gameConstants.idleJumpAnimationDelay;

            var nextPosition = cat.transform.position;
            nextPosition += cat.transform.forward * gameConstants.idleJumpAnimationForwardRelocation;
            nextPosition += cat.transform.up * gameConstants.idleJumpAnimationUpRelocation;

            cat.GetRigidbody().isKinematic = true;
            cat.transform.DOJump(nextPosition, gameConstants.idleJumpAnimationPower, 1, remainingTime).SetEase(Ease.Linear);

            isJumping = true;
            await UniTask.WaitForSeconds(remainingTime);
            isJumping = false;

            cat.GetRigidbody().isKinematic = false;

            stateMachine.ChangeState(CatState.Locomotion);
        }

        public override bool Exit()
        {
            base.Exit();
            return !isJumping;
        }
    }
}