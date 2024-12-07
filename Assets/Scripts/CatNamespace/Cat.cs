﻿using InspectorLogger;
using UnityEngine;

namespace CatNamespace
{
    public enum CatState
    {
        Locomotion,
        Jumping,
        Interacting,
        Eating,
        SittingDown,
        Sitting,
        StandingUp,
    }

    public class Cat : MonoBehaviour
    {
        public static int AnimationParam_Speed = Animator.StringToHash("Speed");
        public static int AnimationParam_Interact = Animator.StringToHash("Interact");
        public static int AnimationParam_Eat = Animator.StringToHash("Eat");
        public static int AnimationParam_Sit = Animator.StringToHash("Sit");
        public static int AnimationParam_RunJump = Animator.StringToHash("RunJump");

        public static float InteractAnimationDuration = 0.825f;
        public static float EatAnimationDuration = 0.95f;
        public static float StandUpAnimationDuration = 0.825f;

        [Header("References")]
        [SerializeField] private Animator animator;
        [SerializeField] private Rigidbody rigidbody;

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

        private CatStateMachine stateMachine;
        private CatInput catInput;

        private void Start()
        {
            catInput = new CatInput();
            catInput.Enable();
            catInput.Cat.Enable();

            stateMachine = new CatStateMachine();

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
            animator.SetBool(AnimationParam_Interact, true);
            animator.SetBool(AnimationParam_Interact, false);
        }

        public void PlayEatAnimation()
        {
            this.Log("PlayEatAnimation", LogStyles.AnimationPositive);
            animator.SetBool(AnimationParam_Eat, true);
            animator.SetBool(AnimationParam_Eat, false);
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
            animator.SetBool(AnimationParam_RunJump, true);
            animator.SetBool(AnimationParam_RunJump, false);
        }

#endregion AnimationMethods
    }
}