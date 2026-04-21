using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HitBoxAnim : MonoBehaviour
{
    [SerializeField] Animator hitAnimator;
    [SerializeField] HitBox hitBox;

    string isHitParamName = "IsHit";
    int isHitHash;

    private void Awake()
    {
        if (hitAnimator == null) 
        {
            Debug.LogError($"[HitBoxAnim] Animator 컴포넌트가 할당되지 않음.");
            return;
        }
        if (hitBox == null) 
        {
            Debug.LogError($"[HitBoxAnim] hitBox 컴포넌트가 할당되지 않음.");
            return;
        }

        // 기본공격 여부
        isHitHash = Animator.StringToHash(isHitParamName);
    }

    void LateUpdate()
    {
        hitAnimator.SetBool(isHitHash, hitBox.isHit);
    }
}
