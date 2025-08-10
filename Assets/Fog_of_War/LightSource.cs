using UnityEngine;

[DisallowMultipleComponent]
public class LightSource : MonoBehaviour
{
    public float Radius { get; private set; } = 12.0f;
    
    public bool IsLightOn { get; private set; }

    private void Awake()
    {
        TurnOffTorch();
    }

    // private void OnEnable()
    // {
    //     if (FogOfWarController.Instance)
    //         FogOfWarController.Instance.Register(this);
    // }

    public void TurnOnTorch()
    {
        IsLightOn = true;
        
        if (FogOfWarController.Instance)
            FogOfWarController.Instance.Register(this);
    }
    public void TurnOffTorch()
    {
        if (FogOfWarController.Instance)
            FogOfWarController.Instance.Unregister(this);
        
        IsLightOn = false;
    }
}