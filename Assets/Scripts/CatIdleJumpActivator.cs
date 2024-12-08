using CatNamespace;
using ScriptableObjects;
using UnityEngine;

public class CatIdleJumpActivator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameConstants gameConstants;

    private Cat cat;

    private void Start()
    {
        cat = FindFirstObjectByType<Cat>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("CatIdleJumpDetector")) return;

        var angleDifferent = Vector3.Angle(cat.transform.forward, transform.forward);
        cat.allowedToIdleJump = !(angleDifferent > gameConstants.idleJumpAnimationForwardCheckTolerance);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("CatIdleJumpDetector")) return;
        cat.allowedToIdleJump = false;
    }
}