using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialPassthrough : MonoBehaviour
{
    [SerializeField] string _deckbuildingScene = "DeckBuilding";
    [SerializeField] string _gameScene = "SampleScene";

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // This method is called when the scene loads.
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == _deckbuildingScene)
        {
            Debug.Log("Deckbuilding Scene Loaded.");
            GameObject coverup = GameObject.Find("Tutorial Coverup");
            coverup.SetActive(true);
        }
    }

}
