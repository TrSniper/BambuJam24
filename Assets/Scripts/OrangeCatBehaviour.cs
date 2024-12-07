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