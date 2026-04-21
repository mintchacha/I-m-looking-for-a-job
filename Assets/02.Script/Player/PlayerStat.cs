using System;
using UnityEngine;

// 플레이어의 체력과 공격력 등의 스탯을 관리하는 클래스
public class PlayerStat : MonoBehaviour
{
    [Header("체력관리 컴포넌트")]
    [SerializeField] UnitHealth health;
    [Header("유닛의 능력치")]
    public UnitStatData statData;
    [Header("기본공격 컴포넌트")]
    public DefaultAttack defaultAttack;

    private void Awake()
    {
        if (defaultAttack == null) 
        {
            Debug.Log("[PlayerStat] DefaultAttack이 할당되지 않음");
            return;
        }
        if(health == null) health = GetComponent<UnitHealth>();
        health.SetMaxHealth(statData.maxHealth);
    }
}
