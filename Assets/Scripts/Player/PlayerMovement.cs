using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

using Interfaces;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour, IMovable
{
    [Header("Movement settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float gravity = -15.0f;
    
    [Header("Dash settings")]
    [SerializeField] private float dashSpeed = 18.0f;
    [SerializeField] private float dashCooldown = 1.0f;
    [SerializeField] private float dashDuration = 0.5f;
    
    private bool _isDashing;
    private bool _dashOnCooldown;
    
    private CharacterController _controller;
    private Input_Actions _inputActions;
    
    private bool _isMoving;
    private Vector3 _moveDirection;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        _inputActions ??= new Input_Actions();
        SingOnInputActions();
    }

    private void Update()
    {
        if (!_isMoving || _isDashing) return;
        
        MoveInDirection(_moveDirection);
    }
    
    public void MoveInDirection(Vector3 direction)
    {
        _controller.Move(direction * moveSpeed);
    }

    private void SingOnInputActions()
    {
        if (_inputActions == null) return;
        
        _inputActions.Player.Move.started += OnMoveStarted;
        _inputActions.Player.Move.performed += OnMovePerformed;
        _inputActions.Player.Move.canceled += OnMoveCanceled;
        
        _inputActions.Player.Dash.started += OnDashStarted;

        _inputActions.Enable();
    }

    private void OnMoveStarted(InputAction.CallbackContext obj)
    {
        _isMoving = true;
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        var input = ctx.ReadValue<Vector2>();
        _moveDirection = new Vector3(input.x, gravity, input.y).normalized;
    }
    
    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        _isMoving = false;
        _moveDirection = Vector3.zero;
    }
    
    private void OnDashStarted(InputAction.CallbackContext ctx)
    {
        if (_dashOnCooldown || _moveDirection == Vector3.zero) return;
        StartCoroutine(DashRoutine(_moveDirection));
    }
    
    private IEnumerator DashRoutine(Vector3 dashDirection)
    {
        _isDashing = true;
        _dashOnCooldown = true;

        var time = 0f;
        while (time < dashDuration)
        {
            MoveInDirection(dashDirection * dashSpeed);
            time += Time.deltaTime;
            
            yield return null;
        }

        _isDashing = false;
        
        
        yield return new WaitForSeconds(dashCooldown);
        _dashOnCooldown = false;
    }

    private void UnsignFromInputActions()
    {
        if (_inputActions == null) return;
        
        _inputActions.Player.Move.started -= OnMoveStarted;
        _inputActions.Player.Move.performed -= OnMovePerformed;
        _inputActions.Player.Move.canceled -= OnMoveCanceled;
        
        _inputActions.Player.Dash.performed -= OnDashStarted;
        
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