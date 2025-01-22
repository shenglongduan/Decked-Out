using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckbuildingTransfer : MonoBehaviour
{


    public List<TowerCardSO> cardsList;
    GameLoader _loader;
    DeckbuildingManager _manager;
    CardRandoEngine _cardRandoEngine;

    private void Start()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }
    private void Initialize()
    {
        _manager = ServiceLocator.Get<DeckbuildingManager>();
        GetCardsFromDeckManager();
        _cardRandoEngine = FindObjectOfType<CardRandoEngine>();
        _cardRandoEngine.towerCards = cardsList;
    }

    private void GetCardsFromDeckManager()
    {
        cardsList.AddRange(_manager.commonCards);
        Debug.Log("Found " + _manager.commonCards.Length + " common cards");
        cardsList.AddRange(_manager.uncommonCards);
        Debug.Log("Found " + _manager.uncommonCards.Length + " uncommon cards");
        cardsList.AddRange(_manager.rareCards);
        Debug.Log("Found " + _manager.rareCards.Length + " rare cards");
        cardsList.AddRange(_manager.epicCards);
        Debug.Log("Found " + _manager.epicCards.Length + " epic cards");
        cardsList.AddRange(_manager.legendaryCards);
        Debug.Log("Found " + _manager.legendaryCards.Length + " legendary cards");
    }
}
