using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f; // 에너미 이동 속도
    private Transform targetTreasure; // 목표 트레저 오브젝트

    public void SetTarget(Transform treasure)
    {
        targetTreasure = treasure; // 트레저 오브젝트 설정
    }

    private void Update()
    {
        if (targetTreasure == null) return;

        // 트레저를 향해 이동
        Vector2 direction = (targetTreasure.position - transform.position).normalized;
        transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
    }
}