using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    
    public enum WeaponType
    {
        None,
        Sword,
        Bow,
        Spear,
        Magic
    }

    [Header("Player Stats")]
    [Range(1, 100)][SerializeField] private int health = 100;
    public int Health
    {
        get => health;
        set => health = Mathf.Clamp(value, 0, 100);
    }

    [Range(1f, 20f)][SerializeField] private float speed = 5f;
    public float Speed
    {
        get => speed;
        set => speed = Mathf.Clamp(value, 0, 20);
    }

    [Header("Weapon Stats")]
    [SerializeField] private WeaponType weaponType = WeaponType.None;
    public WeaponType CurrentWeaponType
    {
        get => weaponType;
        set => weaponType = value;
    }

    [SerializeField] private float attackCooldown = 0.5f;
    public float AttackCooldown
    {
        get => attackCooldown;
        set => attackCooldown = Mathf.Max(0, value); // 쿨다운은 0 이상이어야 함
    }

    [Header("Attack Range")]
    [SerializeField] private bool isAttackRangeEnhanced = false; // 공격 각도 강화 여부
    public bool IsAttackRangeEnhanced
    {
        get => isAttackRangeEnhanced;
        set => isAttackRangeEnhanced = value;
    }

    [Header("Weapon Length")]
    [SerializeField] private float bonusWeaponLength = 0f; // 추가 무기 길이
    public float BonusWeaponLength
    {
        get => bonusWeaponLength;
        set => bonusWeaponLength = Mathf.Max(0, value); // 추가 길이는 0 이상이어야 함
    }
}