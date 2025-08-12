using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class FogOfWarController : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    public static FogOfWarController Instance { get; private set; }

    [Header("Shader Limits")]
    [SerializeField] private int maxLights = 16;

    [Header("Overlay Settings")]
    [SerializeField] private float edgeSoftness = 3f;
    [SerializeField] private Color tintColor = new Color(0,0,0,1.0f);

    private readonly List<LightSource> _sources = new();
    private Material _matInstance;
    private Renderer _rend;

    private static readonly int IdLightCount    = Shader.PropertyToID("_LightCount");
    private static readonly int IdLightPositions= Shader.PropertyToID("_LightPositions");
    private static readonly int IdLightRadii    = Shader.PropertyToID("_LightRadii");
    private static readonly int IdLightSoftness = Shader.PropertyToID("_LightSoftness");
    private static readonly int IdTintColor     = Shader.PropertyToID("_TintColor");

    private Vector4[] _posBuf;
    private float[]   _radBuf;

    private void Awake()
    {
        meshRenderer.enabled = true;
        
        if (Instance && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }
            
        Instance = this;

        _rend = GetComponent<Renderer>();
        _matInstance = _rend.material;

        _posBuf = new Vector4[maxLights];
        _radBuf = new float[maxLights];

        _matInstance.SetFloat(IdLightSoftness, edgeSoftness);
        _matInstance.SetColor(IdTintColor, tintColor);
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void Register(LightSource source)
    {
        if (!_sources.Contains(source)) _sources.Add(source);
    }

    public void Unregister(LightSource source)
    {
        _sources.Remove(source);
    }

    private void LateUpdate()
    {
        Debug.Log($"total lights count : {_sources.Count}");
        
        var count = 0;
        for (var i = 0; i < _sources.Count && count < maxLights; i++)
        {
            var s = _sources[i];
            if (!s || !s.isActiveAndEnabled || !s.IsLightOn) continue;

            var p = s.transform.position;
            _posBuf[count] = new Vector4(p.x, p.y, p.z, 0f);

            if (_sources.Count == 1 && s.Radius < 3.0f)
            {
                Debug.Log("You fall into the completely darkness.. y-y-y-y-y-y ! :D");
                s.TurnOffLights();
            }
            _radBuf[count] = s.Radius;
            count++;
        }

        _matInstance.SetInt(IdLightCount, count);
        _matInstance.SetVectorArray(IdLightPositions, _posBuf);
        _matInstance.SetFloatArray(IdLightRadii, _radBuf);
        _matInstance.SetFloat(IdLightSoftness, edgeSoftness);
        _matInstance.SetColor(IdTintColor, tintColor);
    }
}