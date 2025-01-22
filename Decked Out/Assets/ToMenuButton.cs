using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToMenuButton : MonoBehaviour
{
    string _sceneToLoadName;
    TransitionScreenManager _transitionManager;

    private void Awake()
    {
        if (_transitionManager == null)
        {
            _transitionManager = FindObjectOfType<TransitionScreenManager>();
        }
    }
    public void LoadScene(string sceneToLoadName)
    {
        _transitionManager.StartTranistion(sceneToLoadName);
    }
}
