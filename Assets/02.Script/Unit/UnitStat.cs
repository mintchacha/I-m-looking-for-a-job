using System;
using UnityEngine;

// 플레이어의 체력과 공격력 등의 스탯을 관리하는 클래스
public class UnitStat : MonoBehaviour
{
    [Header("체력관리 컴포넌트")]
    [SerializeField] UnitHealth health;
    [Header("유닛의 능력치")]
    public UnitStatData statData;
    [Header("처치 점수")] // 일단 사용 x
    public int score;
    [Header("히트박스 범위설정")]
    public float colliderVerticalOffset = 0.5f;
    public float colliderHorizontalOffset = 0.5f;

    private void Awake()
    {
        if (health == null) health = GetComponent<UnitHealth>();
        health.SetMaxHealth(statData.maxHealth);
    }
}
