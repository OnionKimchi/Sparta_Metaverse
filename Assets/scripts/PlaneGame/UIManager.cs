using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // SceneManager를 사용하기 위해 추가

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public GameObject restartButton; // Restart 버튼
    public GameObject returnToMainButton; // Return to Main Scene 버튼

    public void Start()
    {
        if (restartButton == null)
        {
            Debug.LogError("Restart button is null");
        }

        if (scoreText == null)
        {
            Debug.LogError("ScoreText is null");
            return;
        }
        if (highScoreText == null)
        {
            Debug.LogError("HighScoreText is null");
            return;
        }
        if (returnToMainButton == null)
        {
            Debug.LogError("Return to Main Scene button is null");
            return;
        }

        restartButton.SetActive(false);
        returnToMainButton.SetActive(false);
    }

    public void SetRestart()
    {
        restartButton.SetActive(true);
        returnToMainButton.SetActive(true);
    }

    public void UpdateScore(int score)
    {
        scoreText.text = $"Score: {score}";
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = $"High Score: {highScore}";
    }

    // Restart 버튼 클릭 시 호출
    public void OnRestartButtonClicked()
    {
        Debug.Log("Restarting game...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 현재 씬 다시 로드
    }

    // Return to Main Scene 버튼 클릭 시 호출
    public void OnReturnToMainButtonClicked()
    {
            Debug.Log("Returning to Main Scene...");
            SceneManager.LoadScene("MainScene"); // MainScene으로 이동
    }
}