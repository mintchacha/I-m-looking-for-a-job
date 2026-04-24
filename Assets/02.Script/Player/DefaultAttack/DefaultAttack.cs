using System.Collections.Generic;
using UnityEngine;

public class DefaultAttack : MonoBehaviour
{
    public int id;
    public float damageMultiplier;
    public int maxComboCount;

    [Header("기본공격")]
    public HitBox hitBox;
    [Header("피격 방향 설정")]
    public float attckDistance = 0.3f;
    [Header("피격 범위 설정")]
    public float colliderVerticalOffset;
    public float colliderHorizontalOffset;

    [Header("점프공격")]
    public FollowHitBox JumpHitBox;
    [Header("점프 피격 방향 설정")]
    public float JumpVerticalAttckDistance = 0.3f;
    public float JumpHorizontalAttckDistance = 0.3f;
    [Header("점프 피격 범위 설정")]
    public float JumpColliderVerticalOffset;
    public float JumpColliderHorizontalOffset;

    private void Awake()
    {
        if (hitBox == null)
        {
            Debug.Log("[BattleManager] hitBox가 할당되지 않음");
            return;
        }
        if (JumpHitBox == null)
        {
            Debug.Log("[BattleManager] JumpHitBox 할당되지 않음");
            return;
        }
    }
}