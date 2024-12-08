using System;
using System.Linq;
using Cysharp.Threading.Tasks;
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
    }

    public class Cat : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameConstants gameConstants;
        [SerializeField] private Animator animator;
        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private Transform camera;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip[] walkSounds;
        [SerializeField] private AudioClip[] runSounds;

        [Header("State Info")]
        [SerializeField] private CatState currentState;
        [SerializeField] private float currentSpeed;

        [Header("Input Info")]
        [SerializeField] private Vector2 lookInput;
        [SerializeField] private Vector2 moveInput;
        [SerializeField] private bool isInteractKeyDown;
        [SerializeField] private bool isJumpKeyDown;
        [SerializeField] private bool isEatKeyDown;
        [SerializeField] private bool isRunKey;

        [Header("Sound Info")]
        [SerializeField] private bool isWaitingForSound;

        public Rigidbody GetRigidbody() => rigidbody;
        public CatState GetCurrentState() => currentState;
        public float GetCurrentSpeed() => currentSpeed;
        public Vector2 GetLookInput() => lookInput;
        public Vector2 GetMoveInput() => moveInput;
        public bool IsInteractKeyDown() => isInteractKeyDown;
        public bool IsJumpKeyDown() => isJumpKeyDown;
        public bool IsEatKeyDown() => isEatKeyDown;
        public bool IsRunKey() => isRunKey;
        public void StateMachineOnly_SetCurrentState(CatState state) => currentState = state;

        public bool CanEat() => currentSpeed <= gameConstants.maxMoveSpeedToEat;
        public bool CanInteract() => currentSpeed <= gameConstants.maxMoveSpeedToInteract;

        private CatStateMachine stateMachine;
        private CatInput catInput;

        private int currentWalkSoundIndex = 0;
        private int currentRunSoundIndex = 0;

        private void Start()
        {
            catInput = new CatInput();
            catInput.Enable();
            catInput.Cat.Enable();

            stateMachine = new CatStateMachine(this);

            stateMachine.RegisterState(CatState.Locomotion, new CatLocomotionState(this, gameConstants, stateMachine));
            stateMachine.RegisterState(CatState.IdleJumping, new CatIdleJumpingState(this, gameConstants, stateMachine));
            stateMachine.RegisterState(CatState.RunJumping, new CatRunJumpingState(this, gameConstants, stateMachine));
            stateMachine.RegisterState(CatState.Interacting, new CatInteractingState(this, gameConstants, stateMachine));
            stateMachine.RegisterState(CatState.Eating, new CatEatingState(this, gameConstants, stateMachine));

            stateMachine.ChangeState(CatState.Locomotion);
        }

        private void Update()
        {
            if (currentState is not CatState.RunJumping and not CatState.IdleJumping)
            {
                GetInput();
                UpdateSpeeds();
            }

            stateMachine.Update();
            ToggleSound();
        }

        private void FixedUpdate()
        {
            MoveAndRotateCat();
        }

        private void GetInput()
        {
            if (GameManager.Instance.IsOnFailScreen())
            {
                ResetInput();
                return;
            }

            lookInput = catInput.Cat.Look.ReadValue<Vector2>();
            moveInput = catInput.Cat.Move.ReadValue<Vector2>();
            isInteractKeyDown = catInput.Cat.Interact.WasPressedThisFrame();
            isEatKeyDown = catInput.Cat.Eat.WasPressedThisFrame();
            isJumpKeyDown = catInput.Cat.Jump.WasPressedThisFrame();
            isRunKey = catInput.Cat.Run.IsPressed();
        }

        private void ResetInput()
        {
            lookInput = Vector2.zero;
            moveInput = Vector2.zero;
            isInteractKeyDown = false;
            isEatKeyDown = false;
            isJumpKeyDown = false;
            isRunKey = false;
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
            SetAnimatorHorizontalInput();
        }

        private void MoveAndRotateCat()
        {
            var moveDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
            moveDirection = camera.right * moveDirection.x + camera.forward * moveDirection.z;
            moveDirection.y = 0f;

            //Do not move if do not look at the moveDirection
            var angleDifference = Vector3.Angle(transform.forward, moveDirection);

            if (angleDifference > gameConstants.maxAllowedAngleForMovement)
            {
                rigidbody.linearVelocity = new Vector3(0, rigidbody.linearVelocity.y, 0);
            }

            else
            {
                var velocity = moveDirection * (currentSpeed * gameConstants.catRealSpeedMultiplier);
                rigidbody.linearVelocity = new Vector3(velocity.x, rigidbody.linearVelocity.y, velocity.z);
            }

            var rotationSpeed = currentSpeed > gameConstants.minMoveSpeedToFastRotate ? gameConstants.catRunningRotationSpeed : gameConstants.catRotationSpeed;
            var baseStep = rotationSpeed * Time.fixedDeltaTime;
            var slerpT = baseStep / (1f + (angleDifference / 90f));
            transform.forward = Vector3.Slerp(transform.forward, moveDirection, slerpT);

            AlignWithSlope();
        }

        private void AlignWithSlope()
        {
            var rayOrigin = transform.position + Vector3.up * 0.5f;

            if (Physics.Raycast(rayOrigin, Vector3.down, out var hit, 3f, gameConstants.groundLayer))
            {
                var groundNormal = hit.normal;
                var projectedNormal = Vector3.ProjectOnPlane(groundNormal, transform.right);

                if (projectedNormal.sqrMagnitude > 0.0001f)
                {
                    var angle = Vector3.SignedAngle(transform.up, projectedNormal, transform.right);
                    var pitchRotation = Quaternion.AngleAxis(angle, transform.right);
                    var targetRotation = pitchRotation * transform.rotation;
                    transform.rotation = targetRotation;
                }

                else
                {
                    //Debug.LogError("Ground hit but normal is too small");
                }
            }

            else
            {
                //Debug.LogError("No ground hit");
            }
        }

        private async UniTask ToggleSound()
        {
            if (isWaitingForSound) return;

            if (currentSpeed > 0f)
            {
                if (isRunKey)
                {
                    if (!audioSource.isPlaying || !runSounds.Contains(audioSource.clip))
                    {
                        await PlayNextRunSound();
                    }
                }
                else
                {
                    if (!audioSource.isPlaying || !walkSounds.Contains(audioSource.clip))
                    {
                        await PlayNextWalkSound();
                    }
                }
            }
            else if (currentSpeed <= 0.15f && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }

        private async UniTask PlayNextWalkSound()
        {
            if (isWaitingForSound || walkSounds.Length == 0) return;

            isWaitingForSound = true;

            var randomIndex = UnityEngine.Random.Range(0, walkSounds.Length);
            audioSource.clip = walkSounds[randomIndex];
            audioSource.Play();

            await UniTask.WaitForSeconds(gameConstants.catWalkingSoundInterval);

            isWaitingForSound = false;
        }

        private async UniTask PlayNextRunSound()
        {
            if (isWaitingForSound || runSounds.Length == 0) return;

            isWaitingForSound = true;

            var randomIndex = UnityEngine.Random.Range(0, runSounds.Length);
            audioSource.clip = runSounds[randomIndex];
            audioSource.Play();

            await UniTask.WaitForSeconds(gameConstants.catRunningSoundInterval);

            isWaitingForSound = false;
        }


#region AnimationMethods

        private void SetAnimatorSpeed(float speed)
        {
            currentSpeed = speed;
            animator.SetFloat(gameConstants.animationParamSpeed, speed);
        }

        private void SetAnimatorHorizontalInput()
        {
            var moveDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
            moveDirection = camera.right * moveDirection.x + camera.forward * moveDirection.z;
            moveDirection.y = 0f;

            var angleDifference = Vector3.SignedAngle(transform.forward, moveDirection, Vector3.up);
            var targetHorizontalInput = Mathf.Clamp(angleDifference / 90f, -1f, 1f);
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

        public void PlayRunJumpAnimation()
        {
            this.Log("PlayRunJumpAnimation", LogStyles.AnimationPositive);
            animator.SetTrigger(gameConstants.animationParamRunJump);
        }

        public void PlayIdleJumpAnimation()
        {
            this.Log("PlayIdleJumpAnimation", LogStyles.AnimationPositive);
            animator.SetTrigger(gameConstants.animationParamIdleJump);
        }

#endregion AnimationMethods
    }
}