using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSpeedManager : MonoBehaviour
{
    [SerializeField] GameObject _panel;
    [SerializeField] float _fastSpeedTimeScale = 1.5f;
    [SerializeField] float _turboSpeedTimeScale = 3f;
    [SerializeField] Transform _activetedSpot;
    [SerializeField] Transform _decativeSpot;

    [Header("UI")]
    [SerializeField] GameObject _timeControlPanel;

    private void Awake()
    {
        DeactiveControlPanel();
        if (_panel == null)
        {
            Debug.LogError("Panel GameObject not found");
        }
        if (_activetedSpot == null)
        {
            Debug.LogError("Active Transform Not Found");
        }
        if (_decativeSpot == null)
        {
            Debug.LogError("Deactive Tranform Not Found");
        }
    }

    public void ActivateControlPanel()
    {
        _panel.transform.position = _activetedSpot.position;
        ResumeGame();
    }
    public void DeactiveControlPanel()
    {
        _panel.transform.position = _decativeSpot.position;
        ResumeGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        Debug.Log("Game Speed at timescale: " + Time.timeScale.ToString());
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        Debug.Log("Game Speed at timescale: " + Time.timeScale.ToString());
    }
    public void FastSpeed()
    {
        Time.timeScale = _fastSpeedTimeScale;
        Debug.Log("Game Speed at timescale: " + Time.timeScale.ToString());
    }
    public void TurboSpeed()
    {
        Time.timeScale = _turboSpeedTimeScale;
        Debug.Log("Game Speed at timescale: " + Time.timeScale.ToString());
    }

}
