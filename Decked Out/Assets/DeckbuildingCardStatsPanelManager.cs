using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeckbuildingCardStatsPanelManager : MonoBehaviour
{
    [SerializeField] TowerCardSO _card;
    [Range(0, 0.5f)]
    [SerializeField] float _sliderCheat;

    [Header("Display Stats")]
    [SerializeField] Image _image;
    [SerializeField] Image _border;
    [SerializeField] Image _icon;
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] TextMeshProUGUI _desc;

    [Header("Sliders")]
    [SerializeField] Slider _dmgSlider;
    [SerializeField] Image _dmgFill;
    [SerializeField] TextMeshProUGUI _dmgText;
    [SerializeField] Image _rangeFill;
    [SerializeField] Slider _rangeSlider;
    [SerializeField] TextMeshProUGUI _rangeText;
    [SerializeField] Slider _rofSlider;
    [SerializeField] Image _rofFill;
    [SerializeField] TextMeshProUGUI _rofText;
    [SerializeField] Slider _durationSlider;
    [SerializeField] Image _durationFill;
    [SerializeField] TextMeshProUGUI _durationText;

    Color _rarityColour;
    public void Awake()
    {
        this.gameObject.SetActive(false);
    }
    public void EnableAndFillStatsPanel(TowerCardSO card)
    {
        _card = card;
        this.gameObject.SetActive(true);
        Debug.Log("Slotting in " + card.name + " to deckbuilding stats panel.");
        _rarityColour = _card.rarityColor;
        _image.sprite = _card.image;
        _border.sprite = _card.background;
        _icon.sprite = _card.icon;
        _name.text = _card.towerName;
        _name.color = _rarityColour;
        _desc.text = _card.towerInfo;
        _desc.color = _rarityColour;


        if (_card.damage > 0)
        {
            _dmgSlider.value = (_card.damage / 25) + _sliderCheat;
            _dmgFill.color = _rarityColour;
        }
        else if(_card.damage <= 0)
        {
            _dmgSlider.value = 0;
        }
        _dmgText.text = _card.damage.ToString();

        _rangeSlider.value = (_card.range / 5) + _sliderCheat;
        _rangeText.text = _card.range.ToString();
        _rangeFill.color = _card.rarityColor;

        if (_card.rateOfFire > 0) 
        {
            _rofSlider.value = (_card.rateOfFire / 10) + _sliderCheat;
            _rofFill.color = _rarityColour;
        }
        else if (_card.rateOfFire <= 0)
        {
            _rofSlider.value = 0;
        }
        _rofText.text = _card.rateOfFire.ToString();

        _durationSlider.value = (_card.duration / 10) + _sliderCheat;
        _durationText.text = _card.duration.ToString();
        _durationFill.color = _rarityColour;
    }
    public void DisableStatsPanel()
    {
        this.gameObject.SetActive(false);
    }
}
