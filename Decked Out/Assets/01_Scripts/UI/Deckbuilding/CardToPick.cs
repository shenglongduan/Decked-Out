using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardToPick : MonoBehaviour
{
    public TowerCardSO _card;

    [Header("Card Display")]
    [SerializeField] Image _background;
    [SerializeField] Image _image;
    [SerializeField] Image _icon;
    [SerializeField] TextMeshProUGUI _name;

    [Header("Card To Drag Around")]
    [SerializeField] GameObject _cardRig;
    [SerializeField] float _cardHoldTime;

    bool _isPressed = false;
    float _timer;
    public void UpdateUI()
    {
        if (_card != null)
        {
            _image.enabled = true;
        }
        else
        {
            _image.enabled = false;
        }
        _background.sprite = _card.background;
        _image.sprite = _card.image;
        _icon.sprite = _card.icon;
        _name.text = _card.name;
        _name.color = _card.rarityColor;
        
    }

    public void SetCard(TowerCardSO card)
    {
        _card = card;
        UpdateUI();
    }
    public void Clicked()
    {
        _isPressed = true;
    }
    private void Update()
    {
        if (_isPressed)
        {
            _timer += Time.deltaTime;
            if (_timer >= _cardHoldTime)
            {
                //DragOff();
            }
            else
            {
                SlotIn();
            }
        }
    }

    public void SlotIn()
    {
        SelectedCard[] selectedCardsArray = FindObjectsOfType<SelectedCard>();
        _isPressed = false;
        foreach (SelectedCard selectedCard in selectedCardsArray)
        {
            if (!selectedCard.selected)
            {
                selectedCard.SlotInCard(_card);
            }
            else
            {
                continue;
            }
            break;
        }
    }
    public void DragOff()
    {
        if (_isPressed)
        {
            Debug.Log("Dragged Off");
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject cardRig = Instantiate(_cardRig, mousePos, Quaternion.identity, FindObjectOfType<Canvas>().transform);
            if (cardRig.activeInHierarchy)
            {
                _isPressed = false;
            }
            CardRig cardRigScript = cardRig.GetComponent<CardRig>();
            cardRigScript.SetCard(_card);
        }
        
    }
}
