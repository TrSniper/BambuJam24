using System;
using InspectorLogger;
using UnityEngine;

public class OrangeCatBehaviour : MonoBehaviour
{
    public static int AnimationParam_Speed = Animator.StringToHash("Speed");
    public static int AnimationParam_Interact = Animator.StringToHash("Interact");
    public static int AnimationParam_Eat = Animator.StringToHash("Eat");
    public static int AnimationParam_Sit = Animator.StringToHash("Sit");
    public static int AnimationParam_RunJump = Animator.StringToHash("RunJump");

    public static float InteractAnimationDuration = 0.5f;
    public static float EatAnimationDuration = 0.5f;
    public static float StandUpAnimationDuration = 0.5f;

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

    [Header("Animation Info")]
    [SerializeField] private bool currentInteract;
    [SerializeField] private bool currentEat;
    [SerializeField] private bool currentSit;
    [SerializeField] private bool currentRunJump;

    private CatInput catInput;

    private void Start()
    {
        catInput = new CatInput();
        catInput.Enable();
        catInput.Cat.Enable();
    }

    private void Update()
    {
        GetInput();
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

    private void SetAnimatorSpeed(float speed)
    {
        animator.SetFloat(AnimationParam_Speed, speed);
    }

    private void PlayInteractAnimation()
    {
        this.Log("PlayInteractAnimation", LogStyles.AnimationPositive);
        animator.SetBool(AnimationParam_Interact, true);
        animator.SetBool(AnimationParam_Interact, false);
    }

    private void PlayEatAnimation()
    {
        this.Log("PlayEatAnimation", LogStyles.AnimationPositive);
        animator.SetBool(AnimationParam_Eat, true);
        animator.SetBool(AnimationParam_Eat, false);
    }

    private void PlaySitAnimation()
    {
        this.Log("PlaySitAnimation", LogStyles.AnimationPositive);
        animator.SetBool(AnimationParam_Sit, true);
    }

    private void PlayStandUpAnimation()
    {
        this.Log("PlayStandUpAnimation", LogStyles.AnimationPositive);
        animator.SetBool(AnimationParam_Sit, false);
    }

    private void PlayRunJumpAnimation()
    {
        this.Log("PlayRunJumpAnimation", LogStyles.AnimationPositive);
        animator.SetBool(AnimationParam_RunJump, true);
        animator.SetBool(AnimationParam_RunJump, false);
    }

#endregion AnimationMethods
}