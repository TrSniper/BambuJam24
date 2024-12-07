using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameConstants", menuName = "GameConstants", order = 0)]
    public class GameConstants : ScriptableObject
    {
        [Header("Animation Parameters")]
        public int AnimationParam_Speed = Animator.StringToHash("Speed");
        public int AnimationParam_Interact = Animator.StringToHash("Interact");
        public int AnimationParam_Eat = Animator.StringToHash("Eat");
        public int AnimationParam_Sit = Animator.StringToHash("Sit");
        public int AnimationParam_RunJump = Animator.StringToHash("RunJump");

        [Header("Animation Durations")]
        public float RunJumpAnimationDuration = 2.6f;
        public float InteractAnimationDuration = 1.45f;
        public float EatAnimationDuration = 5f;
        public float StandUpAnimationDuration = 1.4f;

        [Header("Movement Speeds")]
        public float MaxMoveSpeedToEat = 0.1f;
        public float MaxMoveSpeedToInteract = 0.1f;
        public float MaxMoveSpeedToIdleJump = 0.1f;
        public float MaxMoveSpeedToSit = 0.1f;
        public float MinMoveSpeedToRunJump = 0.9f;
    }
}