using Cysharp.Threading.Tasks;
using InspectorLogger;
using ScriptableObjects;

namespace CatNamespace
{
    public class CatSittingState : CatBaseState
    {
        public CatSittingState(Cat cat, GameConstants gameConstants, CatStateMachine stateMachine) : base(cat, gameConstants, stateMachine) { }

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
                await UniTask.WaitForSeconds(gameConstants.standUpAnimationDuration);
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