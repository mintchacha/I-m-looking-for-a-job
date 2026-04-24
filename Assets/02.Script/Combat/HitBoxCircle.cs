using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class HitBoxCircle : MonoBehaviour
{
    CircleCollider2D hitBoxCollider;
    Vector2 attackDirection;
    public bool isHit { get; private set; }

    GameObject owner;
    LayerMask targetLayer;

    public string sfxSoundName;
    float damage;

    float radius;

    [SerializeField] bool debugMode = true;

    private void OnEnable()
    {
        hitBoxCollider = GetComponent<CircleCollider2D>();
        hitBoxCollider.isTrigger = true;

        if (hitBoxCollider == null)
        {
            Debug.LogError($"[HitBox] BoxCollider2D 컴포넌트가 할당되지 않음.");
            return;
        }
        isHit = true;
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.SfxPlay(sfxSoundName);
        }

        if (!debugMode) Destroy(gameObject, 0.5f);
    }

    // TODO: 히트박스 초기화 설정
    public void InitializeHitBox(float radius, float damage, Vector2 attackDirection, GameObject owner)
    {
        this.damage = damage;
        this.attackDirection = attackDirection;
        this.owner = owner;
        this.targetLayer = (owner.layer == LayerMask.NameToLayer("Player")) ? 1 << LayerMask.NameToLayer("Enemy") : 1 << LayerMask.NameToLayer("Player"); ;
        this.radius = radius;        
        if (owner == null)
        {
            Debug.LogError("[HitBox] owner가 null입니다.");
            return;
        }

        if (targetLayer == -1)
        {
            Debug.LogError("[HitBox] Player 또는 Enemy 레이어가 존재하지 않습니다.");
            return;
        }

        // instance 하고 initialize 에서 값 받아오니까 awake나 OnEnable 에서 설정하면 안됨.        
        hitBoxCollider.radius = radius;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == owner) return;

        if (((1 << collision.gameObject.layer) & targetLayer) != 0) {
            Debug.Log("영역적중");
            owner.GetComponent<PlayerStat>().specialAttack.Hit(collision.gameObject);
        }

    }

    private void OnDrawGizmos()
    {
        if (debugMode) GizmoDrow();
    }

    void GizmoDrow()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}
