using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ShopUIManager : MonoBehaviour
{
    [SerializeField] GameObject _gemPanel;
    [SerializeField] TextMeshProUGUI _gemAmountText;
    [SerializeField] TextMeshProUGUI _cardsUnlockedText;
    [SerializeField] GameObject _unlockCardsButton;
    [SerializeField] GameObject _cardPanel;
    [SerializeField] DeckbuildingCardStatsPanelManager _cardStatsManager;
    [SerializeField] GameObject _cardStatsPanel;

    [Header("Towers")]
    [SerializeField] private TowerCardSO _archer_Tower;
    [SerializeField] private TowerCardSO _attraction_Tower;
    [SerializeField] private TowerCardSO _cannon;
    [SerializeField] private TowerCardSO _sniper_Tower;
    [SerializeField] private TowerCardSO _frost_Tower;
    [SerializeField] private TowerCardSO _earthquake_Tower;
    [SerializeField] private TowerCardSO _wave_Tower;
    [SerializeField] private TowerCardSO _balista_Tower;
    [SerializeField] private TowerCardSO _buff_Tower;
    [SerializeField] private TowerCardSO _poison_Tower;
    [SerializeField] private TowerCardSO _mystery_Tower;
    [SerializeField] private TowerCardSO _organ_Tower;
    [SerializeField] private TowerCardSO _flamethrower;
    [SerializeField] private TowerCardSO _mortar_Tower;
    [SerializeField] private TowerCardSO _electric_Tower;

    [Header("Spells")]
    [SerializeField] private TowerCardSO _frost;
    [SerializeField] private TowerCardSO _fireball;
    [SerializeField] private TowerCardSO _big_Bomb;
    [SerializeField] private TowerCardSO _lighting_Bolt;
    [SerializeField] private TowerCardSO _freeze;
    [SerializeField] private TowerCardSO _black_Hole;
    [SerializeField] private TowerCardSO _nuke;



    SaveSystem _saveSystem;
    string _newCardName;

    private void Start()
    {
        _saveSystem = FindObjectOfType<SaveSystem>();
        _cardStatsPanel.SetActive(false);
        _unlockCardsButton.SetActive(false);
        UpdateUI();
    }

    public void OpenGemBuyingPanel()
    {
        Debug.Log("Opening Gem Panel");
        _cardPanel.SetActive(false);
        _unlockCardsButton.SetActive(true);
        _gemPanel.SetActive(true);
    }
    public void OpenCardPanel()
    {
        _cardPanel.SetActive(true);
        _unlockCardsButton.SetActive(false);
        _gemPanel.SetActive(false);
    }
    public void RegisterNewCard(string cardName)
    {
        _newCardName = null;
        _newCardName = cardName;
        ShowCaseNewCard(ExtractSOFromTowerName(cardName));
    }
    public void ShowCaseNewCard(TowerCardSO card)
    {
        _cardStatsManager.EnableAndFillStatsPanel(card);
        _cardStatsPanel.SetActive(true);
    }

    public void UpdateUI()
    {
        _gemAmountText.text = _saveSystem.GetGemCount().ToString();
        bool[] bools = _saveSystem.GetAllCardCollected();
        int index = 0;
        foreach (bool cardCollectedBool in bools)
        {
            if (cardCollectedBool)
            {
                index++;
            }
        }
        _cardsUnlockedText.text = index + "<size=50%>of</size> 22";
    }
    private TowerCardSO ExtractSOFromTowerName(string name)
    {
        Debug.Log("Registering " + name);
        switch (name)
        {
            // Towers
            case "Archer_Tower":
                return _archer_Tower;
            case "Attraction_Tower":
                return _attraction_Tower;
            case "Cannon":
                return _cannon;
            case "Sniper_Tower":
                return _sniper_Tower;
            case "Frost_Tower":
                return _frost_Tower;
            case "Earthquake_Tower":
                return _earthquake_Tower;
            case "Wave_Tower":
                return _wave_Tower;
            case "Balista_Tower":
                return _balista_Tower;
            case "Buff_Tower":
                return _buff_Tower;
            case "Poison_Tower":
                return _poison_Tower;
            case "Mystery_Tower":
                return _mystery_Tower;
            case "Organ_Tower":
                return _organ_Tower;
            case "Flamethrower":
                return _flamethrower;
            case "Mortar_Tower":
                return _mortar_Tower;
            case "Electric_Tower":
                return _electric_Tower;

            // Spells
            case "Frost":
                return _frost;
            case "Fireball":
                return _fireball;
            case "Big_Bomb":
                return _big_Bomb;
            case "Lighting_Bolt":
                return _lighting_Bolt;
            case "Freeze":
                return _freeze;
            case "Black_Hole":
                return _black_Hole;
            case "Nuke":
                return _nuke;

            default:
                Debug.LogWarning($"TowerCardSO with name {name} not found.");
                return null;
        }
    }
}
