using System.Collections.Generic;
using UnityEngine;

public class DefaultAttack : MonoBehaviour
{
    public int id;
    public float damageMultiplier;
    public int maxComboCount;
    //public Sprite attackSprite; 이펙트 히트박스로 통일

    [Header("피격 범위 설정")]

    public float colliderVerticalOffset;
    public float colliderHorizontalOffset;

}