using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameOverCollider : MonoBehaviour
{
    [SerializeField] private bool isDog;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cat")) GameOverManager.Instance.GameOver(isDog).Forget();
    }
}