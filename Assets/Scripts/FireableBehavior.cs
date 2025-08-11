using UnityEngine;

public class FireableBehavior : MonoBehaviour
{
    [SerializeField] private string torchTag = "Torch";
    [SerializeField] private LightSource lightSource;
    [SerializeField] private Light torchSpotLight;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag(torchTag)) return;
        
        lightSource.TurnOnLights();
        torchSpotLight.enabled = true;
        
        other.gameObject.GetComponent<TorchMovement>().MoveTorchBackToRoot();
    }
}
