using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputReader : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;

    // input action 데이터
    InputAction moveAction;
    InputAction jumpAction;
    InputAction AttackAction;

    // 액션이름 집합
    string moveActionName = "Move";
    string jumpActionName = "Jump";
    string AttackActionName = "Attack";

    // Input 입력값
    public Vector2 MoveVector { get; private set; }
    public bool jumpPressed { get; private set; }
    public bool attackPressed { get; private set; }
    // 입력값 초기화
    public void ResetJump() => jumpPressed = false;    
    public void ResetAttack() => attackPressed = false;

    private void Awake()
    {
        if(playerInput == null) playerInput = GetComponent<PlayerInput>();
        ResolveActions();
    }

    private void Update()
    {
        MoveVector = moveAction != null ? moveAction.ReadValue<Vector2>() : Vector2.zero;
        
        if (jumpAction.WasPressedThisFrame()) jumpPressed = true;
        if (AttackAction.WasPressedThisFrame()) attackPressed = true;
    }


    void ResolveActions()
    {
        if (playerInput == null || playerInput.actions == null)
        { 
            Debug.Log("[PlayerInputReader] PlayerInput 또는 Actions가 할당되지 않음");
            return;
        }

        moveAction = FindAction(moveActionName);
        jumpAction = FindAction(jumpActionName);
        AttackAction = FindAction(AttackActionName);
    }

    InputAction FindAction(string actionName)
    {
        if (string.IsNullOrWhiteSpace(actionName))
        {
            return null;
        }

        InputAction action = playerInput.actions.FindAction(actionName);
        if (action == null)
        {
            Debug.Log($"[PlaeryInputReader] Action 못 찾음 : {actionName}");
            return null;
        }
        return action;
    }

}
