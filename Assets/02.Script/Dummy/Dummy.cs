using UnityEngine;

// 더미 오브젝트
public class Dummy : MonoBehaviour, IDamageable
{
    public void TakeDamage(float amount)
    {
        //if (((1 << gameObject.layer) & LayerMask.GetMask("Enemy")) != 0)
        Debug.Log(1);
        PlayerStat.SpecialEnergeChange(20);        
    }
}
