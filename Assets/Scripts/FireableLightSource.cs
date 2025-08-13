using UnityEngine;

public class FireableLightSource : MonoBehaviour
{
    [SerializeField] private string torchTag = "Torch";
    [SerializeField] private LightSource lightSourcePoint;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag(torchTag)) return;
        
        lightSourcePoint.TurnOnLights();
        other.gameObject.GetComponent<TorchMovement>().MoveTorchBackToRoot();
    }

    public void RegisterInFogOfWarList()
    {
        FogOfWarController.Instance.Register(lightSourcePoint);
    }

    public void UnregisterInFogOfWarList()
    {
        FogOfWarController.Instance.Unregister(lightSourcePoint);
    }

    // Debug - Drawing yellow circle around selected "Light Source" with particular radius
    private readonly int _segments = 64; 
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        DrawCircle(transform.position, lightSourcePoint.Radius);
    }
    private void DrawCircle(Vector3 center, float radius)
    {
        var angleStep = 360.0f / _segments;
        var prevPoint = Vector3.zero;
        
        for (var i = 0; i <= _segments; i++)
        {
            var angle = Mathf.Deg2Rad * (i * angleStep);
            var point = new Vector3(Mathf.Cos(angle) * radius, 0.0f, Mathf.Sin(angle) * radius) + center;

            if (i > 0)
                Gizmos.DrawLine(prevPoint, point);

            prevPoint = point;
        }
    }
    
}
