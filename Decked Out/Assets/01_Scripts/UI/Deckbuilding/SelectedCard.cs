using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedCard : MonoBehaviour
{
    [SerializeField] TowerCardSO _card;

    [Header("Card Base Display")]
    [SerializeField] Image _border;
    [SerializeField] Image _image;
    [SerializeField] Image _icon;
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] Image _greyOut;
    [SerializeField] Color _gold;

    [Header("Card Stats Display")]
    [SerializeField] Button _button;
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
    [SerializeField] float _sliderCheat = 0.15f;

    [Header("Misc")]
    [SerializeField] float _longPressTimer = 2f;
    [SerializeField] DeckbuildingCardStatsPanelManager _cardStatsPanel;

    bool _selected = false;
    bool _timerEnabled;
    float _timer;
    Color _rarityColour;
    Sprite _glowBorder;
    Sprite _baseBorder;

    public TowerCardSO card { get { return _card; } }
    public bool selected { get { return _selected; } }

    GameLoader _loader;
    DeckbuildingManager _manager;
    private void Start()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
        _manager = FindObjectOfType<DeckbuildingManager>();
        _glowBorder = _manager._glowBorder;
    }
    private void Initialize()
    {
        //DisableUI();
    }

    public void SlotInCard(TowerCardSO card)
    {
        if (!_selected)
        {
            Debug.Log(card + " :slotted in");
            _card = card;
            UpdateUI();
            _timerEnabled = false;
        }
       
    }
    public void Unslot()
    {
        _card = null;
        _selected = false;
        DisableUI();
    }
    public void SetBorderSprite(Sprite border)
    {
        _border.sprite = border;
        _baseBorder = border;
    }
    private void UpdateUI()
    {
        //Selected Effects Clear
        if (!_selected)
        {
            _border.transform.localScale = Vector3.one;
            _baseBorder = _border.sprite;
            _icon.color = Color.white;
        }
        _rarityColour = _card.rarityColor;
        _border.enabled = true;
        _image.enabled = true;
        _icon.enabled = true;
        _name.enabled = true;
        //_dmgText.enabled = true;
        //_rangeText.enabled = true;
        //_rofText.enabled = true;
        //_durationText.enabled = true;
        _border.sprite = _card.background;
        _image.sprite = _card.image;
        _icon.sprite = _card.icon;
        _name.text = _card.towerName;
        _name.color = _rarityColour;

        if (_card.damage > 0)
        {
            //_dmgSlider.value = (_card.damage / 25) + _sliderCheat;
            //_dmgFill.color = _rarityColour;
            //_dmgText.text = _card.damage.ToString();
        }
        else if (_card.damage <= 0)
        {
            //_dmgSlider.value = 0;
            //_dmgText.text = null;

        }


        //_rangeSlider.value = (_card.range / 5) + _sliderCheat;
       // _rangeText.text = _card.range.ToString();
       // _rangeFill.color = _card.rarityColor;

        if (_card.rateOfFire > 0)
        {
           // _rofSlider.value = (_card.rateOfFire / 10) + _sliderCheat;
           // _rofFill.color = _rarityColour;
           // _rofText.text = _card.rateOfFire.ToString();
        }
        else if (_card.rateOfFire <= 0)
        {
            //_rofSlider.value = 0;
           // _rofText.text = null;
        }


       // _durationSlider.value = (_card.duration / 10) + _sliderCheat;
       // _durationText.text = _card.duration.ToString();
       // _durationFill.color = _rarityColour;
    }
    private void DisableUI()
    {
        _border.enabled = false;
        _image.enabled = false;
        _icon.enabled = false;
        _name.enabled = false;

       // _dmgSlider.value = 0f;
        //_dmgText.enabled = false;
        //_rangeSlider.value = 0f;
        //_rangeText.enabled = false;
        //_rofSlider.value = 0f;
        //_rofText.enabled = false;
        //_durationSlider.value = 0f;
        //_durationText.enabled = false;
    }
    public void SelectCard()
    {
        if (!_cardStatsPanel.gameObject.activeInHierarchy)
        {
            if (!_selected)
            {
                _manager.AddCard(_card);
                _selected = true;
                _border.sprite = _glowBorder;
                _border.transform.localScale = new Vector2(1.26f, 1.26f);
                _icon.color = _gold;
            }
            else if (_selected)
            {
                _manager.RemoveCard(_card);
                _selected = false;
                _border.sprite = _baseBorder;
                _border.transform.localScale = Vector2.one;
                _icon.color = Color.white;
            }
        }

    }
    public void ActivateGlow()
    {
        _selected = true;
        _border.sprite = _glowBorder;
        _border.transform.localScale = new Vector2(1.26f, 1.26f);
        _icon.color = _gold;
    }
    public void GreyOut(bool on)
    {
        _greyOut.gameObject.SetActive(on);
        _button.interactable = !on;
    }
    public void StartLongPressTimer()
    {
        Debug.Log("Long Press Timer Started");
        _timerEnabled = true;
        _timer = _longPressTimer;
    }
    public void StopLongPressTimer()
    {
        _timerEnabled = false;
    }
    private void Update()
    {
        if (_timerEnabled)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                _timerEnabled = false;
                _cardStatsPanel.EnableAndFillStatsPanel(_card);
            }
        }
    }
}
