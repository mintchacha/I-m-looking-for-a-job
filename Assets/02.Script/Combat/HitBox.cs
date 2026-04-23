using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(BoxCollider2D))]
public class HitBox : MonoBehaviour
{
    BoxCollider2D hitBoxCollider;
    Vector2 attackDirection;
    public bool isHit { get; private set; }

    GameObject owner;
    LayerMask targetLayer;

    public string sfxSoundName;
    float damage;

    float verticalSize;
    float horizontalSize;

    [SerializeField] bool debugMode = true;


    private void Update()
    {
        if (debugMode) Debug.Log($"[HitBox] isHit: {isHit}");
    }

    private void OnEnable()
    {
        hitBoxCollider = GetComponent<BoxCollider2D>();
        hitBoxCollider.isTrigger = true;

        if (hitBoxCollider == null)
        {
            Debug.LogError($"[HitBox] BoxCollider2D 컴포넌트가 할당되지 않음.");
            return;
        }

        if(!debugMode) Destroy(gameObject, 0.2f);
    }

    // TODO: 히트박스 초기화 설정
    public void InitializeHitBox(float verticcalSize, float horizontalSize, float damage, Vector2 attackDirection, GameObject owner)
    {
        this.damage = damage;
        this.attackDirection = attackDirection;
        this.owner = owner;
        this.targetLayer = (owner.layer == LayerMask.NameToLayer("Player")) ? 1 << LayerMask.NameToLayer("Enemy") : 1 << LayerMask.NameToLayer("Player"); ;
        this.verticalSize = verticcalSize;
        this.horizontalSize = horizontalSize;
        //this.sprite = sprite;        
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
        Vector2 size = hitBoxCollider.size;
        size.y = verticalSize;
        size.x = horizontalSize;
        hitBoxCollider.size = size;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == owner) return;

        if (((1 << collision.gameObject.layer) & targetLayer) != 0)
        {
            IDamageable target = collision.gameObject.GetComponent<IDamageable>();
            //UnitHealth enemy = target as UnitHealth;

            // 한번의 히트박스가 두번이상 피격되는 것을 방지하기 위해 isHit 체크 (공격 후에는 히트박스 destroy되어서 isHit false바꿀필요는 없음.)
            //if (target != null && !isHit)
            if (target != null)
            {
                SoundManager.Instance.SfxPlay(sfxSoundName);
                // 피격 시 스프라이트모션 활성화
                isHit = true;
                target.TakeDamage(damage);
            }
        }
    }
    //void DestroyHitBox() => Destroy(gameObject);

    private void OnDrawGizmos()
    {
        if (debugMode) GizmoDrow();
    }

    void GizmoDrow()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector2(horizontalSize * transform.lossyScale.x, verticalSize * transform.lossyScale.y));
    }

}
