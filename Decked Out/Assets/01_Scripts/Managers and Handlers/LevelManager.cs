// =============================================================================
// 
// Every manager is registered here.
// 
//            
// 
// =============================================================================
using System;
using UnityEngine;
//using static UnityEditor.PlayerSettings;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject _waveManager = null;
    [SerializeField] private MouseInputHandling _mouseInputHandling = null;
    [SerializeField] private PositionUpdater Pos = null;
    [SerializeField] private CardRandoEngine CardRando = null;
    [SerializeField] private Castle castle = null;
    [SerializeField] private EndGameSplashManager _endGameSplash = null;
    [SerializeField] private DeckbuildingManager _deckbuilingManager = null;


    private GameLoader _loader;

    public event Action LevelLoaded;

    private void Start()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        Debug.Log("Registering Wave Manager");
        if (_waveManager != null)
        {
            var wm = Instantiate(_waveManager);
            var waveManager = wm.GetComponent<WaveManager>();
            ServiceLocator.Register<WaveManager>(waveManager.Initialize());
        }

        Debug.Log("Registering MouseInputHandling");
        _mouseInputHandling.Initialize();
       
        //CardRando.Initialize();
        ServiceLocator.Register<MouseInputHandling>(_mouseInputHandling);
       
        ServiceLocator.Register<CardRandoEngine>(CardRando);
        castle.Initialize();
        ServiceLocator.Register<Castle>(castle);
        Pos.Initialize();
        ServiceLocator.Register<PositionUpdater>(Pos);
        _endGameSplash.Initialize();

        LevelLoaded?.Invoke();
    }
}
