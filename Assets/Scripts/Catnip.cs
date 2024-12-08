using System;
using System.Collections;
using UnityEngine;

public class Catnip : MonoBehaviour
{
    public static event Action OnCatnipIngested;

    [SerializeField] ParticleSystem effect;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Cat")) return;
        OnCatnipIngested?.Invoke();
        StartCoroutine(nameof(EndThis));
    }

    private void Start()
    {
        StartCoroutine(nameof(EndThis));
    }

    IEnumerator EndThis()
    {
        effect.Play();
        yield return new WaitForSeconds(effect.main.duration);
        Destroy(gameObject);
    }
}
