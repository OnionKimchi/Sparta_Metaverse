using UnityEngine;
using UnityEngine.SceneManagement; // 씬 전환을 위해 필요

public class EnterPlaneController : MonoBehaviour
{
    [SerializeField] private GameObject popupCanvas; // 팝업 UI 캔버스
    [SerializeField] private string miniGameSceneName = "MiniGameScene"; // 미니게임 씬 이름
    [SerializeField] private Vector3 targetPosition = new Vector3(16.5f, 4f, 0); // 플레이어 이동 좌표

    private GameObject player; // 플레이어 오브젝트 참조
    private BaseController baseController; // 플레이어의 컨트롤러 참조

    private void Start()
    {
        // 플레이어 오브젝트를 태그로 찾습니다.
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("EnterPlaneController: Player object not found! Make sure the Player has the 'Player' tag.");
        }
    }

    private void Update()
    {
        // 팝업 캔버스가 활성화된 동안 키 입력 처리
        if (popupCanvas != null && popupCanvas.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Z)) // Z 키 입력
            {
                StartMiniGame();
            }
            else if (Input.GetKeyDown(KeyCode.X)) // X 키 입력
            {
                ClosePopup();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // 플레이어와 충돌 확인
        {
            MovePlayerToTarget(); // 플레이어를 지정된 좌표로 이동
            ShowPopup(); // 팝업 UI 활성화
        }
    }

    // 플레이어를 지정된 좌표로 이동
    private void MovePlayerToTarget()
    {
        if (player != null)
        {
            player.transform.position = targetPosition; // 플레이어 위치 이동
            Debug.Log($"Player moved to position: {player.transform.position}");
        }
    }

    // 팝업 UI를 활성화
    private void ShowPopup()
    {
        if (popupCanvas != null)
        {
            popupCanvas.SetActive(true); // 팝업 UI 활성화
        }
        if (baseController != null)
        {
            baseController.canMove = false; // 플레이어 이동 비활성화
        }
    }

    // 미니게임 씬으로 전환하는 버튼 이벤트
    public void StartMiniGame()
    {
        ClosePopup(); // 팝업 UI를 닫음
        SceneManager.LoadScene(miniGameSceneName); // 미니게임 씬으로 전환
    }

    // 팝업창을 닫는 버튼 이벤트
    public void ClosePopup()
    {
        if (popupCanvas != null)
        {
            popupCanvas.SetActive(false); // 팝업 UI 비활성화
        }
        if (baseController != null)
        {
            baseController.canMove = true; // 플레이어 이동 활성화
        }
    }
}