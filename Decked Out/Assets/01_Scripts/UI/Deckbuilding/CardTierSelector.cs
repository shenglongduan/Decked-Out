using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

public class CardTierSelector : MonoBehaviour
{
    [SerializeField] DeckbuildingManager _manager;
    [SerializeField] Color _rarityColour;

    [Header("Tier Data")]
    [SerializeField] Sprite _border;
    [SerializeField] string _tier;
    [SerializeField] int _maxCount;

    [Header("Card Data")]
    [SerializeField] SelectedCard[] _cardRenderers;
    [SerializeField] List<TowerCardSO> _cardsOfRarity = new List<TowerCardSO>();

    [Header("UI")]
    [SerializeField] Sprite _selectedTextSprite;
    [SerializeField] Image _textImage;
    [SerializeField] Sprite _normalTextSprite;
    [SerializeField] Image _cardNeededIndicator;

    List<CardTierSelector> _otherTierButtons = new List<CardTierSelector>();
    bool _tierSet = false;

    public void LoadInCardFromSaveSystem(TowerCardSO card)
    {
        _cardsOfRarity.Add(card);
    }

    public void SetTier()
    {
         Debug.Log("Renderering " + _cardsOfRarity.Count + " cards from " + _tier + " tier");
        _cardRenderers = _manager.SetTierRenderers(_cardsOfRarity.Count(), _tier);
        _textImage.sprite = _selectedTextSprite;
        if (_otherTierButtons.Count == 0)
        {
            _otherTierButtons = _manager.tierButtons.ToList();
            if (_otherTierButtons.Contains(this))
            {
                _otherTierButtons.Remove(this);
            }
        }
        foreach (CardTierSelector TierButton in _otherTierButtons)
        {
            TierButton.UnSetTier();
        }
        PresentCards();
        _tierSet = true;
    }
    public void UnSetTier()
    {
        if (_tierSet)
        {
            _textImage.sprite = _normalTextSprite;
            _tierSet = false;
        }
    }
    private void PresentCards()
    {
        _manager.DeactivateGreyOut();
        for (int i = 0; i < _cardRenderers.Length; i++)
        {
            _cardRenderers[i].SlotInCard(_cardsOfRarity[i]);
            _cardRenderers[i].SetBorderSprite(_border);
        }
        for (int i = 0; i < _cardRenderers.Length; i++)
        {
            if (_manager.CheckIfSavedCards(_tier).Contains(_cardRenderers[i].card))
            {
                _cardRenderers[i].ActivateGlow();
            }
        }
        if (_manager.CheckIfSavedCards(_tier).Length >= _maxCount)
        {
            Debug.Log("Checking Saved Cards for Grey Out");
            foreach (SelectedCard cardRenderer in _cardRenderers)
            {
                if (!cardRenderer.selected)
                {
                    cardRenderer.GreyOut(true);
                }
            }
            
        }
        if (_manager.CheckIfSavedCards(_tier).Length > 0)
        {
            DisableCardNeededIndicator();
        }
        else if (_manager.CheckIfSavedCards(_tier).Length == 0)
        {
            EnableCardNeededIndicator();
        }
    }

    public void EnableCardNeededIndicator()
    {
        _cardNeededIndicator.gameObject.SetActive(true);
    }
    public void DisableCardNeededIndicator()
    {
        _cardNeededIndicator.gameObject.SetActive(false);
    }
}
