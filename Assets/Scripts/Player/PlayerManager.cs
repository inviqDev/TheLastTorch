using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : Singleton<PlayerManager>
{
    [Header("Torch throwing settings")]
    [SerializeField] private TorchMovement torchMovement;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask targetMask;
    
    [SerializeField] private LightSource lightSource;

    private Input_Actions _inputActions;
    
    protected override void Awake()
    {
        base.Awake();
        
        lightSource.enabled = false;
    }

    private void Start()
    {
        lightSource.enabled = true;
    }

    private void OnEnable()
    {
        _inputActions ??= new Input_Actions();
        
        _inputActions.Player.ThrowTorch.performed += OnThrowTorchPerformed;
        _inputActions.Enable();
        
        lightSource.enabled = true;
    }

    private void OnThrowTorchPerformed(InputAction.CallbackContext ctx)
    {
        var ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out var hit, 100.0f, targetMask)) return;
        
        var targetDirection = (hit.transform.position - torchMovement.transform.position).normalized;
        torchMovement.LaunchByPlayerClick(targetDirection);
    }

    private void OnDisable()
    {
        lightSource.enabled = false;
    }
}
