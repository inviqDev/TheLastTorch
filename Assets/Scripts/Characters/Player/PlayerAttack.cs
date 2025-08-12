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
        
        _inputActions.Player.ThrowTorch.performed += OnThrowTorchPerformed;
        _inputActions.Enable();
    }

    private void OnThrowTorchPerformed(InputAction.CallbackContext ctx)
    {
        var ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out var hit, 100.0f, fireableMask)) return;
        
        Debug.Log(hit.transform.name);
        torchMovement.LaunchTorch(hit.transform);
    }

    private void UnsignOnInputActions()
    {
        if (_inputActions == null) return;
        
        _inputActions.Player.ThrowTorch.performed -= OnThrowTorchPerformed;
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