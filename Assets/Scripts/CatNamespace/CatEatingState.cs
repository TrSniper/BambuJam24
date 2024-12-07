using Cysharp.Threading.Tasks;
using InspectorLogger;
using ScriptableObjects;

namespace CatNamespace
{
    public class CatEatingState : CatBaseState
    {
        public CatEatingState(Cat cat, GameConstants gameConstants, CatStateMachine stateMachine) : base(cat, gameConstants, stateMachine) { }

        private bool isEating;

        public override async void Enter()
        {
            base.Enter();
            cat.Log("Entering Eating State", LogStyles.StatePositive);
            cat.PlayEatAnimation();

            isEating = true;
            await UniTask.WaitForSeconds(gameConstants.eatAnimationDuration);
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