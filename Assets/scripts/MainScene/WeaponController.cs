using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject weaponPivot; // Weapon Pivot 오브젝트
    public float attackRange = 45f; // 기본 공격 각도

    private bool isAttacking = false;

    // BaseController 참조
    private BaseController baseController;

    // PlayerStatus 참조
    private PlayerStatus playerStatus;

    private float currentAttackCooldown; // 현재 공격 쿨다운

    void Awake()
    {
        // BaseController 컴포넌트 참조
        baseController = GetComponentInParent<BaseController>();

        // PlayerStatus 컴포넌트 참조
        playerStatus = GetComponentInParent<PlayerStatus>();
        if (playerStatus == null)
        {
            Debug.LogError("WeaponController: PlayerStatus component is missing!");
        }

        // Weapon Pivot이 설정되지 않은 경우, 자식 오브젝트에서 찾기
        if (weaponPivot == null)
        {
            weaponPivot = transform.Find("WeaponPivot")?.gameObject;
            if (weaponPivot == null)
            {
                Debug.LogError("WeaponController: WeaponPivot is missing!");
            }
        }
    }

    void Start()
    {
        // Weapon Pivot의 초기 길이 설정
        UpdateWeaponLength();

        // 초기 공격 쿨다운 설정
        UpdateAttackCooldown();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isAttacking)
        {
            if (playerStatus != null && playerStatus.CurrentWeaponType == PlayerStatus.WeaponType.Sword)
            {
                StartCoroutine(SwingWeapon());
            }
        }
    }

    // Weapon Pivot의 길이를 업데이트
    public void UpdateWeaponLength()
    {
        if (weaponPivot != null && playerStatus != null)
        {
            // Weapon Pivot의 기본 길이(1f)에 추가 길이를 더하여 최종 길이 설정
            Vector3 newScale = weaponPivot.transform.localScale;
            newScale.y = 1f + playerStatus.BonusWeaponLength; // 기본 길이 + 추가 길이
            weaponPivot.transform.localScale = newScale;

            Debug.Log($"Weapon Pivot length updated: {newScale.y}");
        }
    }

    // 공격 쿨다운을 업데이트
    public void UpdateAttackCooldown()
    {
        if (playerStatus != null)
        {
            currentAttackCooldown = playerStatus.AttackCooldown;
            Debug.Log($"Attack Cooldown updated: {currentAttackCooldown}");
        }
    }

    IEnumerator SwingWeapon()
    {
        // 공격 쿨다운과 Weapon Pivot 길이를 업데이트
        UpdateAttackCooldown();
        UpdateWeaponLength();
        // 공격 각도를 업데이트
        if (playerStatus != null && playerStatus.IsAttackRangeEnhanced)
        {
            attackRange = 180f; // 공격 각도 강화
        }
        else
        {
            attackRange = 45f; // 기본 공격 각도
        }

        isAttacking = true; // 공격 상태로 설정
        weaponPivot.SetActive(true); // 무기 회전 축 활성화

        // BaseController의 rotZ 값을 가져옵니다.
        float rotZ = baseController != null ? baseController.rotZ : 0f;

        // 공격 시작 각도와 끝 각도를 설정
        float startAngle = rotZ - attackRange - 90f; // 공격 시작 각도
        float endAngle = rotZ + attackRange - 90f;   // 공격 끝 각도

        // 공격 애니메이션의 총 지속 시간 (초 단위)
        float duration = 0.1f; // 무기를 휘두르는 데 걸리는 총 시간
        float elapsed = 0f; // 애니메이션이 시작된 이후 경과 시간

        // 공격 애니메이션 실행
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float angle = Mathf.Lerp(startAngle, endAngle, t);
            weaponPivot.transform.rotation = Quaternion.Euler(0, 0, angle);

            elapsed += Time.deltaTime;
            yield return null;
        }

        weaponPivot.SetActive(false); // 무기 회전 축 비활성화

        // 공격 쿨다운
        yield return new WaitForSeconds(currentAttackCooldown);

        isAttacking = false; // 공격 상태 해제
    }
}