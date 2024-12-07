using InspectorLogger;

namespace CatNamespace
{
    public class CatLocomotionState : CatBaseState
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

            if (cat.IsJumpKeyDown() && cat.CanRunJump()) stateMachine.ChangeState(CatState.RunJumping);
            else if (cat.IsInteractKeyDown() && cat.CanInteract()) stateMachine.ChangeState(CatState.Interacting);
            else if (cat.IsEatKeyDown() && cat.CanEat()) stateMachine.ChangeState(CatState.Eating);
            else if (cat.IsSitKeyDown() && cat.CanSit()) stateMachine.ChangeState(CatState.Sitting);
        }

        public override bool Exit()
        {
            base.Exit();
            cat.Log("Exiting Locomotion State", LogStyles.StateNegative);
            return true;
        }
    }
}