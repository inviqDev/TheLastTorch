using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : Singleton<PlayerManager>
{
    [Header("Torch throwing settings")]
    [SerializeField] private TorchMovement torchMovement;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask targetMask;
    
    [Header("Light source settings")]
    [SerializeField] private LightSource lightSource;

    private Input_Actions _inputActions;

    private void Start()
    {
        lightSource.TurnOnTorch();
    }

    private void OnEnable()
    {
        _inputActions ??= new Input_Actions();
        
        _inputActions.Player.ThrowTorch.performed += OnThrowTorchPerformed;
        _inputActions.Enable();
    }

    private void OnThrowTorchPerformed(InputAction.CallbackContext ctx)
    {
        var ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out var hit, 100.0f, targetMask)) return;
        torchMovement.LaunchTorch(hit.transform);
    }

    private void OnDisable()
    {
        if (_inputActions == null) return;
        
        _inputActions.Player.ThrowTorch.performed -= OnThrowTorchPerformed;
        _inputActions.Enable();
    }

    private void OnDestroy()
    {
        if (_inputActions == null) return;
        
        _inputActions.Player.ThrowTorch.performed -= OnThrowTorchPerformed;
        _inputActions.Enable();
    }
}
