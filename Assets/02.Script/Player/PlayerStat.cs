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
    [Header("필살기 컴포넌트")]
    public SpecialAttack specialAttack;
    [Header("필살기 게이지")]
    public static float specialAttackEnerge;
    public bool isSpecialAttack = false;

    private void Awake()
    {
        if (defaultAttack == null) 
        {
            Debug.Log("[PlayerStat] DefaultAttack이 할당되지 않음");
            return;
        }
        if (specialAttack == null) 
        {
            Debug.Log("[PlayerStat] specialAttack 할당되지 않음");
            //return;
        }
        if(health == null) health = GetComponent<UnitHealth>();
        health.SetMaxHealth(statData.maxHealth);
    }

    private void Update()
    {
        isSpecialAttack = (specialAttackEnerge >= specialAttack.needEnerge) ? true : false;
    }
    public static void SpecialEnergeChange(float amount) 
    {
        if (amount <= 0) return;
        specialAttackEnerge = Mathf.Clamp(specialAttackEnerge += amount, 0, 100);
    }
}
