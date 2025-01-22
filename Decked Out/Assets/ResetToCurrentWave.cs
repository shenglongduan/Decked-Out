using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetToCurrentWave : MonoBehaviour
{
    private WaveManager waveManager;

    private void Start()
    {
        waveManager = FindObjectOfType<WaveManager>();
    }
}
