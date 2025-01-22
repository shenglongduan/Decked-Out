using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSearch : MonoBehaviour
{
    [SerializeField] string _namePlacehold = "{TowerName}";
    [SerializeField] string _descPlacehold = "{Desc}";

    private TutorialManager _manager;
    private Popup _popup;
    private TowerSelection _towerSelection;
    private string _selectedTowerName;

    private void Awake()
    {
        _manager = FindObjectOfType<TutorialManager>();
        _towerSelection = FindObjectOfType<TowerSelection>();
        _popup = GetComponent<Popup>();
    }


    public string GetTowerType()
    {
        Debug.Log("Got Tower Type");
        _selectedTowerName = _towerSelection.towers;
        return _selectedTowerName;
    }
    public string GetTowerDesc()
    {
        Debug.Log("Got Tower Desc for: " + _selectedTowerName);

        // Normalize the tower name to avoid issues with spaces in file paths
        string normalizedTowerName = _selectedTowerName.Replace(" ", "_");

        // Load the text asset from the Resources folder
        TextAsset textDesc = Resources.Load<TextAsset>("Text/TowerDesc/" + normalizedTowerName);

        // Check if the text asset was loaded successfully
        if (textDesc != null)
        {
            return textDesc.text;
        }
        else
        {
            Debug.LogError("Tower description file not found for: " + normalizedTowerName);
            return "Description not available."; // or any other fallback string
        }
    }
}
