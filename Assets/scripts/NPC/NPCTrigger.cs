using UnityEngine;

public class NPCTrigger : MonoBehaviour
{
    [SerializeField] private GameObject bubbleBox; // 말풍선 오브젝트
    [SerializeField] private string playerTag = "Player"; // 플레이어 태그 (기본값: "Player")

    private void Start()
    {
        // BubbleBox를 비활성화 상태로 설정
        if (bubbleBox != null)
        {
            bubbleBox.SetActive(false);
        }
        else
        {
            Debug.LogError("BubbleBox is not assigned in the Inspector!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어가 TriggerBox에 들어왔을 때
        if (other.CompareTag(playerTag))
        {
            if (bubbleBox != null)
            {
                bubbleBox.SetActive(true); // BubbleBox 활성화
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // 플레이어가 TriggerBox에서 나갔을 때
        if (other.CompareTag(playerTag))
        {
            if (bubbleBox != null)
            {
                bubbleBox.SetActive(false); // BubbleBox 비활성화
            }
        }
    }
}