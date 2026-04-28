using UnityEngine;

// 더미 오브젝트
public class Dummy : MonoBehaviour, IDamageable
{
    public void TakeDamage(float amount)
    {
        PlayerStat.SpecialEnergeChange(20);        
    }
}
