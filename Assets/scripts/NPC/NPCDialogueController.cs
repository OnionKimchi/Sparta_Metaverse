using UnityEngine;
using TMPro; // TextMeshPro를 사용하기 위해 필요

public class NPCDialogueController : MonoBehaviour
{
    public enum NPCName
    {
        Lizard1
    }

    [SerializeField] private NPCName npcName; // NPC 이름
    [SerializeField] private TextMeshProUGUI dialogueText; // 대사를 표시할 TextMeshPro 오브젝트

    private void Start()
    {
        if (dialogueText == null)
        {
            Debug.LogError("NPCDialogueController: TextMeshProUGUI is not assigned!");
            return;
        }

        UpdateDialogue(); // 초기 대사 설정
    }

    public void UpdateDialogue()
    {
        switch (npcName)
        {
            case NPCName.Lizard1:
                int highScore = MiniGameManager.Instance != null ? MiniGameManager.Instance.highScore : -1;
                dialogueText.text = $"Your highscore is {highScore}!";
                break;

            default:
                dialogueText.text = "Hello!";
                break;
        }
    }
}