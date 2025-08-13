using System;
using UnityEngine;

public class PlayerModel : CharacterBaseModel
{
    [Header("Dash settings")]
    [SerializeField] private float dashSpeed = 18.0f;
    public float DashSpeed => dashSpeed;
    
    [SerializeField] private float dashCooldown = 1.0f;
    public float DashCooldown => dashCooldown;
    
    [SerializeField] private float dashDuration = 0.5f;
    public float DashDuration => dashDuration;
    
    [SerializeField] private TorchModel torchModel;
    public TorchModel TorchModel => torchModel;
}