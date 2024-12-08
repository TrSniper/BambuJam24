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
            if (isInteracting) return;

            base.Enter();
            cat.Log("Entering Interacting State", LogStyles.StatePositive);
            cat.PlayInteractAnimation();

            SoundManager.Instance.PlayOneShotSound(SoundType.CatAttack);

            isInteracting = true;
            await UniTask.WaitForSeconds(gameConstants.interactAnimationDuration);
            isInteracting = false;

            stateMachine.ChangeState(CatState.Locomotion);
        }

        public override bool Exit()
        {
            base.Exit();
            return !isInteracting;
        }
    }
}