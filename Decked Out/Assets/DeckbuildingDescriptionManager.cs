using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckbuildingDescriptionManager : MonoBehaviour
{
    [SerializeField] GameObject _tipsPanel;


    public void OpenPanel()
    {
        _tipsPanel.SetActive(true);
    }
    public void ClosePanel()
    {
        _tipsPanel.SetActive(false);
    }
}
