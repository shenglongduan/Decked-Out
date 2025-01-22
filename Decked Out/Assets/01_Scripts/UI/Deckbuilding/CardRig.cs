using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardRig : MonoBehaviour
{
    public TowerCardSO _card;

    [Header("Card Display")]
    [SerializeField] Image _background;
    [SerializeField] Image _image;
    [SerializeField] Image _icon;
    [SerializeField] TextMeshProUGUI _name;
    
    public void SetCard(TowerCardSO card)
    {
        _card = card;
        _background.sprite = _card.background;
        _image.sprite = _card.image;
        _icon.sprite = _card.icon;
        _name.text = _card.name;
    }
    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePos;
    }

}
