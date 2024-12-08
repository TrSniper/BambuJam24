using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image blackScreen;
    [SerializeField] private TextMeshProUGUI dogText;
    [SerializeField] private TextMeshProUGUI waterText;

    [Header("Parameters")]
    [SerializeField] private float blackScreenFadeInDuration;
    [SerializeField] private float textFadeInDuration;
    [SerializeField] private float textDuration;
    [SerializeField] private float textFadeOutDuration;

    public static GameOverManager Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) GameOver(true).Forget();
    }
#endif

    public async UniTask GameOver(bool isDog)
    {
        blackScreen.gameObject.SetActive(true);
        blackScreen.DOFade(1, blackScreenFadeInDuration);
        await UniTask.WaitForSeconds(blackScreenFadeInDuration);

        dogText.gameObject.SetActive(isDog);
        waterText.gameObject.SetActive(!isDog);

        dogText.DOFade(1, textFadeInDuration);
        waterText.DOFade(1, textFadeInDuration);
        await UniTask.WaitForSeconds(textFadeInDuration + textDuration);

        dogText.DOFade(0, textFadeOutDuration);
        waterText.DOFade(0, textFadeOutDuration);

        await UniTask.WaitForSeconds(textFadeOutDuration);

        SceneManager.LoadScene(0);
    }
}