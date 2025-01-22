using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarIndicatorManager : MonoBehaviour
{
    [Header("UnSelectablePillars")]
    [SerializeField] GameObject _leftUSP;
    [SerializeField] GameObject _rightUSP;
    [SerializeField] GameObject _midUSP;

    [Header("SelectablePillars")]
    [SerializeField] GameObject _left;
    [SerializeField] GameObject _right;
    [SerializeField] GameObject _mid;
    [SerializeField] GameObject _midLeft;
    [SerializeField] GameObject _midRight;

    [SerializeField] DeckbuildingManager _deckbuildingManager;
    public void SetTier(string tier)
    {
        ClearUnSelectable();
        ClearSelectable();
        int savedCards = (_deckbuildingManager.CheckIfSavedCards(tier).Length);
        if (tier == "Common" || tier == "Uncommon")
        {
            _midUSP.SetActive(true);
            switch (savedCards)
            {
                case 1:
                    _left.SetActive(true);
                    break;
                case 2:
                    _left.SetActive(true);
                    _midLeft.SetActive(true);
                    break;
                case 3:
                    _left.SetActive(true);
                    _midLeft.SetActive(true);
                    _midRight.SetActive(true);
                    break;
                case 4:
                    _left.SetActive(true);
                    _midLeft.SetActive(true);
                    _midRight.SetActive(true);
                    _right.SetActive(true);
                    break;
            }
        }
        else if (tier == "Rare" || tier == "Epic")
        {
            _leftUSP.SetActive(true);
            _rightUSP.SetActive(true);
            switch (savedCards)
            {
                case 1:
                    _midLeft.SetActive(true);
                    break;
                case 2:
                    _midLeft.SetActive(true);
                    _mid.SetActive(true);
                    break;
                case 3:
                    _midLeft.SetActive(true);
                    _mid.SetActive(true);
                    _midRight.SetActive(true);
                    break;
            }
        }
        else if (tier == "Legendary")
        {
            _leftUSP.SetActive(true);
            _rightUSP.SetActive(true);
            _midUSP.SetActive(true);
            switch (savedCards)
            {
                case 1:
                    _midLeft.SetActive(true);
                    break;
                case 2:
                    _midLeft.SetActive(true);
                    _midRight.SetActive(true);
                    break;
            }
        }
    }

    public void CardAdded(string tier, int cardCount)
    {
        if (tier == "Common" || tier == "Uncommon")
        {
            switch (cardCount)
            {
                case 1: 
                    _left.SetActive(true);
                    break;
                case 2:
                    _midLeft.SetActive(true);
                    break;
                case 3:
                    _midRight.SetActive(true);
                    break;
                case 4:
                    _right.SetActive(true);
                    break;
            }
        }
        else if (tier == "Rare" || tier == "Epic")
        {
            switch (cardCount)
            {
                case 1:
                    _midLeft.SetActive(true);
                    break;
                case 2:
                    _mid.SetActive(true);
                    break;
                case 3:
                    _midRight.SetActive(true);
                    break;
            }
        }
        else if (tier == "Legendary")
        {
            switch (cardCount)
            {
                case 1:
                    _midLeft.SetActive(true);
                    break;
                case 2:
                    _midRight.SetActive(true);
                    break;
            }
        }
    }
    public void CardRemoved(string tier, int cardCount) 
    {
        if (tier == "Common" || tier == "Uncommon")
        {
            switch (cardCount)
            {
                case 0:
                    _left.SetActive(false);
                    break;
                case 1:
                    _midLeft.SetActive(false);
                    break;
                case 2:
                    _midRight.SetActive(false);
                    break;
                case 3:
                    _right.SetActive(false);
                    break;
            }
        }
        else if (tier == "Rare" || tier == "Epic")
        {
            switch (cardCount)
            {
                case 0:
                    _midLeft.SetActive(false);
                    break;
                case 1:
                    _mid.SetActive(false);
                    break;
                case 2:
                    _midRight.SetActive(false);
                    break;
            }
        }
        else if (tier == "Legendary")
        {
            switch (cardCount)
            {
                case 0:
                    _midLeft.SetActive(false);
                    break;
                case 1:
                    _midRight.SetActive(false);
                    break;
            }
        }
    }
    private void ClearUnSelectable()
    {
        _leftUSP.SetActive(false);
        _rightUSP.SetActive(false);
        _midUSP.SetActive(false);
    }
    private void ClearSelectable()
    {
        _left.SetActive(false);
        _right.SetActive(false);
        _mid.SetActive(false);
        _midLeft.SetActive(false);
        _midRight.SetActive(false);
    }
}
