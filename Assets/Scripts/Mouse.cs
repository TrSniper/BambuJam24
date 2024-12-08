using System;
using CatNamespace;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using ScriptableObjects;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameConstants gameConstants;
    [SerializeField] private Patroller patroller;

    [Header("Info")]
    [SerializeField] private bool isDead;
    [SerializeField] private bool isEaten;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Cat")) return;
        var cat = other.GetComponent<Cat>();

        if (cat.GetCurrentState() is CatState.Interacting && !isDead)
        {
            isDead = true;
            patroller.Kill();
            patroller.enabled = false;
            PlayDeathAnimation().Forget();
            SoundManager.Instance.PlayOneShotSound(SoundType.MouseDeath);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("CatEatDetector")) return;
        var cat = other.transform.parent.GetComponent<Cat>();

        if (cat.GetCurrentState() is CatState.Eating && isDead && !isEaten)
        {
            isEaten = true;
            PlayBeingEatenAnimation().Forget();
            SoundManager.Instance.PlayOneShotSound(SoundType.MouseEaten);
        }
    }

    private async UniTask PlayDeathAnimation()
    {
        var currentYPosition = transform.position.y;
        await transform.DOMoveY(currentYPosition + gameConstants.mouseDeathAnimationUpDistance, gameConstants.mouseDeathAnimationUpDuration)
            .AsyncWaitForCompletion();

        var targetRotation = transform.rotation.eulerAngles + new Vector3(0, 0, gameConstants.mouseDeathAnimationZRotationAmount);
        await transform.DORotate(targetRotation, gameConstants.mouseDeathAnimationZRotationDuration)
            .AsyncWaitForCompletion();
    }

    private async UniTask PlayBeingEatenAnimation()
    {
        await UniTask.WaitForSeconds(gameConstants.mouseEatenAnimationDelay);
        await transform.DOScale(0f, gameConstants.mouseEatenAnimationDuration)
            .AsyncWaitForCompletion();
    }
}