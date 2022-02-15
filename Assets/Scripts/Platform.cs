﻿using UnityEngine;

// 발판으로서 필요한 동작을 담은 스크립트
public class Platform : MonoBehaviour {
    public GameObject[] obstacles; // 장애물 오브젝트들

    bool stepped = false; // 플레이어 캐릭터가 밟았었는가

    // 컴포넌트가 활성화될때 마다 매번 실행되는 메서드
    private void OnEnable() {
        // 밟힘 상태를 리셋
        stepped = false;

        // 장애물의 수만큼 루프
        for (int i = 0; i < obstacles.Length; i++)
        {
            if (Random.Range(0, 1f) < GameManager.instance.randomObstacle)
            {
                obstacles[i].SetActive(true);
            }
            else
            {
                obstacles[i].SetActive(false);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        // 충돌한 상대방의 태그가 Player이고 이전에 플레이어가 밟지 않았다면
        if (collision.collider.tag == "Player" && !stepped)
        {
            // 점수를 추가하고 밟힘 상태를 참으로 변경
            stepped = true;
            GameManager.instance.AddScore(1);
        }
    }
}