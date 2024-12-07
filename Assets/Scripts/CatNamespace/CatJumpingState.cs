using Cysharp.Threading.Tasks;
using InspectorLogger;

namespace CatNamespace
{
    public class CatJumpingState : CatBaseState
    {
        public CatJumpingState(Cat cat, CatStateMachine stateMachine) : base(cat, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            cat.Log("Entering Jumping State", LogStyles.StatePositive);
            cat.PlayRunJumpAnimation();
        }

        public override async void Update()
        {
            base.Update();
            await UniTask.WaitForSeconds(Cat.JumpAnimationDuration);
            stateMachine.ChangeState(CatState.Locomotion);
        }

        public override bool Exit()
        {
            base.Exit();
            return false;
        }
    }
}