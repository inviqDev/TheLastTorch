using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask fireableMask;
    [SerializeField] private TorchMovement torchMovement;

    private Input_Actions _inputActions;

    private void OnEnable()
    {
        SingOnInputActions();
    }

    private void SingOnInputActions()
    {
        _inputActions ??= PlayerManager.Instance?.InputActions;
        if (_inputActions == null) return;

        _inputActions.Player.AttackByClick.performed += OnAttackByClickPerformed;
        _inputActions.Enable();
    }

    private void OnAttackByClickPerformed(InputAction.CallbackContext ctx)
    {
        if (torchMovement.State == TorchState.InHand)
        {
            var ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (!Physics.Raycast(ray, out var hit, 100.0f, fireableMask)) return;
            
            torchMovement.LaunchToLightSource(hit.transform.position);
            return;
        }

        if (torchMovement.State is not (TorchState.MovingToLight or TorchState.MovingToEnemy)) return;
        torchMovement.MoveTorchBackToRoot();
    }

    private void UnsignOnInputActions()
    {
        if (_inputActions == null) return;

        _inputActions.Player.AttackByClick.performed -= OnAttackByClickPerformed;
        _inputActions.Disable();
    }

    private void OnDisable()
    {
        UnsignOnInputActions();
    }

    private void OnDestroy()
    {
        UnsignOnInputActions();
    }
}