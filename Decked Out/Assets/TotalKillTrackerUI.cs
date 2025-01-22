using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TotalKillTrackerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _killText;

    SaveSystem _saveSystem;

    private void Start()
    {
        _saveSystem = FindObjectOfType<SaveSystem>();
        _killText.text = _saveSystem.GetTotalKill().ToString();
    }
}
