using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] string gameScene;
    [SerializeField] string storeScene;
    [SerializeField] GameObject creditsPanel;
    //[SerializeField] GameObject closeCreditsButton;
    //[SerializeField] GameObject minimizeCollider;
    //[SerializeField] Image enemyPageImage;
    //[SerializeField] GameObject _tutorialPrompt;


    public void Start()
    {
        //minimizeCollider.SetActive(false);
        //_tutorialPrompt.SetActive(false);
    }

    public void StartGame()
    {
        //_tutorialPrompt.gameObject.SetActive(true);
        var loadSceneTask = SceneManager.LoadSceneAsync(gameScene);
    }

    public void OpenStore()
    {
        //_tutorialPrompt.gameObject.SetActive(true);
        var loadSceneTask = SceneManager.LoadSceneAsync(storeScene);
    }

    public void StartTutorial()
    {
        GameLoader gameLoader = FindObjectOfType<GameLoader>();
        if (gameLoader != null)
        {
            gameLoader.gameObject.AddComponent<TutorialPassthrough>();
            var loadSceneTask = SceneManager.LoadSceneAsync(gameScene);
        }
        else
        {
            Debug.LogError("Can't find GameLoader.");
        }
    }
    public void NoTutorial()
    {
        var loadSceneTask = SceneManager.LoadSceneAsync(gameScene);
    }
    public void ContinueGame()
    {
        Debug.Log("Continue Game not yet implimented");
    }
    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
        //minimizeCollider.SetActive(true);
    }
    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
    }
    public void OpenSettings()
    {
        FindObjectOfType<AudioManager>().OpenMenu();
        //minimizeCollider.SetActive(true);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
   
}
