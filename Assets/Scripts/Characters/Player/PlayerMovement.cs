using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

using Interfaces;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour, IMovable
{
    private PlayerModel _playerModel;
    private CharacterController _controller;
    private Input_Actions _inputActions;
    
    private bool _isDashing;
    private bool _dashOnCooldown;
    
    private bool _isMoving;
    private Vector3 _moveDirection;

    private void Awake()
    {
        _playerModel = GetComponent<PlayerModel>();
        _controller = GetComponent<CharacterController>();
    }
    
    private void OnEnable()
    {
        _inputActions ??= PlayerManager.Instance?.InputActions;
        if (_inputActions == null) return;
        
        SignOnInputActions();
    }

    private void Update()
    {
        if (!_isMoving || _isDashing) return;
        
        MoveInDirection(_moveDirection * (_playerModel.MoveSpeed * Time.deltaTime));
    }
    
    public void MoveInDirection(Vector3 direction)
    {
        _controller.Move(direction);
    }

    private void SignOnInputActions()
    {
        if (_inputActions == null) return;
        
        _inputActions.Player.Move.started += OnMoveStarted;
        _inputActions.Player.Move.performed += OnMovePerformed;
        _inputActions.Player.Move.canceled += OnMoveCanceled;
        
        _inputActions.Player.Dash.started += OnDashStarted;
    }

    private void OnMoveStarted(InputAction.CallbackContext ctx)
    {
        _isMoving = true;
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        var input = ctx.ReadValue<Vector2>();
        _moveDirection = new Vector3(input.x, _playerModel.Gravity, input.y);
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
        while (time < _playerModel.DashDuration)
        {
            
            MoveInDirection(dashDirection * (_playerModel.DashSpeed * Time.deltaTime));
            time += Time.deltaTime;
            
            yield return null;
        }

        _isDashing = false;
        
        
        yield return new WaitForSeconds(_playerModel.DashCooldown);
        _dashOnCooldown = false;
    }

    private void UnsignFromInputActions()
    {
        if (_inputActions == null) return;
        
        _inputActions.Player.Move.started -= OnMoveStarted;
        _inputActions.Player.Move.performed -= OnMovePerformed;
        _inputActions.Player.Move.canceled -= OnMoveCanceled;
        
        _inputActions.Player.Dash.performed -= OnDashStarted;
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