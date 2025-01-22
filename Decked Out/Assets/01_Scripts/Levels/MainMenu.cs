using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Button _startGameButton;

    private GameLoader loader = null;

    private void Awake()
    {
        loader = ServiceLocator.Get<GameLoader>();
        loader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {

        _startGameButton.onClick.AddListener(OnStartGameClicked);
    }

    private void OnStartGameClicked()
    {
        var loadSceneTask = SceneManager.LoadSceneAsync("Level_001");
    }
}
