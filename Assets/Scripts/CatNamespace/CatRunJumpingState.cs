using Cysharp.Threading.Tasks;
using InspectorLogger;

namespace CatNamespace
{
    public class CatRunJumpingState : CatBaseState
    {
        public CatRunJumpingState(Cat cat, CatStateMachine stateMachine) : base(cat, stateMachine) { }

        private bool isJumping;

        public override async void Enter()
        {
            base.Enter();
            cat.Log("Entering Jumping State", LogStyles.StatePositive);
            cat.PlayRunJumpAnimation();

            isJumping = true;
            await UniTask.WaitForSeconds(Cat.RunJumpAnimationDuration);
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