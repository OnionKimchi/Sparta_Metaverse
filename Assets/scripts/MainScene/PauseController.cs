using UnityEngine;

public class PauseController : MonoBehaviour
{
    public static PauseController Instance { get; private set; } // 싱글턴 인스턴스

    [SerializeField] private GameObject pauseMenuUI; // 퍼즈 메뉴 UI
    private bool isPaused = false; // 퍼즈 상태

    private void Awake()
    {
        // 싱글턴 설정
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Tab 또는 Esc 키 입력으로 퍼즈 상태 전환
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        if (IsAnyUIActive()) return; // 다른 UI가 활성화된 경우 퍼즈 불가

        isPaused = true;
        Time.timeScale = 0; // 게임 일시정지
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true); // 퍼즈 메뉴 활성화
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1; // 게임 재개
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false); // 퍼즈 메뉴 비활성화
        }
    }

    private bool IsAnyUIActive()
    {
        // UIManager, EnterPlaneController, EnterDefenceController의 UI 상태 확인
        if (UIManager.Instance != null && UIManager.Instance.IsAnyUIActive())
        {
            return true;
        }

        if (EnterPlaneController.Instance != null && EnterPlaneController.Instance.IsPopupActive())
        {
            return true;
        }

        if (EnterDefenceController.Instance != null && EnterDefenceController.Instance.IsPopupActive())
        {
            return true;
        }

        return false;
    }
}