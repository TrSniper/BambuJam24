using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameConstants", menuName = "GameConstants", order = 0)]
    public class GameConstants : ScriptableObject
    {
        [Header("General Parameters")]
        public LayerMask groundLayer;

        [Header("Movement Parameters")]
        public float acceleration;
        public float runningAcceleration;
        public float deceleration;
        public float maxRunSpeed;
        public float maxWalkSpeed;
        public float catRealSpeedMultiplier;
        public float catRotationSpeed;
        public float horizontalInputSmoothSpeed;
        public float maxAllowedAngleForMovement;

        [Header("Camera Parameters")]
        public float cameraSensitivity;
        public bool isXInverted;
        public bool isYInverted;

        [Header("Animation Durations")]
        public float idleJumpAnimationDuration;
        public float runJumpAnimationDuration;
        public float interactAnimationDuration;
        public float eatAnimationDuration;

        [Header("Movement Speeds")]
        public float maxMoveSpeedToEat;
        public float maxMoveSpeedToInteract;
        public float maxMoveSpeedToIdleJump;
        public float maxMoveSpeedToSit;
        public float minMoveSpeedToRunJump;

        [Header("Animation Parameters")]
        public int animationParamSpeed = Animator.StringToHash("Speed");
        public int animationParamInteract = Animator.StringToHash("Interact");
        public int animationParamEat = Animator.StringToHash("Eat");
        public int animationParamRunJump = Animator.StringToHash("RunJump");
        public int animationParamIdleJump = Animator.StringToHash("IdleJump");
        public int animationParamHorizontalInput = Animator.StringToHash("HorizontalInput");
    }
}