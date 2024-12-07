using InspectorLogger;
using UnityEngine;

namespace CatNamespace
{
    public enum CatState
    {
        Locomotion,
        Jumping,
        Interacting,
        Eating,
        Sitting,
    }

    public class Cat : MonoBehaviour
    {
        public static int AnimationParam_Speed = Animator.StringToHash("Speed");
        public static int AnimationParam_Interact = Animator.StringToHash("Interact");
        public static int AnimationParam_Eat = Animator.StringToHash("Eat");
        public static int AnimationParam_Sit = Animator.StringToHash("Sit");
        public static int AnimationParam_RunJump = Animator.StringToHash("RunJump");

        public static float JumpAnimationDuration = 0.925f;
        public static float InteractAnimationDuration = 0.825f;
        public static float EatAnimationDuration = 0.95f;
        public static float StandUpAnimationDuration = 0.825f;

        [Header("References")]
        [SerializeField] private Animator animator;
        [SerializeField] private Rigidbody rigidbody;

        [Header("State Info")]
        [SerializeField] private CatState currentState;

        [Header("Input Info")]
        [SerializeField] private Vector2 lookInput;
        [SerializeField] private Vector2 moveInput;
        [SerializeField] private bool isInteractKeyDown;
        [SerializeField] private bool isSitKeyDown;
        [SerializeField] private bool isJumpKeyDown;
        [SerializeField] private bool isRunKey;

        public Vector2 GetLookInput() => lookInput;
        public Vector2 GetMoveInput() => moveInput;
        public bool IsInteractKeyDown() => isInteractKeyDown;
        public bool IsSitKeyDown() => isSitKeyDown;
        public bool IsJumpKeyDown() => isJumpKeyDown;
        public bool IsRunKey() => isRunKey;
        public bool StateMachineOnly_SetCurrentState(CatState state) => currentState == state;

        private CatStateMachine stateMachine;
        private CatInput catInput;

        private void Start()
        {
            catInput = new CatInput();
            catInput.Enable();
            catInput.Cat.Enable();

            stateMachine = new CatStateMachine(this);

            stateMachine.RegisterState(CatState.Locomotion, new CatLocomotionState(this, stateMachine));
            stateMachine.RegisterState(CatState.Jumping, new CatJumpingState(this, stateMachine));
            stateMachine.RegisterState(CatState.Interacting, new CatInteractingState(this, stateMachine));
            stateMachine.RegisterState(CatState.Eating, new CatEatingState(this, stateMachine));
            stateMachine.RegisterState(CatState.Sitting, new CatSittingState(this, stateMachine));

            stateMachine.ChangeState(CatState.Locomotion);
        }

        private void Update()
        {
            GetInput();
            stateMachine.Update();
        }

#region InputMethods

        private void GetInput()
        {
            lookInput = catInput.Cat.Look.ReadValue<Vector2>();
            moveInput = catInput.Cat.Move.ReadValue<Vector2>();
            isInteractKeyDown = catInput.Cat.Interact.WasPressedThisFrame();
            isSitKeyDown = catInput.Cat.Sit.WasPressedThisFrame();
            isJumpKeyDown = catInput.Cat.Jump.WasPressedThisFrame();
            isRunKey = catInput.Cat.Run.IsPressed();
        }

#endregion

#region AnimationMethods

        public void SetAnimatorSpeed(float speed)
        {
            animator.SetFloat(AnimationParam_Speed, speed);
        }

        public void PlayInteractAnimation()
        {
            this.Log("PlayInteractAnimation", LogStyles.AnimationPositive);
            animator.SetTrigger(AnimationParam_Interact);
        }

        public void PlayEatAnimation()
        {
            this.Log("PlayEatAnimation", LogStyles.AnimationPositive);
            animator.SetTrigger(AnimationParam_Eat);
        }

        public void PlaySitAnimation()
        {
            this.Log("PlaySitAnimation", LogStyles.AnimationPositive);
            animator.SetBool(AnimationParam_Sit, true);
        }

        public void PlayStandUpAnimation()
        {
            this.Log("PlayStandUpAnimation", LogStyles.AnimationPositive);
            animator.SetBool(AnimationParam_Sit, false);
        }

        public void PlayRunJumpAnimation()
        {
            this.Log("PlayRunJumpAnimation", LogStyles.AnimationPositive);
            animator.SetTrigger(AnimationParam_RunJump);
        }

#endregion AnimationMethods
    }
}