using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

public class VolumeChanger : MonoBehaviour
{
    [SerializeField]
    Volume CatnipVolume;
    [SerializeField]
    Volume NormalVolume;

    [SerializeField] 
    float effectDuration = 7.0f;

    private void Awake()
    {
        Catnip.OnCatnipIngested += Catnip_OnCatnipIngested;
    }

    private void Start()
    {
        //Invoke(nameof(Catnip_OnCatnipIngested), 2f);
    }


    private async void Catnip_OnCatnipIngested()
    {
        CatnipVolume.enabled = true;
        CatnipVolume.weight = 0f;

        while (CatnipVolume.weight <= 1f)
        {
            CatnipVolume.weight += Time.deltaTime;
            NormalVolume.weight -= Time.deltaTime;
            await UniTask.Yield();
        }
        CatnipVolume.weight = 1f;
        NormalVolume.weight = 0f;
        NormalVolume.enabled = false;
        while(effectDuration>0f)
        {
            effectDuration -= Time.deltaTime;
            await UniTask.Yield();
        }
        while (CatnipVolume.weight >= 0f)
        {
            CatnipVolume.weight -= Time.deltaTime;
            NormalVolume.weight += Time.deltaTime;
            await UniTask.Yield();
        }
        CatnipVolume.weight = 0f;
        NormalVolume.weight = 1f;
    }
}
