using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    float offsetX;

    void Start()
    {
        if (target == null)
            return;

        offsetX = transform.position.x - target.position.x;
    }

    void Update()
    {
        // 현재 씬 이름 가져오기
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (target == null)
            return;

        // 미니게임 씬에서는 x축만 따라감
        if (currentSceneName == "MiniGameScene")
        {
            Vector3 pos = transform.position;
            pos.x = target.position.x + offsetX;
            transform.position = pos;
        }
        // 메인 씬에서는 x축과 y축 모두 따라감
        else if (currentSceneName == "MainScene")
        {
            Vector3 pos = transform.position;
            pos.x = target.position.x + offsetX;
            pos.y = target.position.y; // y축도 따라감
            transform.position = pos;
        }
    }
}
