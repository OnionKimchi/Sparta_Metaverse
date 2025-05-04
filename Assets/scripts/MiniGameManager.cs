using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameManager : MonoBehaviour
{
    static MiniGameManager instance;
    private int highScore = 0;

    public static MiniGameManager Instance
    {
        get { return instance; }
    }

    private int currentScore = 0;
    UIManager uiManager;

    public UIManager UIManager
    {
        get { return uiManager; }
    }

    private void Awake()
    {
        instance = this;
        uiManager = FindObjectOfType<UIManager>();
    }

    private void Start()
    {
        uiManager.UpdateScore(0);
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }
    public void AddScore(int score)
    {
        currentScore += score;
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
        uiManager.UpdateScore(currentScore);
        Debug.Log("Score: " + currentScore);
    }
}