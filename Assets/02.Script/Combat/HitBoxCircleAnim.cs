using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[RequireComponent(typeof(Animator))]
public class HitBoxCircleAnim : MonoBehaviour
{
    [SerializeField] Animator hitAnimator;
    [SerializeField] HitBoxCircle HiBox;

    string isHitParamName = "IsHit";
    int isHitHash;

    private void Awake()
    {
        if (hitAnimator == null)
        {
            Debug.LogError($"[HitBoxAnim] Animator 컴포넌트가 할당되지 않음.");
            return;
        }
        // 기본공격 여부
        isHitHash = Animator.StringToHash(isHitParamName);
    }

    private void LateUpdate()
    {
        hitAnimator.SetBool(isHitHash, HiBox.isHit);
    }

    public void OnDestory()
    {
        Destroy(gameObject);
    }

}
