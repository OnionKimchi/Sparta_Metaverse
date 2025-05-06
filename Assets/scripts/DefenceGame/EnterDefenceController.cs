using UnityEngine;

public class EnterDefenceController : MonoBehaviour
{
    public static EnterDefenceController Instance { get; private set; } // 싱글턴 인스턴스

    [SerializeField] private GameObject popupCanvas; // 팝업 UI 캔버스
    [SerializeField] private Vector3 targetPosition1 = new Vector3(5f, 16.5f, 0f); // 대기 위치
    [SerializeField] private Vector3 targetPosition2 = new Vector3(79.5f, 3.5f, 0f); // 디펜스 던전 위치

    private GameObject player; // 플레이어 오브젝트 참조

    private void Start()
    {
        // 플레이어 오브젝트를 태그로 찾습니다.
        player = GameObject.FindGameObjectWithTag("Player");
    }


    private void Update()
    {
        // 팝업 캔버스가 활성화된 동안 키 입력 처리
        if (popupCanvas != null && popupCanvas.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Z)) // Z 키 입력
            {
                MovePlayerToTarget2(); // 디펜스 던전 위치로 이동
            }
            else if (Input.GetKeyDown(KeyCode.X)) // X 키 입력
            {
                ClosePopup(); // 팝업 닫기
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // 플레이어와 충돌 확인
        {
            Debug.Log("Player entered the trigger zone.");
            MovePlayerToTarget1(); // 대기 위치로 이동
            ShowPopup(); // 팝업 UI 활성화
        }
    }

    // 플레이어를 대기 위치로 이동
    private void MovePlayerToTarget1()
    {
        if (player != null)
        {
            player.transform.position = targetPosition1; // 플레이어 위치 이동
        }
    }

    // 플레이어를 디펜스 던전 위치로 이동
    public void MovePlayerToTarget2()
    {
        if (player != null)
        {
            player.transform.position = targetPosition2; // 플레이어 위치 이동
        }

        ClosePopup(); // 팝업 닫기
    }

    // 팝업 UI를 활성화
    private void ShowPopup()
    {
        if (popupCanvas != null)
        {
            popupCanvas.SetActive(true); // 팝업 UI 활성화
            Time.timeScale = 0; // 게임 멈춤
        }
    }

    // 팝업창을 닫는 메서드
    public void ClosePopup()
    {
        if (popupCanvas != null)
        {
            popupCanvas.SetActive(false); // 팝업 UI 비활성화
            Time.timeScale = 1; // 게임 재개
        }
    }

    // 버튼 클릭 시 호출되는 메서드
    public void OnStartDefenceGameButtonClicked()
    {
        MovePlayerToTarget2(); // 디펜스 던전 위치로 이동
    }

    public bool IsPopupActive()
    {
        return popupCanvas != null && popupCanvas.activeSelf;
    }
}