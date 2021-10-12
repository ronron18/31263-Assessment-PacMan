using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    basically, this script is made to control animated things in the main menu.
    including pac-student and stuff.
*/

public class MainMenuAnimManager : MonoBehaviour
{
    private Text titleText;
    private Text subtitleText;
    private RectTransform menuBox;
    private Tweener tweener;

    private float currentTitleHue = 0.0f;
    private const float menuRatio = 800.0f/1920;

    // Start is called before the first frame update
    void Start()
    {
        tweener = GetComponent<Tweener>();
        menuBox = GameObject.FindWithTag("MenuBox").GetComponent<RectTransform>();
        titleText = GameObject.FindWithTag("TitleText").GetComponent<Text>();
        subtitleText = GameObject.FindWithTag("SubtitleText").GetComponent<Text>();
        InitializeMenu();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(Input.GetKeyDown(KeyCode.W)) ShowMenu();
        if(Input.GetKeyDown(KeyCode.S)) HideMenu();
        */
        CycleColors();  // Cycle through colors for title and subtitle
    }

    // Cycles through colors for title and subtitle
    private void CycleColors() {
        if(currentTitleHue > 1000.0f) currentTitleHue = 0.0f;
        titleText.color = Color.HSVToRGB(currentTitleHue % 1.0f, 0.5f, 1.0f);
        subtitleText.color = Color.HSVToRGB((currentTitleHue + 0.1f) % 1.0f, 0.5f, 0.75f);
        currentTitleHue += 0.2f * Time.deltaTime;
    }

    private void HideMenu() {
        Vector2 startPosition = new Vector2(0.0f, 0.0f);
        Vector2 endPosition = new Vector2(-menuRatio * Screen.width, 0.0f);
        tweener.AddTween(menuBox, startPosition, endPosition, 1.0f, Easings.Easing.so);
    }

    private void ShowMenu() {
        Vector2 startPosition = new Vector2(-menuRatio * Screen.width, 0.0f);
        Vector2 endPosition = new Vector2(0.0f, 0.0f);
        tweener.AddTween(menuBox, startPosition, endPosition, 1.0f, Easings.Easing.so);
    }

    private void InitializeMenu() {
        menuBox.anchoredPosition = new Vector2(-menuRatio * Screen.width, menuBox.anchoredPosition.y);
        ShowMenu();
    }
}
