using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class StartSequence : MonoBehaviour
{
    [SerializeField]
    AudioClip[] startSounds;
    Dictionary<string, AudioClip> sounds;

    [SerializeField]
    AudioSource[] startSources;

    [SerializeField]
    TextMeshProUGUI startText;
    [SerializeField]
    Image blackImage;

    private async void Start()
    {
        sounds = new Dictionary<string, AudioClip>();
        sounds.Clear();
        sounds.Add("miyav1", startSounds[0]);
        sounds.Add("miyav2", startSounds[1]);
        sounds.Add("hithafif", startSounds[2]);
        sounds.Add("takedamage", startSounds[3]);
        sounds.Add("hitstrong", startSounds[4]);
        sounds.Add("hitcinematic", startSounds[5]);
        sounds.Add("door", startSounds[6]);

        blackImage.color += new Color(0,0,0,1);

        await StartingSequence();
        TextAndBlackSequence();
    }

    async UniTask StartingSequence()
    {
        startSources[0].clip = sounds["hithafif"];
        startSources[0].Play();
        startSources[1].clip = sounds["takedamage"];
        startSources[1].Play();
        await UniTask.WaitForSeconds(startSources[1].clip.length);
        startSources[0].Stop();
        startSources[0].clip = sounds["miyav1"];
        await UniTask.WaitForSeconds(2.4f);
        startSources[0].Play();
        startSources[1].clip = sounds["hitstrong"];
        startSources[1].Play();
        await UniTask.WaitForSeconds(3.1f);
        startSources[0].Stop();
        startSources[0].clip = sounds["miyav2"];
        startSources[0].time = 1.5f;
        startSources[0].Play();
        startSources[1].Stop();
        startSources[1].clip = sounds["door"];
        startSources[1].Play();
        await UniTask.WaitForSeconds(6f);
        startSources[0].Stop();
        startSources[1].Stop();
        startSources[2].clip = sounds["hitcinematic"];
        startSources[2].Play();
        await UniTask.WaitForSeconds(sounds["hitcinematic"].length);
        startSources[2].Stop();
    }
    void TextAndBlackSequence()
    {
        startText.gameObject.SetActive(true);
        startText.DOFade(0, 2.5f);
        blackImage.DOFade(0, 2.5f);
    }


}
