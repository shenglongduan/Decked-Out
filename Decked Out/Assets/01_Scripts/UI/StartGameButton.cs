using UnityEngine;
using UnityEngine.UI;

public class StartGameButton : MonoBehaviour
{
    [SerializeField] private Button _startButton;

    bool _foundManager;
    WaveManager waveManager;
    private void Awake()
    {
        Initialize();
    }
    private void LateUpdate()
    {
        if (_foundManager == false)
        {
            waveManager = FindObjectOfType<WaveManager>();
            waveManager.SetStartButton(_startButton);
            _foundManager = true;
        }
    }
    private void Initialize() 
    {
        _foundManager = false;
    }

}
