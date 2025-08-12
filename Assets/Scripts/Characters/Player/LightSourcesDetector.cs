using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[DisallowMultipleComponent]
public class LightSourcesDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Fireable")) return;
        
        Debug.Log(other.gameObject.name);
        
        var fireableBehavior = other.gameObject.GetComponent<FireableBehavior>();
        fireableBehavior.RegisterInFogOfWarList();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Fireable")) return;
        
        var fireableBehavior = other.gameObject.GetComponent<FireableBehavior>();
        fireableBehavior.UnregisterInFogOfWarList();
    }


    //     [Header("What to detect")]
//     [Tooltip("Слой(и) фонарей. Выбери из списка (LayerMask).")]
//     [SerializeField] private LayerMask lightLayers;
//
//     // Живые фонари внутри триггера (без дублей)
//     private HashSet<LightSource> _triggeredLightSources;
//
//     private BoxCollider _trigger;
//
//     private void Awake()
//     {
//         var detectorRigidbody = GetComponent<Rigidbody>();
//         detectorRigidbody.isKinematic = true;
//         detectorRigidbody.useGravity = false;
//         
//         GetComponent<BoxCollider>().isTrigger = true;
//         
//         _triggeredLightSources ??= new HashSet<LightSource>();
//     }
//
//     private void OnEnable()
//     {
//         _triggeredLightSources ??= new HashSet<LightSource>();
//         _triggeredLightSources.RemoveWhere(x => !x);
//     }
//
//     private void OnDisable()
//     {
//         // При отключении — разрегистрировать всё, чтобы контроллер не держал мусор
//         if (FogOfWarController.Instance)
//         {
//             foreach (var lightSource in _triggeredLightSources.Where(x => x))
//             {
//                 FogOfWarController.Instance.Unregister(lightSource);
//             }
//         }
//         _triggeredLightSources.Clear();
//     }
//
//     private void OnTriggerEnter(Collider other)
//     {
//         if (!LayerAllowed(other.gameObject.layer)) return;
//         if (!other.TryGetComponent(out LightSource ls)) return;
//
//         // Можно фильтровать выключенные огни уже на входе
//         if (!ls.isActiveAndEnabled || !ls.IsLightOn) return;
//
//         if (_triggeredLightSources.Add(ls))
//             FogOfWarController.Instance?.Register(ls);
//     }
//
//     private void OnTriggerExit(Collider other)
//     {
//         if (!other.TryGetComponent(out LightSource ls)) return;
//         Debug.Log(ls.gameObject.name);
//
//         if (_triggeredLightSources.Remove(ls))
//             FogOfWarController.Instance?.Unregister(ls);
//     }
//
//     // Если фонарь может включаться/выключаться, пока он ВНУТРИ триггера,
//     // полезно периодически «перепроверять» набор (дёшево — раз в N кадров/сек).
//     [SerializeField, Tooltip("Как часто чистить набор внутри триггера, сек. 0 = не чистить")]
//     private float revalidateInterval = 0.5f;
//     private float _revalidateTimer;
//
//     private void Update()
//     {
//         if (revalidateInterval <= 0f || _triggeredLightSources.Count == 0) return;
//
//         _revalidateTimer -= Time.deltaTime;
//         if (_revalidateTimer > 0f) return;
//         _revalidateTimer = revalidateInterval;
//
//         // Убираем выключенные/уничтоженные
//         var toRemove = ListPool<LightSource>.Get();
//         foreach (var ls in _triggeredLightSources)
//         {
//             if (!ls || !ls.isActiveAndEnabled || !ls.IsLightOn)
//                 toRemove.Add(ls);
//         }
//         foreach (var ls in toRemove)
//         {
//             if (ls) FogOfWarController.Instance?.Unregister(ls);
//             _triggeredLightSources.Remove(ls);
//         }
//         ListPool<LightSource>.Release(toRemove);
//     }
//
//     private bool LayerAllowed(int layer) => (lightLayers.value & (1 << layer)) != 0;
//
// #if UNITY_EDITOR
//     private void OnDrawGizmosSelected()
//     {
//         var col = GetComponent<BoxCollider>();
//         if (!col) return;
//
//         Gizmos.matrix = transform.localToWorldMatrix;
//         Gizmos.color = new Color(0f, 1f, 1f, 0.15f);
//         Gizmos.DrawCube(col.center, col.size);
//         Gizmos.color = new Color(0f, 1f, 1f, 0.9f);
//         Gizmos.DrawWireCube(col.center, col.size);
//     }
// #endif
}

// Простенький пул, чтобы не аллоцировать временные списки каждый раз
static class ListPool<T>
{
    static readonly Stack<List<T>> Pool = new();
    public static List<T> Get() => Pool.Count > 0 ? Pool.Pop() : new List<T>();
    public static void Release(List<T> list) { list.Clear(); Pool.Push(list); }
}