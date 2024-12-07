using Cysharp.Threading.Tasks;
using InspectorLogger;

namespace CatNamespace
{
    public class CatEatingState : CatBaseState
    {
        public CatEatingState(Cat cat, CatStateMachine stateMachine) : base(cat, stateMachine) { }

        private bool isEating;

        public override async void Enter()
        {
            base.Enter();
            cat.Log("Entering Eating State", LogStyles.StatePositive);
            cat.PlayEatAnimation();

            isEating = true;
            await UniTask.WaitForSeconds(Cat.EatAnimationDuration);
            isEating = false;

            stateMachine.ChangeState(CatState.Locomotion);
        }

        public override void Update()
        {
            base.Update();
        }

        public override bool Exit()
        {
            base.Exit();
            return !isEating;
        }
    }
}