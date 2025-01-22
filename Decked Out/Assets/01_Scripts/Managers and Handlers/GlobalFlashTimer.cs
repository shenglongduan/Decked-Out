using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalFlashTimer : MonoBehaviour
{    
    public static GlobalFlashTimer Instance { get; private set; }
    [SerializeField] float flashDelay = 2f;
    [SerializeField] float flashDuration = 1f;

    private float _globalTimer = 0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        _globalTimer += Time.deltaTime;
    }

    public bool IsFlashing()
    {
        float cycleLength = flashDelay + flashDuration;
        return (_globalTimer % cycleLength) >= flashDelay;
    }

    public void ResetTimer()
    {
        _globalTimer = 0f;
    }
}
