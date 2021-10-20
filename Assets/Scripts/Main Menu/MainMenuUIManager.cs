using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
    basically, this script is made to control animated things in the main menu.
    including pac-student and stuff.
*/

public class MainMenuUIManager : MonoBehaviour
{
    public enum GameScenes {
        MainMenu = 0,
        LevelOne = 1,
        LevelTwo = 2
    };

    [SerializeField] private Text titleText;
    [SerializeField] private Text subtitleText;
    [SerializeField] private RectTransform menuBox;
    [SerializeField] private RectTransform loadingPanel;
    private Tweener tweener;

    private float currentTitleHue = 0.0f;
    private const float menuRatio = 800.0f/1920;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        tweener = GetComponent<Tweener>();
        InitializeMenu();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(Input.GetKeyDown(KeyCode.W)) ShowMenu();
        if(Input.GetKeyDown(KeyCode.S)) HideMenu();
        */
        Debug.Log(loadingPanel.anchoredPosition);
        if(titleText != null && subtitleText != null) CycleColors();  // Cycle through colors for title and subtitle
    }

    // Cycles through colors for title and subtitle
    private void CycleColors() {
        if(currentTitleHue > 1000.0f) currentTitleHue = 0.0f;
        titleText.color = Color.HSVToRGB(currentTitleHue % 1.0f, 0.5f, 1.0f);
        subtitleText.color = Color.HSVToRGB((currentTitleHue + 0.1f) % 1.0f, 0.5f, 0.75f);
        currentTitleHue += 0.2f * Time.deltaTime;
    }

    public void HideMenu() {
        /*
        Vector2 startPosition = new Vector2(0.0f, 0.0f);
        Vector2 endPosition = new Vector2(-menuRatio * Screen.width, 0.0f);
        tweener.AddTween(menuBox, startPosition, endPosition, 1.0f, Easings.Easing.so);
        */
        StartCoroutine(LerpMenu(new Vector2(-menuRatio * Screen.width, 0.0f), 1.0f, Easings.Easing.so));
    }

    public void ShowMenu() {
        /*
        Vector2 startPosition = new Vector2(-menuRatio * Screen.width, 0.0f);
        Vector2 endPosition = new Vector2(0.0f, 0.0f);
        tweener.AddTween(menuBox, startPosition, endPosition, 1.0f, Easings.Easing.so);
        */
        StartCoroutine(LerpMenu(new Vector2(0.0f, 0.0f), 1.0f, Easings.Easing.so));
    }

    public void ExitGame() {
        Application.Quit();
    }

    private void InitializeMenu() {
        menuBox.anchoredPosition = new Vector2(-menuRatio * Screen.width, menuBox.anchoredPosition.y);
        loadingPanel.sizeDelta = new Vector2(Screen.width, Screen.height);
        HideLoadingScreen();
        ShowMenu();
    }

    // Show/Hide Loading Screen
    public void ShowLoadingScreen() {
        StartCoroutine(LerpLoadingScreen(new Vector2(0.0f, 0.0f), 1.0f, Easings.Easing.so));
    }

    public void HideLoadingScreen() {
        StartCoroutine(LerpLoadingScreen(new Vector2(Screen.width, 0.0f), 1.0f, Easings.Easing.so));
    }

    // Load levels
    public void LoadFirstLevel() {
        StartCoroutine(LoadLevelCoroutine((int)GameScenes.LevelOne));
    }

    // what to do when a level loads
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        /* Load first level UI */
    }

    public IEnumerator LoadLevelCoroutine(int level) {
        HideMenu();
        ShowLoadingScreen();

        yield return new WaitForSecondsRealtime(1); 
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(level); // Load Scene async
        SceneManager.sceneLoaded += OnSceneLoaded;
        while(!loadScene.isDone) 
            yield return null;
        HideLoadingScreen();
    }

    // Temporary fix, will prolly be removed
    private IEnumerator LerpLoadingScreen(Vector2 targetPos, float duration, Easings.Easing ease)
    {
        Vector2 startingPos = loadingPanel.anchoredPosition;
        float currentTime = 0.0f;
        while (currentTime < duration)
        {
            float timeFraction = currentTime/duration;
            timeFraction = Easings.CalculateTimeFraction(timeFraction, ease);
            loadingPanel.anchoredPosition = Vector2.Lerp(startingPos, targetPos, currentTime / duration);
            currentTime += Time.deltaTime;
            yield return null;
        }

        yield return null;
        loadingPanel.anchoredPosition = targetPos;
    }

    private IEnumerator LerpMenu(Vector2 targetPos, float duration, Easings.Easing ease)
    {
        Vector2 startingPos = menuBox.anchoredPosition;
        float currentTime = 0.0f;
        while (currentTime < duration)
        {
            float timeFraction = currentTime/duration;
            timeFraction = Easings.CalculateTimeFraction(timeFraction, ease);
            menuBox.anchoredPosition = Vector2.Lerp(startingPos, targetPos, currentTime / duration);
            currentTime += Time.deltaTime;
            yield return null;
        }

        yield return null;
        menuBox.anchoredPosition = targetPos;
    }
}
