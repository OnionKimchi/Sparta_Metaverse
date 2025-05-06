using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureController : MonoBehaviour
{
    [Header("Treasure Stats")]
    [SerializeField] private int maxHealth = 100; // 트레저의 최대 체력
    private int currentHealth;

    [Header("Sprites")]
    [SerializeField] private SpriteRenderer closedSprite; // 닫힌 상태
    [SerializeField] private SpriteRenderer halfOpenSprite; // 반쯤 열린 상태
    [SerializeField] private SpriteRenderer openSprite; // 완전히 열린 상태

    [Header("Damage Settings")]
    [SerializeField] private float damageInterval = 1f; // 초당 피해 간격
    private List<EnemyController> attachedEnemies = new List<EnemyController>(); // 트레저에 붙어 있는 에너미 목록

    private void Start()
    {
        currentHealth = maxHealth; // 초기 체력 설정
        UpdateSprite(); // 초기 스프라이트 설정
        StartCoroutine(ApplyDamageOverTime()); // 초당 피해 처리 시작
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null && !attachedEnemies.Contains(enemy))
            {
                attachedEnemies.Add(enemy); // 에너미 추가
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null && attachedEnemies.Contains(enemy))
            {
                attachedEnemies.Remove(enemy); // 에너미 제거
            }
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth; // 체력을 최대값으로 초기화
        UpdateSprite(); // 스프라이트 업데이트
        Debug.Log("Treasure health has been reset.");
    }

    private IEnumerator ApplyDamageOverTime()
    {
        while (currentHealth > 0)
        {
            yield return new WaitForSeconds(damageInterval);

            int totalDamage = 0;
            foreach (EnemyController enemy in attachedEnemies)
            {
                if (enemy != null)
                {
                    totalDamage += enemy.AttackPower; // 에너미의 공격력을 합산
                }
            }

            if (totalDamage > 0)
            {
                TakeDamage(totalDamage);
            }
        }
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 체력 제한
        Debug.Log($"Treasure took {damage} damage. Current health: {currentHealth}");

        UpdateSprite();

        if (currentHealth <= 0)
        {
            OnTreasureDestroyed();
        }
    }

    private void UpdateSprite()
    {
        // 체력에 따라 스프라이트 변경
        if (currentHealth > maxHealth * 0.5f)
        {
            SetActiveSprite(closedSprite);
        }
        else if (currentHealth > 0)
        {
            SetActiveSprite(halfOpenSprite);
        }
        else
        {
            SetActiveSprite(openSprite);
        }
    }

    private void SetActiveSprite(SpriteRenderer activeSprite)
    {
        // 모든 스프라이트 비활성화
        closedSprite.gameObject.SetActive(false);
        halfOpenSprite.gameObject.SetActive(false);
        openSprite.gameObject.SetActive(false);

        // 활성화할 스프라이트만 활성화
        activeSprite.gameObject.SetActive(true);
    }

    private void OnTreasureDestroyed()
    {
        Debug.Log("Treasure has been destroyed!");
        // 추가적인 게임 오버 로직을 여기에 구현
    }
}