using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    public bool innovated = false;
    public int currentScore;
    public float currentTime;
    public int currentLife = 3;
    public bool gameOver = false;
    public bool gameStarted = false;

    // if innovated true
    public float timeWithoutEating = 0.0f;

    void Start()
    {
        
    }
    void Update()
    {
        if((currentLife <= 0 || GameObject.FindWithTag("Pellet") == null) && !gameOver) // If Pellet not found or no lives left
        {
            // game over
            Time.timeScale = 0.0f;
            gameOver = true;
            SaveHighScore();
        }
        if(!gameOver) currentTime += Time.deltaTime;

        if(innovated) InnovatedUpdate();

        if(!gameStarted)    // Initialize score and time
        {
            currentScore = 0;
            currentTime = 0.0f;
            gameStarted = true;
        }
    }

    void InnovatedUpdate()
    {
        timeWithoutEating += Time.deltaTime;
    }

    void SaveHighScore()
    {
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        float bestTime = PlayerPrefs.GetFloat("BestTime", 0.0f);

        if(currentScore > bestScore || (currentScore == bestScore && currentTime < bestTime)) 
        {
            PlayerPrefs.SetInt("BestScore", currentScore);
            PlayerPrefs.SetFloat("BestTime", currentTime);
        }
    }
}
