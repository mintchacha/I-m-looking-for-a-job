using System.Collections.Generic;
using UnityEngine;

public class DefaultAttack : MonoBehaviour
{
    public int id;
    public float damageMultiplier;
    public int maxComboCount;
    public float attckDistance = 0.3f;

    [Header("피격 범위 설정")]

    public float colliderVerticalOffset;
    public float colliderHorizontalOffset;

}