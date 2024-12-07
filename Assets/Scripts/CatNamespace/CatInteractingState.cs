using Cysharp.Threading.Tasks;
using InspectorLogger;

namespace CatNamespace
{
    public class CatInteractingState : CatBaseState
    {
        public CatInteractingState(Cat cat, CatStateMachine stateMachine) : base(cat, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            cat.Log("Entering Interacting State", LogStyles.StatePositive);
            cat.PlayInteractAnimation();
        }

        public override async void Update()
        {
            base.Update();
            cat.PlayInteractAnimation();
            await UniTask.WaitForSeconds(Cat.InteractAnimationDuration);
            stateMachine.ChangeState(CatState.Locomotion);
        }

        public override bool Exit()
        {
            base.Exit();
            return false;
        }
    }
}