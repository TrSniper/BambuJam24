using Cysharp.Threading.Tasks;
using InspectorLogger;

namespace CatNamespace
{
    public class CatEatingState : CatBaseState
    {
        public CatEatingState(Cat cat, CatStateMachine stateMachine) : base(cat, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            cat.Log("Entering Eating State", LogStyles.StatePositive);
            cat.PlayEatAnimation();
        }

        public override async void Update()
        {
            base.Update();
            await UniTask.WaitForSeconds(Cat.EatAnimationDuration);
            stateMachine.ChangeState(CatState.Locomotion);
        }

        public override bool Exit()
        {
            base.Exit();
            return false;
        }
    }
}