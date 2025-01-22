using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameObject[] _popups;
    [Range(0f, 0.25f)]
    [SerializeField] float _checkTimer;

    private GameLoader _loader;
    public int _index = 0;
    private float _timer;
    private bool _tutorial;
    private bool _waiting = false;
    private string _searchName;
    private GameObject _waitingObject;
    public string _selectedTowerName;

    private void Start()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }

    public void Waiting(bool waiting, string searchName)
    {
        _waiting = waiting;
        _searchName = searchName;
    }

    private void Initialize()
    {
        TutorialPassthrough passthrough = FindObjectOfType<TutorialPassthrough>();
        if (passthrough != null)
        {
            _tutorial = true;
            LoadPopup();
            _timer = _checkTimer;
        }
    }
    private void LoadPopup()
    {
        _popups[_index].SetActive(true);
    }
    private void Update()
    {
        if (_tutorial && _waiting == true)
        {
            _timer -= Time.deltaTime;
            if (_searchName == null)
            {
                Debug.LogWarning("Tutorial Object Search Name is blank, is something else searching for an object?");
            }
            if (_waiting && _timer <= 0)
            {
                _waitingObject = GameObject.Find(_searchName);
                _timer = _checkTimer;
                Debug.Log("Checking for:" +_searchName);
            }
            if (_waitingObject != null && _waitingObject.activeInHierarchy)
            {
                _waiting = false;
                _waitingObject = null;
                _popups[_index].gameObject.SetActive(false);
                _index++;
                LoadPopup();
                
            }
            else if (_waitingObject != null && _waitingObject.activeInHierarchy == false)
            {
                Debug.LogError("Found Waiting Object, but is not active");
            }
        }     
              

    }

}
