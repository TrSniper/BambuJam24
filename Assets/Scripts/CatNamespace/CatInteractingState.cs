using Cysharp.Threading.Tasks;
using InspectorLogger;
using ScriptableObjects;

namespace CatNamespace
{
    public class CatInteractingState : CatBaseState
    {
        public CatInteractingState(Cat cat, GameConstants gameConstants, CatStateMachine stateMachine) : base(cat, gameConstants, stateMachine) { }

        private bool isInteracting;

        public override async void Enter()
        {
            base.Enter();
            cat.Log("Entering Interacting State", LogStyles.StatePositive);
            cat.PlayInteractAnimation();

            isInteracting = true;
            await UniTask.WaitForSeconds(gameConstants.InteractAnimationDuration);
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