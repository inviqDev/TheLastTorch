using UnityEngine;

[DisallowMultipleComponent]
public class LightSource : MonoBehaviour
{
    [field: SerializeField] public float Radius { get; private set; } = 12.0f;
    [SerializeField] private Light lightSource;
    
    public bool IsLightOn { get; private set; }

    private void Awake()
    {
        if (TryGetComponent<Light>(out var lightComponent))
        {
            lightSource = lightComponent;
        }
        
        TurnOffLights();
    }

    // private void OnEnable()
    // {
    //     if (FogOfWarController.Instance)
    //         FogOfWarController.Instance.Register(this);
    // }

    public void TurnOnLights()
    {
        IsLightOn = true;
        lightSource.enabled = true;
        enabled = true;
        
        if (FogOfWarController.Instance)
            FogOfWarController.Instance.Register(this);
    }
    public void TurnOffLights()
    {
        if (FogOfWarController.Instance)
            FogOfWarController.Instance.Unregister(this);
        
        IsLightOn = false;
        lightSource.enabled = false;
        enabled = false;
    }
}