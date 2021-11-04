using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
    basically, yes
*/

public class UIManager : MonoBehaviour
{
    public enum GameScenes {
        MainMenu = 0,
        LevelOne = 1,
        LevelTwo = 2
    };

    public static GameObject manager;

    // Main Menu
    public Text titleText;
    public Text subtitleText;
    public RectTransform menuBox;
    public RectTransform loadingPanel;
    public Text bestScore;
    public Text bestTime;
    public Tweener tweener;

    // LevelOne
    public Text scoreText;
    public Text timeText;
    public RectTransform ghostScaredStatus;
    public Text ghostScaredTimer;
    public List<GameObject> lifeIndicators;
    public StatusManager statusManager;
    public GhostsStatusController ghostsStatusController;
    public bool ghostScaredUIShown;
    public GameObject lifeIndicatorPrefab;
    public Text countdown;
    private bool gameOverCalled = false; // Prevents coroutine from being called multiple times >:(((((

    private float currentTitleHue = 0.0f;
    private const float menuRatio = 800.0f/1920;

    // Start is called before the first frame update
    void Awake()
    {
        // Remove new manager if one is already present
        GameObject[] managers = GameObject.FindGameObjectsWithTag("Manager");
        if(managers.Length > 1)
        {
            foreach(GameObject manager in managers)
            {
                if(manager == gameObject)
                {
                    Destroy(manager);
                    Debug.Log("NEW MANAGER DESTROYED");
                    return;
                }
                    
            }
        }
        Debug.Log("Manager count: " + managers.Length);
        DontDestroyOnLoad(gameObject);
        InitializeMainMenu();
        HideLoadingScreenInstant();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(Input.GetKeyDown(KeyCode.W)) ShowMenu();
        if(Input.GetKeyDown(KeyCode.S)) HideMenu();
        */
        if(titleText != null && subtitleText != null) CycleColors();  // Cycle through colors for title and subtitle

        if(scoreText != null && statusManager != null)
            scoreText.text = statusManager.currentScore.ToString();
        if(timeText != null && statusManager != null)
            timeText.text = ((int)(statusManager.currentTime/60)).ToString("D2") + ":" + ((int)(statusManager.currentTime % 60)).ToString("D2") + ":" + ((int)((statusManager.currentTime%1)*100)).ToString("D2");

        if(statusManager != null)
        {
            if(lifeIndicators != null && statusManager.currentLife < lifeIndicators.Count)
            {
                MinusOneLife();
            }
            if(statusManager.gameOver && gameOverCalled == false)
            {
                gameOverCalled = true;
                StartCoroutine(GameOverCoroutine());
            }
        }

        if(ghostsStatusController != null && ghostScaredStatus != null)
        {
            if(ghostsStatusController.inScaredState)
            {
                if(!ghostScaredUIShown) 
                {
                    ghostScaredUIShown = true;
                    StartCoroutine(LerpUIElement(ghostScaredStatus, new Vector2(0.0f, ghostScaredStatus.anchoredPosition.y), 0.5f, Easings.Easing.so));
                }
                ghostScaredTimer.text = Mathf.Ceil(ghostsStatusController.scaredTimer).ToString() + "s";
            }
            else
            {
                if(ghostScaredUIShown)
                {
                    ghostScaredUIShown = false;
                    StartCoroutine(LerpUIElement(ghostScaredStatus, new Vector2(ghostScaredStatus.anchoredPosition.x-120.0f, ghostScaredStatus.anchoredPosition.y), 0.5f, Easings.Easing.so));
                }
            }
        }
    }

    // Cycles through colors for title and subtitle
    private void CycleColors() {
        if(currentTitleHue > 1000.0f) currentTitleHue = 0.0f;
        titleText.color = Color.HSVToRGB(currentTitleHue % 1.0f, 0.5f, 1.0f);
        subtitleText.color = Color.HSVToRGB((currentTitleHue + 0.1f) % 1.0f, 0.5f, 0.75f);
        currentTitleHue += 0.2f * Time.deltaTime;
    }

    /*
        Menu UI Initialization and Game Termination.
    */
    private void InitializeMainMenu() {
        tweener = GetComponent<Tweener>();
        menuBox = GameObject.FindWithTag("MenuBox").GetComponent<RectTransform>();
        loadingPanel = gameObject.transform.Find("LoadingCanvas").Find("Panel").GetComponent<RectTransform>();
        titleText = GameObject.FindWithTag("TitleText").GetComponent<Text>();
        subtitleText = GameObject.FindWithTag("SubtitleText").GetComponent<Text>();
        bestScore = GameObject.FindWithTag("HighScoreText").GetComponent<Text>();
        bestTime = GameObject.FindWithTag("BestTimeText").GetComponent<Text>();
        menuBox.anchoredPosition = new Vector2(-menuRatio * Screen.width, menuBox.anchoredPosition.y);
        loadingPanel.sizeDelta = new Vector2(Screen.width, Screen.height);

        menuBox.transform.Find("Level 1 Button").GetComponent<Button>().onClick.AddListener(LoadFirstLevel);
        //menuBox.transform.Find("Level 2 Button").GetComponent<Button>().onClick.AddListener(LoadSecondLevel);
        menuBox.transform.Find("Exit Button").GetComponent<Button>().onClick.AddListener(ExitGame);
        
        int bestScoreValue = PlayerPrefs.GetInt("BestScore", 0);
        float bestTimeValue = PlayerPrefs.GetFloat("BestTime", 0.0f);
        bestScore.text = "High Score: " + bestScoreValue;
        bestTime.text = "Best Time: " + ((int)(bestTimeValue/60)).ToString("D2") + ":" + ((int)(bestTimeValue % 60)).ToString("D2") + ":" + ((int)((bestTimeValue%1)*100)).ToString("D2");

        Debug.Log("Main menu initialized");
        ShowMenu();
    }

    private void InitializeLevelOne() {
        Debug.Log("Initializing level one");
        scoreText = GameObject.FindWithTag("ScoreText").GetComponent<Text>();
        timeText = GameObject.FindWithTag("TimeText").GetComponent<Text>();
        ghostScaredStatus = GameObject.FindWithTag("GhostScaredStatus").GetComponent<RectTransform>();
        ghostsStatusController = GameObject.FindWithTag("MainGameController").GetComponent<GhostsStatusController>();
        ghostScaredTimer = ghostScaredStatus.transform.Find("ScaredStateTimer").GetComponent<Text>();
        countdown = GameObject.FindWithTag("Countdown").GetComponent<Text>();
        ghostScaredUIShown = false;
        statusManager = GameObject.FindWithTag("MainGameController").GetComponent<StatusManager>();
        InitiateLifeIndicator();
        StartCoroutine(StartLevelCountdown());
    }

    public void ExitGame() {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    /*
        Code for showing/hiding UI elements
    */
    public void ShowMenu() {
        if(menuBox != null)
            StartCoroutine(LerpUIElement(menuBox, new Vector2(0.0f, 0.0f), 1.0f, Easings.Easing.so));
    }

    public void HideMenu() {
        if(menuBox != null)
            StartCoroutine(LerpUIElement(menuBox, new Vector2(-menuRatio * Screen.width, 0.0f), 1.0f, Easings.Easing.so));
    }

    // Show/Hide Loading Screen
    public void ShowLoadingScreen() {
        if(loadingPanel != null)
            StartCoroutine(LerpUIElement(loadingPanel, new Vector2(0.0f, 0.0f), 1.0f, Easings.Easing.so));
    }

    public void HideLoadingScreen() {
        if(loadingPanel != null)
            StartCoroutine(LerpUIElement(loadingPanel, new Vector2(Screen.width, 0.0f), 1.0f, Easings.Easing.so));
    }

    public void HideLoadingScreenInstant() {
        if(loadingPanel != null)
            StartCoroutine(LerpUIElement(loadingPanel, new Vector2(Screen.width, 0.0f), 0.0f, Easings.Easing.s));
    }

    /*
        Codes for loading levels
    */

    // Load levels
    public void LoadFirstLevel() {
        menuBox.transform.Find("Level 1 Button").GetComponent<Button>().interactable = false;
        menuBox.transform.Find("Level 2 Button").GetComponent<Button>().interactable = false;
        menuBox.transform.Find("Exit Button").GetComponent<Button>().interactable = false;
        StartCoroutine(LoadLevelCoroutine((int)GameScenes.LevelOne));
    }

    public void LoadMainMenu() {
        GameObject.FindWithTag("GameExitButton").GetComponent<Button>().interactable = false;
        StartCoroutine(LoadLevelCoroutine((int)GameScenes.MainMenu));
    }

    // what to do when a level loads
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        switch(scene.buildIndex) {
            case (int)GameScenes.MainMenu:
                InitializeMainMenu();
                Debug.Log("Main Menu Loaded");
                break;

            case (int)GameScenes.LevelOne:
                InitializeLevelOne();
                Debug.Log("First Level Loaded");
                break;

            case (int)GameScenes.LevelTwo:
                Debug.Log("Second Level Loaded");
                break;
        }
    }

    /* 
        basically LateOnSceneLoaded, This is used to destroy the old manager since we have a new one as a replacement when accessing the main menu.
        Can't really think off a better solution :'(((
    */
    public void DoStuffAfterLevelIsLoaded(int level) {
        switch(level) {
            case (int)GameScenes.MainMenu:
                Debug.Log("Main Menu Loaded2");
                break;

            case (int)GameScenes.LevelOne:
                Debug.Log("First Level Loaded2");
                break;

            case (int)GameScenes.LevelTwo:
                Debug.Log("Second Level Loaded2");
                break;
        }
    }

    /*
        Coroutines
    */

    // Coroutine to load a level
    public IEnumerator LoadLevelCoroutine(int level) {
        HideMenu();
        ShowLoadingScreen();

        yield return new WaitForSecondsRealtime(1); 
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(level); // Load Scene async
        Debug.Log("Loading level: " + level);
        SceneManager.sceneLoaded += OnSceneLoaded;
        while(!loadScene.isDone) 
            yield return null;
        SceneManager.sceneLoaded -= OnSceneLoaded;  // Prevent some stupid error i spent two hours debugging >:(((
        if(level != (int)GameScenes.MainMenu)
            HideLoadingScreenInstant();
        else
            HideLoadingScreen();
        DoStuffAfterLevelIsLoaded(level);
    }

    // Coroutine for start level countdown
    public IEnumerator StartLevelCountdown() {
        Time.timeScale = 0.0f; // Pauses the game
        countdown.text = "3";
        yield return new WaitForSecondsRealtime(1); 
        countdown.text = "2";
        yield return new WaitForSecondsRealtime(1); 
        countdown.text = "1";
        yield return new WaitForSecondsRealtime(1); 
        countdown.text = "GO!";
        yield return new WaitForSecondsRealtime(1); 
        countdown.text = "";
        Time.timeScale = 1.0f;
        statusManager.gameStarted = true;
        GameObject.FindWithTag("GameExitButton").GetComponent<Button>().onClick.AddListener(LoadMainMenu);
    }

    // Coroutine to lerp a UI element because i won't bother making another tweener/tween class for RectTransforms >:(
    private IEnumerator LerpUIElement(RectTransform uiElement, Vector2 targetPos, float duration, Easings.Easing ease)
    {
        Vector2 startingPos = uiElement.anchoredPosition;
        float currentTime = 0.0f;
        while (currentTime < duration && uiElement != null)
        {
            float timeFraction = currentTime/duration;
            timeFraction = Easings.CalculateTimeFraction(timeFraction, ease);
            uiElement.anchoredPosition = Vector2.Lerp(startingPos, targetPos, currentTime / duration);
            currentTime += Time.deltaTime;
            yield return null;
        }

        yield return null;
        uiElement.anchoredPosition = targetPos;
    }

    // GameOver coroutine
    private IEnumerator GameOverCoroutine()
    {
        Time.timeScale = 0.0f;
        countdown.text = "GAME OVER";
        GameObject.FindWithTag("GameExitButton").GetComponent<Button>().interactable = false;
        yield return new WaitForSecondsRealtime(3);
        Time.timeScale = 1.0f;
        StartCoroutine(LoadLevelCoroutine((int)GameScenes.MainMenu));
    }

    /*
        All the smaller stuff
    */
    // Initiates life indicators
    void InitiateLifeIndicator()
    {
        lifeIndicators = new List<GameObject>();
        float currentPos = 0.0f;
        //statusManager.currentLife
        Debug.Log(statusManager.currentLife);
        for(int i=0; i < statusManager.currentLife; i++)
        {
            GameObject lifeIndicator = Instantiate(lifeIndicatorPrefab, GameObject.FindWithTag("HUD").transform);
            lifeIndicators.Add(lifeIndicator);
            lifeIndicator.GetComponent<RectTransform>().anchoredPosition = new Vector2(currentPos, 0.0f);
            currentPos += 50.0f;
            Debug.Log("Life Added at: " + lifeIndicator.GetComponent<RectTransform>().anchoredPosition);
        }
    }

    // Remove Life
    void MinusOneLife()
    {
        GameObject lifeToBeRemoved = lifeIndicators[statusManager.currentLife];
        lifeIndicators.RemoveAt(statusManager.currentLife);
        Destroy(lifeToBeRemoved);
    }
}
