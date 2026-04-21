using System;
using UnityEditor;
using UnityEngine;

public class UnitHealth : MonoBehaviour, IDamageable
{
    [SerializeField] UnitState unitstate;
    [SerializeField] IUnitAnim anim;
    public float maxHealth { get; private set; }
    public float currentHealth;

    [Header("피격 딜레이 설정")]
    public float damageDelay = 0f;
    float lastDamegeTime = -999f;

    [SerializeField] bool debugMode = false;

    private void Awake()
    {
        if (unitstate == null)
        {
            Debug.Log($"[PlayerAnim] UnitState 연결 안됨. {unitstate}");
            return;
        }
        // 자식요소라 자식에서
        anim = GetComponentInChildren<IUnitAnim>();
        if (anim == null)
        {
            Debug.Log($"[PlayerAnim] IUnitAnim 연결 안됨. {anim}");
            return;
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (debugMode) Debug.Log((Time.time < lastDamegeTime + damageDelay) ? (lastDamegeTime + damageDelay) - Time.time : "피격무시 시간 종료");
    }

    // 스탯에 따른 최대 체력 적용
    public void SetMaxHealth(float amount)
    {
        if (amount <= 0) return;
        maxHealth = amount;
    }

    public void TakeDamage(float amount)
    {
        if (unitstate.state == UNITSTATE.DIE || currentHealth <= 0) return;
        // 피격 가능시간 경과하지않았을시 return 
        if (Time.time < lastDamegeTime + damageDelay) return;

        lastDamegeTime = Time.time;
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        anim.OnDamageTaken();

        if (debugMode) Debug.Log(gameObject.name + " : 데미지 = " + amount);

        if (currentHealth == 0)
        {
            UnitDie();
        }
    }
    void UnitDie()
    {
        unitstate.SetUnitState(UNITSTATE.DIE);
        anim.OnDie();
        // 적이 죽으면 웨이브 카운트감소
        if (((1 << gameObject.layer) & LayerMask.GetMask("Enemy")) != 0) WaveManager.Instance.DecreaseStayEnemy();
    }


}
