using Cysharp.Threading.Tasks;
using InspectorLogger;

namespace CatNamespace
{
    public class CatInteractingState : CatBaseState
    {
        public CatInteractingState(Cat cat, CatStateMachine stateMachine) : base(cat, stateMachine) { }

        private bool isInteracting;

        public override async void Enter()
        {
            base.Enter();
            cat.Log("Entering Interacting State", LogStyles.StatePositive);
            cat.PlayInteractAnimation();

            isInteracting = true;
            await UniTask.WaitForSeconds(Cat.InteractAnimationDuration);
            isInteracting = false;

            stateMachine.ChangeState(CatState.Locomotion);
        }

        public override void Update()
        {
            base.Update();
        }

        public override bool Exit()
        {
            base.Exit();
            return !isInteracting;
        }
    }
}