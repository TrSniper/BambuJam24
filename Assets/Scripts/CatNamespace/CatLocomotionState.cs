using InspectorLogger;

namespace CatNamespace
{
    public class CatLocomotionState : CatStateBase
    {
        public CatLocomotionState(Cat cat, CatStateMachine stateMachine) : base(cat, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            cat.Log("Entering Locomotion State", LogStyles.StatePositive);
        }

        public override void Update()
        {
            base.Update();
            var moveInput = cat.GetMoveInput();

            cat.SetAnimatorSpeed(moveInput.magnitude);

            if (cat.IsJumpKeyDown())
            {
                stateMachine.ChangeState(CatState.Jumping);
            }

            else if (cat.IsInteractKeyDown())
            {
                stateMachine.ChangeState(CatState.Interacting);
            }

            else if (cat.IsSitKeyDown())
            {
                stateMachine.ChangeState(CatState.SittingDown);
            }
        }

        public override void Exit()
        {
            base.Exit();
            cat.Log("Exiting Locomotion State", LogStyles.StateNegative);
        }
    }
}