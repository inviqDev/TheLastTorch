using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTorchThrower : MonoBehaviour
{
    [SerializeField] private Transform torch;
    [SerializeField] private float throwSpeed = 24.0f;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask targetMask;

    private Input_Actions _inputActions;

    private Vector3 _targetDirection;
    private bool _isFlyingForward;
    private bool _isFlyingBackwards;

    private void OnEnable()
    {
        _inputActions ??= new Input_Actions();
        
        _inputActions.Player.ThrowTorch.performed += OnFirePerformed;
        _inputActions?.Enable();
    }

    private void Update()
    {
        if (_isFlyingForward)
        {
            transform.Translate(_targetDirection * (throwSpeed * Time.deltaTime));
            if ((_targetDirection - transform.position).sqrMagnitude > 0.1f) return;
            
        }
        
        if (_isFlyingBackwards)
        {
            
        }
        ReturnTorch2();
    }

    private void ReturnTorch2()
    {
        
    }

    private IEnumerator ThrowTorch(Vector3 targetPos)
    {
        _isFlyingForward = true;

        var startPos = torch.position;
        var distance = Vector3.Distance(startPos, targetPos);
        var travelTime = distance / throwSpeed;
        
        var time = 0f;
        while (time < travelTime)
        {
            torch.position = Vector3.Lerp(startPos, targetPos, time / travelTime);
            time += Time.deltaTime;
            
            yield return null;
        }

        torch.position = targetPos;
        _isFlyingBackwards = true;
        
        // yield return StartCoroutine(ReturnTorch(startPos));
    }

    private IEnumerator ReturnTorch(Vector3 playerPos)
    {
        var startPos = torch.position;
        var distance = Vector3.Distance(startPos, playerPos);
        var travelTime = distance / throwSpeed;
        var time = 0f;

        while (time < travelTime)
        {
            torch.position = Vector3.Lerp(startPos, playerPos, time / travelTime);
            time += Time.deltaTime;
            
            yield return null;
        }

        torch.position = playerPos;
        _isFlyingForward = false;
    }
    
    private void OnFirePerformed(InputAction.CallbackContext ctx)
    {
        if (_isFlyingForward) return;

        var clickPosition = Mouse.current.position.ReadValue();
        var ray = mainCamera.ScreenPointToRay(clickPosition);
        if (!Physics.Raycast(ray, out var hit, 100.0f, targetMask)) return;
        
        _targetDirection = hit.transform.position - torch.position;
        _isFlyingForward = true;
        
        // Debug.Log($"hit = {hit.transform.name}");
        // StartCoroutine(ThrowTorch(hit.point));
    }
    
    private void OnDisable()
    {
        if (_inputActions == null) return;
        
        _inputActions.Player.ThrowTorch.performed -= OnFirePerformed;
        _inputActions?.Disable();
    }

    private void OnDestroy()
    {
        if (_inputActions == null) return;
        
        _inputActions.Player.ThrowTorch.performed -= OnFirePerformed;
        _inputActions?.Disable();
    }
}
