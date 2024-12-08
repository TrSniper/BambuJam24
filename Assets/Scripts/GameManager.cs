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
    [Header("Pause Menu References")]
    [SerializeField] private GameObject pauseMenu;

    [Header("Fail Texts")]
    [SerializeField] private string[] dogTexts;
    [SerializeField] private string[] waterTexts;
    [SerializeField] private string restartText;

    [Header("Fail References")]
    [SerializeField] private Image blackScreen;
    [SerializeField] private TextMeshProUGUI dogText;
    [SerializeField] private TextMeshProUGUI waterText;

    [Header("Fail Parameters")]
    [SerializeField] private float blackScreenFadeInDuration;
    [SerializeField] private float textFadeInDuration;
    [SerializeField] private float textDuration;
    [SerializeField] private float textFadeOutDuration;

    [Header("Info")]
    [SerializeField] private bool isOnFailScreen;

    private int dogTextIndex;
    private int waterTextIndex;
    private int catLives = 9;
    private bool isPaused;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isPaused) ClosePauseMenu();
        else if (Input.GetKeyDown(KeyCode.Escape)) OpenPauseMenu();
    }

    private void OpenPauseMenu()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        isPaused = true;
    }

    private void ClosePauseMenu()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        isPaused = false;
    }

    public async UniTask Fail(bool isDog, Transform checkPoint)
    {
        if (isOnFailScreen) return;

        catLives--;

        isOnFailScreen = true;
        var cat = FindFirstObjectByType<Cat>();
        cat.transform.position = checkPoint.position;
        cat.transform.forward = checkPoint.forward;

        blackScreen.gameObject.SetActive(true);
        dogText.gameObject.SetActive(isDog);
        waterText.gameObject.SetActive(!isDog);

        dogText.text = dogTexts[dogTextIndex];
        waterText.text = waterTexts[waterTextIndex];

        if (catLives == 0)
        {
            dogText.text = restartText;
            waterText.text = restartText;
        }

        dogTextIndex = (dogTextIndex + 1) % dogTexts.Length;
        waterTextIndex = (waterTextIndex + 1) % waterTexts.Length;

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

        if (catLives == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            catLives = 9;
        }
    }
}