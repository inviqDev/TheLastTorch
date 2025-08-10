using System;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] private LightSource lightSource;

    private void Start()
    {
        lightSource.enabled = true;
    }

    private void OnEnable()
    {
        lightSource.enabled = true;
    }
}
