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

    private float currentTitleHue = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        menuBox = GameObject.FindWithTag("MenuBox").GetComponent<RectTransform>();
        titleText = GameObject.FindWithTag("TitleText").GetComponent<Text>();
        subtitleText = GameObject.FindWithTag("SubtitleText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        CycleColors();
    }

    // Cycles through colours
    private void CycleColors() {
        if(currentTitleHue > 1000.0f) currentTitleHue = 0.0f;
        titleText.color = Color.HSVToRGB(currentTitleHue % 1.0f, 0.5f, 1.0f);
        subtitleText.color = Color.HSVToRGB((currentTitleHue + 0.1f) % 1.0f, 0.5f, 0.75f);
        currentTitleHue += 0.2f * Time.deltaTime;
    }
}
