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
        // Initialize sounds dictionary
        sounds = new Dictionary<string, AudioClip>
        {
            { "miyav1", startSounds[0] },
            { "miyav2", startSounds[1] },
            { "hithafif", startSounds[2] },
            { "takedamage", startSounds[3] },
            { "hitstrong", startSounds[4] },
            { "hitcinematic", startSounds[5] },
            { "door", startSounds[6] }
        };

        // Set black image to fully opaque
        blackImage.color = new Color(0, 0, 0, 1);

        // Start the sequence
        await StartingSequence();
    }

    async UniTask StartingSequence()
    {
        // Play "hithafif" on source 0
        await PlayAudio(startSources[0], sounds["hithafif"]);

        // Play "takedamage" on source 1
        await PlayAudio(startSources[1], sounds["takedamage"]);

        // Wait 2.4 seconds before the next audio
        await UniTask.Delay(2400);
        await PlayAudio(startSources[0], sounds["miyav1"]);

        // Play "hitstrong" on source 1
        await PlayAudio(startSources[1], sounds["hitstrong"]);

        // Play "miyav2" starting at 1.5 seconds and "door" on source 1 simultaneously
        PlayAudioFromTime(startSources[0], sounds["miyav2"], 1.5f);
        startSources[1].clip = sounds["door"];
        startSources[1].Play();

        // Wait 6 seconds for these audios to finish
        await UniTask.Delay(6000);

        startText.gameObject.SetActive(true);
        startText.DOFade(1, 2.5f);

        // Stop all sources and play the final audio
        StopAllSources();
        await PlayAudio(startSources[2], sounds["hitcinematic"]);
        TextAndBlackSequence();
    }

     void TextAndBlackSequence()
    {
        startText.DOFade(0, 2.5f);
        blackImage.DOFade(0, 2.5f);
    }

    async UniTask PlayAudio(AudioSource source, AudioClip clip)
    {
        // Play the audio clip on the given source
        if (source == null || clip == null) return;

        source.clip = clip;
        source.Play();

        // Wait for the duration of the clip
        await UniTask.Delay((int)(clip.length * 1000));
    }

    void PlayAudioFromTime(AudioSource source, AudioClip clip, float startTime)
    {
        // Play audio from a specific time
        if (source == null || clip == null) return;

        source.clip = clip;
        source.time = startTime;
        source.Play();
    }

    void StopAllSources()
    {
        // Stop all active audio sources
        foreach (var source in startSources)
        {
            if (source.isPlaying)
                source.Stop();
        }
    }
}
