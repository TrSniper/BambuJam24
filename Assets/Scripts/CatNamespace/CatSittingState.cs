using Cysharp.Threading.Tasks;
using InspectorLogger;

namespace CatNamespace
{
    public class CatSittingState : CatBaseState
    {
        public CatSittingState(Cat cat, CatStateMachine stateMachine) : base(cat, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            cat.Log("Entering Sitting State", LogStyles.StatePositive);
            cat.PlaySitAnimation();
        }

        public override async void Update()
        {
            base.Update();

            if (cat.IsSitKeyDown())
            {
                cat.PlayStandUpAnimation();
                await UniTask.WaitForSeconds(Cat.StandUpAnimationDuration);
                stateMachine.ChangeState(CatState.Locomotion);
            }
        }

        public override bool Exit()
        {
            base.Exit();
            return false;
        }
    }
}