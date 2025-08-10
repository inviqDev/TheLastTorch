using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Movement))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement settings")]
    [SerializeField] private float speed;

    private Movement _movementComponent;
    private Input_Actions _inputActions;
    
    private Vector3 _moveDirection;
    private bool _isMoving;

    private void Awake()
    {
        _movementComponent = GetComponent<Movement>();
    }

    private void OnEnable()
    {
        _inputActions ??= new Input_Actions();
        SingOnInputActions();
    }

    private void Update()
    {
        if (!_isMoving) return;
        
        _movementComponent.MoveInDirection(_moveDirection * speed);
    }

    private void SingOnInputActions()
    {
        if (_inputActions == null) return;
        
        _inputActions.Player.Move.started += OnMoveStarted;
        _inputActions.Player.Move.performed += OnMovePerformed;
        _inputActions.Player.Move.canceled += OnMoveCanceled;
        
        _inputActions.Enable();
    }

    private void OnMoveStarted(InputAction.CallbackContext obj)
    {
        _isMoving = true;
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        var input = ctx.ReadValue<Vector2>();
        _moveDirection = new Vector3(input.x, -15.0f, input.y).normalized;
    }
    
    private void OnMoveCanceled(InputAction.CallbackContext obj)
    {
        _isMoving = false;
    }

    private void UnsignFromInputActions()
    {
        if (_inputActions == null) return;
        
        _inputActions.Player.Move.started -= OnMoveStarted;
        _inputActions.Player.Move.performed -= OnMovePerformed;
        _inputActions.Player.Move.canceled -= OnMoveCanceled;
        
        _inputActions?.Disable();
    }

    private void OnDisable()
    {
        UnsignFromInputActions();
    }

    private void OnDestroy()
    {
        UnsignFromInputActions();
    }
}