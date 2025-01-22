using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreResets : MonoBehaviour
{
    [SerializeField] GameObject _devPanel;
    SaveSystem _saveSystem;
    ShopUIManager _uiManager;
    bool _edtior;

    void Awake()
    {
        _uiManager = FindObjectOfType<ShopUIManager>();
        _saveSystem = FindObjectOfType<SaveSystem>();
        _edtior = Application.isEditor ? true : false;
        Debug.Log("In editor? : " + _edtior);
    }
    private void Update()
    {
        if (_edtior)
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                Debug.LogWarning("Dev Panel has been activated.");
                _devPanel.gameObject.SetActive(!_devPanel.activeInHierarchy);
            }
        }
    }

    public void ResetCards()
    {
        _saveSystem.ResetCardCollected();
        _uiManager.UpdateUI();
    }
    public void ResetGems()
    {
        _saveSystem.ResetGemCount();
        _uiManager.UpdateUI(); 
    }
    public void ResetTotalKills()
    {
        _saveSystem.ResetTotalKill();
    }
    public void Add100Gems()
    {
        _saveSystem.AddGem(100);
        _uiManager.UpdateUI();
    }
    public void ShowAllUnlockedCards()
    {
        string[] unlockedCardNames = _saveSystem.GetAllCardName();
        bool[] unlockedStatus = _saveSystem.GetAllCardCollected();
        for (int i = 0; i < unlockedStatus.Length; i++)
        {
            if (unlockedStatus[i])
            {
                Debug.Log(unlockedCardNames[i] + " : UNLOCKED");
            }
            else if (!unlockedStatus[i])
            {
                Debug.Log(unlockedCardNames[i] + " : LOCKED");
            }
        }

    }
}
