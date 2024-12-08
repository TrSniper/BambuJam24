using System;
using CatNamespace;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
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

    [Header("Info")]
    [SerializeField] private bool isOnFailScreen;

    public bool IsOnFailScreen() => isOnFailScreen;

    public static GameManager Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public async UniTask Fail(bool isDog, Transform checkPoint)
    {
        if (isOnFailScreen) return;

        isOnFailScreen = true;
        var cat = FindFirstObjectByType<Cat>();
        cat.transform.position = checkPoint.position;
        cat.transform.forward = checkPoint.forward;

        blackScreen.gameObject.SetActive(true);
        dogText.gameObject.SetActive(isDog);
        waterText.gameObject.SetActive(!isDog);

        dogText.DOFade(1, textFadeInDuration);
        waterText.DOFade(1, textFadeInDuration);
        await UniTask.WaitForSeconds(textFadeInDuration + textDuration);

        dogText.DOFade(0, textFadeOutDuration);
        waterText.DOFade(0, textFadeOutDuration);
        await UniTask.WaitForSeconds(textFadeOutDuration);

        blackScreen.DOFade(0, blackScreenFadeInDuration);
        await UniTask.WaitForSeconds(blackScreenFadeInDuration);

        blackScreen.gameObject.SetActive(false);
        dogText.gameObject.SetActive(false);
        waterText.gameObject.SetActive(false);

        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 255f);
        dogText.color = new Color(dogText.color.r, dogText.color.g, dogText.color.b, 0);
        waterText.color = new Color(waterText.color.r, waterText.color.g, waterText.color.b, 0);

        isOnFailScreen = false;
    }
}