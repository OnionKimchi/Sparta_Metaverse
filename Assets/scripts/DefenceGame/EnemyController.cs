using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private int spawnDifficulty; // 스폰 시 난이도 저장
    private int health;
    private int attackPower;

    [SerializeField] private GameObject hitboxObject; // 에너미 히트박스 오브젝트
    [SerializeField] private float knockbackForce = 5f; // 넉백 세기
    private AnimationHandler animationHandler; // 애니메이션 핸들러 참조

    // AttackPower 속성 추가
    public int AttackPower
    {
        get => attackPower;
        private set => attackPower = value;
    }


    private void Start()
    {
        if (hitboxObject == null)
        {
            Debug.LogError("EnemyController: Hitbox Object is not assigned.");
        }

        // AnimationHandler 컴포넌트 참조
        animationHandler = GetComponentInChildren<AnimationHandler>();
        if (animationHandler == null)
        {
            Debug.LogError("EnemyController: AnimationHandler is not assigned or missing.");
        }
    }

    public void Initialize(int difficulty)
    {
        spawnDifficulty = difficulty;

        // 난이도에 따라 능력치 설정
        health = 7 + (1 * spawnDifficulty);
        AttackPower = 5 + (2 * spawnDifficulty); // 기본 공격력 5 + 난이도 * 2
    }

    // 에너미가 데미지를 받을 때 호출
    public void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        health -= damage;
        Debug.Log($"Enemy took {damage} damage. Remaining health: {health}");

        // 데미지 애니메이션 재생
        if (animationHandler != null)
        {
            animationHandler.Damage();
        }

        // 넉백 처리
        ApplyKnockback(knockbackDirection);

        if (health <= 0)
        {
            Die();
        }
    }

    private void ApplyKnockback(Vector2 direction)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(direction.normalized * knockbackForce, ForceMode2D.Impulse);
        }
    }

    private void Die()
    {
        Debug.Log("Enemy died.");
        Destroy(gameObject); // 에너미 제거
        DefenceGameManager.Instance.RegisterKill(); // 킬 카운트 증가
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어와 충돌 시
        if (collision.CompareTag("Player"))
        {
            PlayerStatus playerStatus = collision.GetComponent<PlayerStatus>();
            if (playerStatus != null)
            {
                playerStatus.Health -= AttackPower; // 플레이어 체력 감소
                Debug.Log($"Player hit by enemy. Player health: {playerStatus.Health}");

                // 플레이어에게 넉백 적용
                Vector2 knockbackDirection = collision.transform.position - transform.position;
                ApplyKnockbackToPlayer(collision, knockbackDirection);
            }
        }

        // 무기 히트박스와 충돌 시
        if (collision.CompareTag("Weapon"))
        {
            // 플레이어의 공격력을 직접 가져오기
            int damage = PlayerStatus.Instance.AttackPower; // PlayerStatus의 AttackPower 속성 참조
            Vector2 knockbackDirection = transform.position - collision.transform.position;
            TakeDamage(damage, knockbackDirection); // 에너미가 피해를 입음
        }
    }

    private void ApplyKnockbackToPlayer(Collider2D playerCollider, Vector2 direction)
    {
        Rigidbody2D playerRb = playerCollider.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            playerRb.AddForce(direction.normalized * knockbackForce, ForceMode2D.Impulse);
        }
    }
}