using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameSplashManager : MonoBehaviour
{
    public GameObject splashScreen;
    private WaveManager wave_m;
    private GameSpeedManager _gameSpeedManager;
    private TransitionScreenManager _transitionScreenManager;
    public Castle castleGameObject;
    public CardRandoEngine cardRandoEngine;
    public EnemyKillTracker enemyKillTracker;

    public void Initialize()
    {
        wave_m = ServiceLocator.Get<WaveManager>();
        _transitionScreenManager = FindObjectOfType<TransitionScreenManager>();
        _gameSpeedManager = FindObjectOfType<GameSpeedManager>();
        splashScreen.SetActive(false);
        castleGameObject = ServiceLocator.Get<Castle>();
        cardRandoEngine = FindObjectOfType<CardRandoEngine>();
        enemyKillTracker = FindObjectOfType<EnemyKillTracker>();
    }

    public void Death()
    {
        _gameSpeedManager.ActivateControlPanel();
        _gameSpeedManager.ResumeGame();
        splashScreen.SetActive(true);
        enemyKillTracker.EndGame();
        wave_m.StopWave();
    }

    public void Continue()
    {
        castleGameObject.ResetHealth();
        wave_m.AllEnemiesInWaveDestroyed();
        splashScreen.SetActive(false);
    }

    public void MainMenu()
    {
        _transitionScreenManager.StartTranistion("MainMenu");
    }
}
