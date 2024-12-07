using InspectorLogger;
using UnityEngine;

namespace CatNamespace
{
    public enum CatState
    {
        Locomotion,
        IdleJumping,
        RunJumping,
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

        public static float RunJumpAnimationDuration = 2.6f;
        public static float InteractAnimationDuration = 1.45f;
        public static float EatAnimationDuration = 5f;
        public static float StandUpAnimationDuration = 1.4f;

        public static float MaxMoveSpeedToEat = 0.1f;
        public static float MaxMoveSpeedToInteract = 0.1f;
        public static float MaxMoveSpeedToIdleJump = 0.1f;
        public static float MaxMoveSpeedToSit = 0.1f;
        public static float MinMoveSpeedToRunJump = 0.9f;

        [Header("References")]
        [SerializeField] private Animator animator;
        [SerializeField] private Rigidbody rigidbody;

        [Header("State Info")]
        [SerializeField] private CatState currentState;
        [SerializeField] private float currentSpeed;

        [Header("Input Info")]
        [SerializeField] private Vector2 lookInput;
        [SerializeField] private Vector2 moveInput;
        [SerializeField] private bool isInteractKeyDown;
        [SerializeField] private bool isSitKeyDown;
        [SerializeField] private bool isJumpKeyDown;
        [SerializeField] private bool isEatKeyDown;
        [SerializeField] private bool isRunKey;

        public float GetCurrentSpeed() => currentSpeed;
        public Vector2 GetLookInput() => lookInput;
        public Vector2 GetMoveInput() => moveInput;
        public bool IsInteractKeyDown() => isInteractKeyDown;
        public bool IsSitKeyDown() => isSitKeyDown;
        public bool IsJumpKeyDown() => isJumpKeyDown;
        public bool IsEatKeyDown() => isEatKeyDown;
        public bool IsRunKey() => isRunKey;
        public void StateMachineOnly_SetCurrentState(CatState state) => currentState = state;

        public bool CanEat() => currentSpeed <= MaxMoveSpeedToEat;
        public bool CanInteract() => currentSpeed <= MaxMoveSpeedToInteract;
        public bool CanSit() => currentSpeed <= MaxMoveSpeedToSit;
        public bool CanIdleJump() => currentSpeed <= MaxMoveSpeedToIdleJump;
        public bool CanRunJump() => currentSpeed >= MinMoveSpeedToRunJump;

        private CatStateMachine stateMachine;
        private CatInput catInput;

        private void Start()
        {
            catInput = new CatInput();
            catInput.Enable();
            catInput.Cat.Enable();

            stateMachine = new CatStateMachine(this);

            stateMachine.RegisterState(CatState.Locomotion, new CatLocomotionState(this, stateMachine));
            //todo: stateMachine.RegisterState(CatState.IdleJumping, new CatIdleJumpingState(this, stateMachine));
            stateMachine.RegisterState(CatState.RunJumping, new CatRunJumpingState(this, stateMachine));
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
            isEatKeyDown = catInput.Cat.Eat.WasPressedThisFrame();
            isSitKeyDown = catInput.Cat.Sit.WasPressedThisFrame();
            isJumpKeyDown = catInput.Cat.Jump.WasPressedThisFrame();
            isRunKey = catInput.Cat.Run.IsPressed();
        }

#endregion

#region AnimationMethods

        public void SetAnimatorSpeed(float speed)
        {
            currentSpeed = speed;
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