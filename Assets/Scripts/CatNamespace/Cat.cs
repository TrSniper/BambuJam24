using System;
using InspectorLogger;
using ScriptableObjects;
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
        [Header("References")]
        [SerializeField] private GameConstants gameConstants;
        [SerializeField] private Animator animator;
        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private Transform camera;

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

        public bool CanEat() => currentSpeed <= gameConstants.maxMoveSpeedToEat;
        public bool CanInteract() => currentSpeed <= gameConstants.maxMoveSpeedToInteract;
        public bool CanSit() => currentSpeed <= gameConstants.maxMoveSpeedToSit;
        public bool CanIdleJump() => currentSpeed <= gameConstants.maxMoveSpeedToIdleJump;
        public bool CanRunJump() => currentSpeed >= gameConstants.minMoveSpeedToRunJump;

        private CatStateMachine stateMachine;
        private CatInput catInput;

        private void Start()
        {
            catInput = new CatInput();
            catInput.Enable();
            catInput.Cat.Enable();

            stateMachine = new CatStateMachine(this);

            stateMachine.RegisterState(CatState.Locomotion, new CatLocomotionState(this, gameConstants, stateMachine));
            //todo: stateMachine.RegisterState(CatState.IdleJumping, new CatIdleJumpingState(this, stateMachine));
            stateMachine.RegisterState(CatState.RunJumping, new CatRunJumpingState(this, gameConstants, stateMachine));
            stateMachine.RegisterState(CatState.Interacting, new CatInteractingState(this, gameConstants, stateMachine));
            stateMachine.RegisterState(CatState.Eating, new CatEatingState(this, gameConstants, stateMachine));
            stateMachine.RegisterState(CatState.Sitting, new CatSittingState(this, gameConstants, stateMachine));

            stateMachine.ChangeState(CatState.Locomotion);
        }

        private void Update()
        {
            GetInput();
            UpdateSpeeds();
            stateMachine.Update();
        }

        private void FixedUpdate()
        {
            MoveAndRotateCat();
        }

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

        private void UpdateSpeeds()
        {
            var targetSpeed = moveInput.magnitude * (isRunKey ? gameConstants.maxRunSpeed : gameConstants.maxWalkSpeed);

            if (currentSpeed < targetSpeed)
            {
                currentSpeed += (isRunKey ? gameConstants.runningAcceleration : gameConstants.acceleration) * Time.deltaTime;
                currentSpeed = Mathf.Min(currentSpeed, targetSpeed);
            }

            else if (currentSpeed > targetSpeed)
            {
                currentSpeed -= gameConstants.deceleration * Time.deltaTime;
                currentSpeed = Mathf.Max(currentSpeed, targetSpeed);
            }

            SetAnimatorSpeed(currentSpeed);
            SetAnimatorHorizontalInput(moveInput.x);
        }

        private void MoveAndRotateCat()
        {
            var moveDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
            moveDirection = camera.right * moveDirection.x + camera.forward * moveDirection.z;
            moveDirection.y = 0f;

            var velocity = moveDirection * (currentSpeed * gameConstants.catRealSpeedMultiplier);
            rigidbody.linearVelocity = new Vector3(velocity.x, rigidbody.linearVelocity.y, velocity.z);

            //Rotate cat
            transform.forward = Vector3.Slerp(transform.forward, moveDirection, gameConstants.catRotationSpeed * Time.fixedDeltaTime);
        }

#region AnimationMethods

        private void SetAnimatorSpeed(float speed)
        {
            currentSpeed = speed;
            animator.SetFloat(gameConstants.animationParamSpeed, speed);
        }

        private void SetAnimatorHorizontalInput(float targetHorizontalInput)
        {
            var currentHorizontalInput = animator.GetFloat(gameConstants.animationParamHorizontalInput);

            var smoothedHorizontalInput = Mathf.MoveTowards(
                currentHorizontalInput,
                targetHorizontalInput,
                gameConstants.horizontalInputSmoothSpeed * Time.deltaTime
            );

            animator.SetFloat(gameConstants.animationParamHorizontalInput, smoothedHorizontalInput);
        }


        public void PlayInteractAnimation()
        {
            this.Log("PlayInteractAnimation", LogStyles.AnimationPositive);
            animator.SetTrigger(gameConstants.animationParamInteract);
        }

        public void PlayEatAnimation()
        {
            this.Log("PlayEatAnimation", LogStyles.AnimationPositive);
            animator.SetTrigger(gameConstants.animationParamEat);
        }

        public void PlaySitAnimation()
        {
            this.Log("PlaySitAnimation", LogStyles.AnimationPositive);
            animator.SetBool(gameConstants.animationParamSit, true);
        }

        public void PlayStandUpAnimation()
        {
            this.Log("PlayStandUpAnimation", LogStyles.AnimationPositive);
            animator.SetBool(gameConstants.animationParamSit, false);
        }

        public void PlayRunJumpAnimation()
        {
            this.Log("PlayRunJumpAnimation", LogStyles.AnimationPositive);
            animator.SetTrigger(gameConstants.animationParamRunJump);
        }

#endregion AnimationMethods
    }
}