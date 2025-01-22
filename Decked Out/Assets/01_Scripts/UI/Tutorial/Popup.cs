using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public class Popup : MonoBehaviour
{
    [Header("Files")]
    [SerializeField] string[] _fileNames;
    [Header("UI")]
    [SerializeField] TextMeshProUGUI _popupText;
    [SerializeField] GameObject _nextButton;
    [Header("Obj to Find")]
    [SerializeField] bool _keepSearchObject;
    [SerializeField] string _searchName;

    TextMeshProUGUI _buttonTextt;
    private string towerName;
    private string towerDesc;
    TutorialManager _manager;
    int _fileIndex = 0;
    private void Awake()
    {
        _buttonTextt = _nextButton.GetComponentInChildren<TextMeshProUGUI>();
        _manager = FindObjectOfType<TutorialManager>();
        if (_fileNames.Length == 0)
        {
            Debug.LogError("No filenames listed");
        }
        else
        {
            LoadText(_fileIndex);
            if (_fileNames.Length > 1)
            {
                _nextButton.SetActive(true);
                Debug.Log(_fileNames.Length);
            }
        }
        
    }
    private void LoadText(int index)
    {        
        TextAsset textAsset = Resources.Load<TextAsset>("Text/" + _fileNames[index]);
        if (textAsset != null)
        {
            string modifiedText = textAsset.text;
            TryGetComponent<TowerSearch>(out TowerSearch towerSearch);
            if (towerSearch != null)
            {
                towerName = towerSearch.GetTowerType();
                towerDesc = towerSearch.GetTowerDesc();
                Debug.Log(towerName);
                modifiedText = modifiedText.Replace("{towerName}", towerName);
                modifiedText = modifiedText.Replace("{desc}", towerDesc);
            }
            _popupText.text = modifiedText;
        }
        
        
        else
        {
            Debug.LogError(_fileNames[index] + " not found.");
        }

    }
    private void Update()
    {
        if (_fileIndex == _fileNames.Length - 1)
        {
            TryGetComponent<TowerSearch>(out TowerSearch towerSearch);
            if (towerSearch != null && _keepSearchObject == false)
            {
                _searchName = towerName +"(Clone)";
            }
            _nextButton.SetActive(false);
            _manager.Waiting(true, _searchName);

        }
    }
    public void NextText()
    {
        _fileIndex++;
        Debug.Log(_fileIndex);
        LoadText(_fileIndex);
        if (_fileIndex == _fileNames.Length -1)
        {
            TryGetComponent<TowerSearch>(out TowerSearch towerSearch);
            if (towerSearch != null && _keepSearchObject == false)
            {
                _searchName = towerName;
            }
            _nextButton.SetActive(false);
            _manager.Waiting(true, _searchName);

        }
    }
}
