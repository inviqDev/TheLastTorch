using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] private PlayerModel playerModel;
    public PlayerModel PlayerModel => playerModel;
    
    [Header("Torch throwing settings")]
    [SerializeField] private TorchMovement torchMovement;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask targetMask;
    
    [Header("Light source settings")]
    [SerializeField] private LightSource lightSource;

    private Input_Actions _inputActions;
    public Input_Actions InputActions => _inputActions ??= new Input_Actions();

    public bool PlayerIsAlive { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        
        PlayerIsAlive = true;
        _inputActions ??= new Input_Actions();
    }

    private void Start()
    {
        lightSource.TurnOnLights();
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
