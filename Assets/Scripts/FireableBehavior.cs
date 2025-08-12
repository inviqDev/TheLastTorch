using UnityEngine;

public class FireableBehavior : MonoBehaviour
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

    
    public Color gizmoColor = Color.yellow;
    public int segments = 64; // количество сегментов круга (для плавности)

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        DrawCircle(transform.position, lightSourcePoint.Radius);
    }
    private void DrawCircle(Vector3 center, float radius)
    {
        var angleStep = 360f / segments;
        var prevPoint = Vector3.zero;
        
        for (var i = 0; i <= segments; i++)
        {
            var angle = Mathf.Deg2Rad * (i * angleStep);
            var point = new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius) + center;

            if (i > 0)
                Gizmos.DrawLine(prevPoint, point);

            prevPoint = point;
        }
    }
    
}
