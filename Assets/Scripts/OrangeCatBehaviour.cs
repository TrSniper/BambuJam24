using System;
using UnityEngine;

public class OrangeCatBehaviour : MonoBehaviour
{
    public static int AnimationParam_Speed = Animator.StringToHash("Speed");
    public static int AnimationParam_Attack = Animator.StringToHash("Attack");
    public static int AnimationParam_Eat = Animator.StringToHash("Eat");
    public static int AnimationParam_Sit = Animator.StringToHash("Sit");
    public static int AnimationParam_RunJump = Animator.StringToHash("RunJump");

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rigidbody;

    [Header("Input Info")]
    [SerializeField] private Vector2 lookInput;
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private bool isAttackKeyDown;
    [SerializeField] private bool isInteractKeyDown;
    [SerializeField] private bool isSitKeyDown;
    [SerializeField] private bool isJumpKeyDown;
    [SerializeField] private bool isRunKey;

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
        isAttackKeyDown = catInput.Cat.Attack.WasPressedThisFrame();
        isInteractKeyDown = catInput.Cat.Interact.WasPressedThisFrame();
        isSitKeyDown = catInput.Cat.Sit.WasPressedThisFrame();
        isJumpKeyDown = catInput.Cat.Jump.WasPressedThisFrame();
        isRunKey = catInput.Cat.Run.IsPressed();
    }

#endregion

#region AnimationMethods

    private void SetSpeed(float speed)
    {
        animator.SetFloat(AnimationParam_Speed, speed);
    }

    private void SetAttack(bool attack)
    {
        animator.SetBool(AnimationParam_Attack, attack);
    }

    private void SetEat(bool eat)
    {
        animator.SetBool(AnimationParam_Eat, eat);
    }

    private void SetSit(bool sit)
    {
        animator.SetBool(AnimationParam_Sit, sit);
    }

    private void SetRunJump(bool runJump)
    {
        animator.SetBool(AnimationParam_RunJump, runJump);
    }

#endregion AnimationMethods
}