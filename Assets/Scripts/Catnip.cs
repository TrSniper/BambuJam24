using System;
using System.Collections;
using UnityEngine;

public class Catnip : MonoBehaviour
{
    public static event Action OnCatnipIngested;

    [SerializeField] ParticleSystem effect;
    private void OnTriggerEnter(Collider other)
    {
        OnCatnipIngested?.Invoke();
        StartCoroutine(nameof(EndThis));
    }

    IEnumerator EndThis()
    {
        effect.Play();
        yield return new WaitForSeconds(effect.main.duration);
        Destroy(gameObject);
    }
}
