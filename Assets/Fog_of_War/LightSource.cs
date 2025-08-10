using Fog_of_War;
using UnityEngine;

[DisallowMultipleComponent]
public class LightSource : MonoBehaviour
{
    [Min(0f)] [SerializeField] public float radius = 12.0f;
    public bool IsLightOn { get; private set; } = true;

    private void Awake()
    {
        enabled = false;
    }

    private void OnEnable()
    {
        if (FogOfWarController.Instance)
            FogOfWarController.Instance.Register(this);
    }

    private void OnDisable()
    {
        if (FogOfWarController.Instance)
            FogOfWarController.Instance.Unregister(this);
    }
}