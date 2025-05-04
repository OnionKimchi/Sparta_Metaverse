using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseController
{
    private Camera camera;

    protected override void Start()
    {
        base.Start();
        camera = Camera.main;
    }

    protected override void HandleAction()
    {
        
        if (canMove)
        {
            // 키보드 입력을 기준으로 이동 방향 설정
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            movementDirection = new Vector2(horizontal, vertical).normalized;

            // 디버깅 메시지로 이동 방향 확인
            Debug.Log($"HandleAction: Updated movementDirection to {movementDirection}");
        }
    }
}