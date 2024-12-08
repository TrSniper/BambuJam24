using Cysharp.Threading.Tasks;
using UnityEngine;

public class FailCollider : MonoBehaviour
{
    [SerializeField] private Transform checkPoint;
    [SerializeField] private bool isDog;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Cat")) return;

        GameManager.Instance.Fail(isDog, checkPoint).Forget();
    }
}