using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Light))]
public class LightSource : MonoBehaviour
{
    public float Radius { get; private set; } = 30.0f;
    [SerializeField] private float defaultRadius = 30.0f;
    [SerializeField] private float radiusDecrement;

    private Light _light;
    public bool IsLightOn { get; private set; }

    private void Awake()
    {
        _light = GetComponent<Light>();
        // TurnOffLights();
    }

    // private void Start()
    // {
    //     TurnOnLights();
    // }

    private void Update()
    {
        if (Radius < 3.0f)
        {
            TurnOffLights();
            return;
        }
        
        // _light.intensity -= 1.0f;
        _light.range -= radiusDecrement;
        Radius = _light.range;
    }

    // private void OnEnable()
    // {
    //     TurnOnLights();
    // }

    public void TurnOnLights()
    {
        IsLightOn = true;
        _light.enabled = true;
        enabled = true;
        
        Radius = defaultRadius;
        _light.range = defaultRadius;
        
        if (FogOfWarController.Instance)
            FogOfWarController.Instance.Register(this);
    }
    
    public void TurnOffLights()
    {
        if (FogOfWarController.Instance)
            FogOfWarController.Instance.Unregister(this);
        
        Radius = defaultRadius;
        _light.range = defaultRadius;
        
        IsLightOn = false;
        _light.enabled = false;
        enabled = false;
    }

    // private void OnDisable()
    // {
    //     TurnOffLights();
    // }
    //
    // private void OnDestroy()
    // {
    //     TurnOffLights();
    // }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(gizmosCenter.position, Radius);
    // }
}