using System.Collections.Generic;
using UnityEngine;

public class DefenceGameManager : MonoBehaviour
{
    // 싱글턴 인스턴스
    public static DefenceGameManager Instance { get; private set; }

    public enum EnemyType { Warrior, Elf, Wizard } // 에너미 타입 정의

    [SerializeField] private Transform player; // 플레이어 Transform
    [SerializeField] private Collider2D triggerArea; // 트리거 영역
    [SerializeField] private GameObject warriorPrefab;
    [SerializeField] private GameObject elfPrefab;
    [SerializeField] private GameObject wizardPrefab;
    [SerializeField] private Transform[] spawnPoints; // 스폰 위치 배열
    [SerializeField] private float baseSpawnInterval = 1f; // 기본 스폰 간격
    [SerializeField] private Transform treasure; // 트레저 오브젝트

    private List<EnemyMovementController> enemiesInTrigger = new List<EnemyMovementController>();

    private float currentSpawnInterval; // 현재 스폰 간격
    private int spawnCount = 0; // 소환된 에너미 수
    private int difficulty = 1; // 현재 난이도
    private int playerKillCount = 0; // 플레이어 킬 카운트
    private int enhancementCount = 0; // 강화 횟수
    private bool isPlayerInTrigger = false; // 플레이어가 트리거 안에 있는지 여부
    private float spawnTimer = 0f; // 스폰 타이머
    private bool isInEnhancementPhase = false; // 강화 페이즈 활성화 여부


    private void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Multiple instances of DefenceGameManager detected. Destroying duplicate.");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentSpawnInterval = baseSpawnInterval; // 초기 스폰 간격 설정
    }

    private void Update()
    {
        if (triggerArea.bounds.Contains(player.position))
        {
            isPlayerInTrigger = true; // 플레이어가 영역 내에 있음
        }
        else
        {
            isPlayerInTrigger = false; // 플레이어가 영역을 벗어남
        }

        if (isInEnhancementPhase) return; // 강화 페이즈 중에는 스폰 중단

        if (!isPlayerInTrigger) return; // 플레이어가 트리거 안에 없으면 스폰 중단

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= currentSpawnInterval)
        {
            SpawnEnemy();
            spawnTimer = 0f; // 타이머 초기화
        }
        // 트리거 영역 내 에너미 관리
        Collider2D[] colliders = Physics2D.OverlapBoxAll(triggerArea.bounds.center, triggerArea.bounds.size, 0f);
        enemiesInTrigger.Clear();

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                EnemyMovementController enemy = collider.GetComponent<EnemyMovementController>();
                if (enemy != null)
                {
                    enemiesInTrigger.Add(enemy);
                }
            }
        }

        // 에너미들에게 트레저를 목표로 설정
        foreach (EnemyMovementController enemy in enemiesInTrigger)
        {
            enemy.SetTarget(treasure);
        }
    }

    private void SpawnEnemy()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("DefenceGameManager: No spawn points assigned.");
            return;
        }

        // 난이도에 따라 에너미 타입 결정
        EnemyType enemyType = EnemyType.Warrior; // 기본은 워리어
        if (difficulty >= 10)
        {
            enemyType = (EnemyType)Random.Range(0, 3); // 워리어, 엘프, 위자드
        }
        else if (difficulty >= 5)
        {
            enemyType = (EnemyType)Random.Range(0, 2); // 워리어, 엘프
        }

        // 랜덤한 스폰 위치 선택
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // 에너미 프리팹 선택
        GameObject enemyPrefab = null;
        switch (enemyType)
        {
            case EnemyType.Warrior:
                enemyPrefab = warriorPrefab;
                break;
            case EnemyType.Elf:
                enemyPrefab = elfPrefab;
                break;
            case EnemyType.Wizard:
                enemyPrefab = wizardPrefab;
                break;
        }

        if (enemyPrefab != null)
        {
            // 에너미 생성
            GameObject enemyInstance = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

            // 난이도 초기화
            EnemyController enemyController = enemyInstance.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.Initialize(difficulty); // 현재 난이도 전달
            }

            spawnCount++; // 소환 카운트 증가
            Debug.Log($"Enemy spawned: {enemyType}. Total spawn count: {spawnCount}");

            // 난이도 증가 체크
            if (spawnCount % 30 == 0)
            {
                IncreaseDifficulty();
            }
        }
    }

    private void IncreaseDifficulty()
    {
        difficulty++;
        currentSpawnInterval = Mathf.Max(0.2f, baseSpawnInterval - (difficulty * 0.1f)); // 난이도에 따라 스폰 간격 감소
        Debug.Log($"Difficulty increased to {difficulty}. New spawn interval: {currentSpawnInterval}");
    }

    public void RegisterKill()
    {
        playerKillCount++;
        Debug.Log($"Player kill count: {playerKillCount}");

        // 강화 페이즈 체크
        if (playerKillCount / 20 > enhancementCount)
        {
            StartEnhancementPhase();
        }
    }

    private void StartEnhancementPhase()
    {
        if (isInEnhancementPhase) return; // 이미 강화 페이즈 중이면 중단

        isInEnhancementPhase = true;
        enhancementCount++; // 강화 횟수 증가
        Time.timeScale = 0; // 게임 멈춤
        Debug.Log("Enhancement phase started. Press any key to continue.");

        // 아무 키나 누르면 강화 페이즈 종료
        StartCoroutine(WaitForAnyKey());
    }

    private System.Collections.IEnumerator WaitForAnyKey()
    {
        yield return new WaitUntil(() => Input.anyKeyDown);
        EndEnhancementPhase();
    }

    private void EndEnhancementPhase()
    {
        isInEnhancementPhase = false;
        Time.timeScale = 1; // 게임 재개
        Debug.Log("Enhancement phase ended.");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInTrigger = true; // 플레이어가 영역 내에 있음
        }
        if (collision.CompareTag("Enemy"))
        {
            EnemyMovementController enemy = collision.GetComponent<EnemyMovementController>();
            if (enemy != null && !enemiesInTrigger.Contains(enemy))
            {
                enemiesInTrigger.Add(enemy); // 적 추가
                enemy.SetTarget(treasure); // 트레저를 목표로 설정
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInTrigger = false; // 플레이어가 영역을 벗어남
            ResetGameState(); // 게임 상태 초기화
        }
        if (collision.CompareTag("Enemy"))
        {
            EnemyMovementController enemy = collision.GetComponent<EnemyMovementController>();
            if (enemy != null && enemiesInTrigger.Contains(enemy))
            {
                enemiesInTrigger.Remove(enemy); // 적 제거
            }
        }
    }

    private void ResetGameState()
    {
        Debug.Log("Resetting game state to default values.");

        // 기본값으로 초기화
        currentSpawnInterval = baseSpawnInterval;
        spawnCount = 0;
        difficulty = 1;
        playerKillCount = 0;
        enhancementCount = 0;
        spawnTimer = 0f;

        // 트레저 체력 초기화
        TreasureController treasureController = treasure.GetComponent<TreasureController>();
        if (treasureController != null)
        {
            treasureController.ResetHealth();
        }

        // UIManager 초기화 (필요 시)
        UIManager.Instance.HideButtons(); // 버튼 숨기기
    }
    public void OnTreasureDestroyed()
    {
        Debug.Log("Treasure has been destroyed!");
        Time.timeScale = 0; // 게임 멈춤
        UIManager.Instance.SetRestart(); // UIManager를 통해 리트라이 및 리턴 버튼 표시
    }
}