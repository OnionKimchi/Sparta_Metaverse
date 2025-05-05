using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager Instance { get; private set; } // Singleton Instance
    public int highScore = 0;

    private int currentScore = 0;
    private UIManager uiManager;

    public UIManager UIManager
    {
        get { return uiManager; }
    }

    private void Awake()
    {
        // Singleton 패턴 구현
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject); // 이미 Instance가 존재하면 중복된 오브젝트를 파괴
        }
    }

    private void Start()
    {
        // UIManager를 동적으로 찾음
        uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            uiManager.UpdateScore(0);
        }
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

        if (uiManager != null)
        {
            uiManager.UpdateScore(currentScore);
        }
    }
}