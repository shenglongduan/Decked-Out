using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;

public class DeckbuildingManager : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "SampleScene";

    [SerializeField] CardTierSelector[] _buttonScripts;
    [SerializeField] GameObject[] _cardRenderers;
    [SerializeField] Button _startButton;
    [SerializeReference] PillarIndicatorManager _pillarIndicatorManager;

    [SerializeField] List<TowerCardSO> _common;
    [SerializeField] List<TowerCardSO> _uncommon;
    [SerializeField] List<TowerCardSO> _rare;
    [SerializeField] List<TowerCardSO> _epic;
    [SerializeField] List<TowerCardSO> _legendary;
    [SerializeField] string _currentTier;

    public Sprite _glowBorder;
    bool allTiersHaveCard;
    public TowerCardSO[] commonCards { get { return _common.ToArray(); } }
    public TowerCardSO[] uncommonCards { get { return _uncommon.ToArray(); } }
    public TowerCardSO[] rareCards { get { return _rare.ToArray(); } }
    public TowerCardSO[] epicCards { get { return _epic.ToArray(); } }
    public TowerCardSO[] legendaryCards { get { return _legendary.ToArray(); } }

    public CardTierSelector[] tierButtons { get {  return _buttonScripts; } }

    private GameLoader _loader;
    private SaveSystem _saveSystem;

    private void Start()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);

    }
    private void Update()
    {
        if (allTiersHaveCard)
        {
            _startButton.gameObject.SetActive(true);
            _startButton.enabled = true;
        }
        else if (!allTiersHaveCard && _startButton.isActiveAndEnabled)
        {
            _startButton.gameObject.SetActive(false);
            _startButton.enabled = false;
        }
    }
    private void Initialize()
    {
        //DontDestroyOnLoad(gameObject);
        if(ServiceLocator.Contains<DeckbuildingManager>() == false)
        {
            ServiceLocator.Register<DeckbuildingManager>(this);
        }
        TutorialPassthrough tutorialPassthrough = FindObjectOfType<TutorialPassthrough>();
        if (_saveSystem == null)
        {
            _saveSystem = FindObjectOfType<SaveSystem>();
            CheckSavedCards();
        }
        if (tutorialPassthrough == null)
        {
            Button commonButton = _buttonScripts[0].gameObject.GetComponent<Button>();
            commonButton.Select();
            commonButton.onClick.Invoke();
        }
        else
        {
            Debug.Log("Loading Tutorial");
        }
        _startButton.enabled = false;
        _startButton.gameObject.SetActive(false);
    }

    private void CheckSavedCards()
    {
        bool[] savedCardsFromPlayerPref = _saveSystem.GetAllCardCollected();
        string[] cardNamesArray = _saveSystem.GetAllCardName();
        for (int i = 0; i < savedCardsFromPlayerPref.Length; i++)
        {
            string savedCardName = null;
            if (savedCardsFromPlayerPref[i] == true)
            {
                savedCardName = cardNamesArray[i];
                SlotInSavedCardToRespectiveButton(savedCardName);
            }
        }

    }
    private void SlotInSavedCardToRespectiveButton(string cardName)
    {
        Debug.Log("Slotting in " + cardName + " to button.");
        switch (cardName)
        {
            case "Archer_Tower":
                Debug.Log("Arrow Tower gotten from save system.");
                _buttonScripts[0].LoadInCardFromSaveSystem(Resources.Load<TowerCardSO>("TowerSOs/Towers/Archer Tower"));
                break;
            case "Frost_Tower":
                Debug.Log("Frost Tower gotten from save system.");
                _buttonScripts[1].LoadInCardFromSaveSystem(Resources.Load<TowerCardSO>("TowerSOs/Towers/Frost Tower"));
                break;
            case "Buff_Tower":
                Debug.Log("Buff Tower gotten from save system.");
                _buttonScripts[2].LoadInCardFromSaveSystem(Resources.Load<TowerCardSO>("TowerSOs/Towers/Buff Tower"));
                break;
            case "Flamethrower":
                Debug.Log("Flamethrower gotten from save system.");
                _buttonScripts[3].LoadInCardFromSaveSystem(Resources.Load<TowerCardSO>("TowerSOs/Towers/Fire Tower"));
                break;
            case "Electric_Tower":
                Debug.Log("Electric Tower gotten from save system.");
                _buttonScripts[4].LoadInCardFromSaveSystem(Resources.Load<TowerCardSO>("TowerSOs/Towers/Electric Tower"));
                break;
            case "Earthquake_Tower":
                Debug.Log("Earthquake Tower gotten from save system.");
                _buttonScripts[1].LoadInCardFromSaveSystem(Resources.Load<TowerCardSO>("TowerSOs/Towers/Earthquake Tower"));
                break;
            case "Attraction_Tower":
                Debug.Log("Attraction Tower gotten from save system.");
                _buttonScripts[0].LoadInCardFromSaveSystem(Resources.Load<TowerCardSO>("TowerSOs/Towers/Attraction Tower"));
                break;
            case "Cannon":
                Debug.Log("Cannon Tower gotten from save system.");
                _buttonScripts[0].LoadInCardFromSaveSystem(Resources.Load<TowerCardSO>("TowerSOs/Towers/Cannon Tower"));
                break;
            case "Wave_Tower":
                Debug.Log("Wave Tower gotten from save system.");
                _buttonScripts[1].LoadInCardFromSaveSystem(Resources.Load<TowerCardSO>("TowerSOs/Towers/Wave Tower"));
                break;
            case "Balista_Tower":
                Debug.Log("Balista Tower gotten from save system.");
                _buttonScripts[1].LoadInCardFromSaveSystem(Resources.Load<TowerCardSO>("TowerSOs/Towers/Ballista Tower"));
                break;
            case "Poison_Tower":
                Debug.Log("Poison Tower gotten from save system.");
                _buttonScripts[2].LoadInCardFromSaveSystem(Resources.Load<TowerCardSO>("TowerSOs/Towers/Poison Tower"));
                break;
            case "Mystery_Tower":
                Debug.Log("Mystery Tower gotten from save system.");
                _buttonScripts[2].LoadInCardFromSaveSystem(Resources.Load<TowerCardSO>("TowerSOs/Towers/Mystery"));
                break;
            case "Mortar_Tower":
                Debug.Log("Mortar Tower gotten from save system.");
                _buttonScripts[3].LoadInCardFromSaveSystem(Resources.Load<TowerCardSO>("TowerSOs/Towers/Mortar"));
                break;
            case "Sniper_Tower":
                Debug.Log("Sniper Tower gotten from save system.");
                _buttonScripts[0].LoadInCardFromSaveSystem(Resources.Load<TowerCardSO>("TowerSOs/Towers/Sniper Tower"));
                break;
            case "Organ_Tower":
                Debug.Log("Organ Gun gotten from save system.");
                _buttonScripts[2].LoadInCardFromSaveSystem(Resources.Load<TowerCardSO>("TowerSOs/Towers/Organ Gun"));
                break;
            case "Lighting_Bolt":
                Debug.Log("Lighting Bolt spell gotten from save system.");
                _buttonScripts[3].LoadInCardFromSaveSystem(Resources.Load<TowerCardSO>("TowerSOs/Spells/Lightning"));
                break;
            case "Big_Bomb":
                Debug.Log("Big Bomb spell gotten from save system.");
                _buttonScripts[2].LoadInCardFromSaveSystem(Resources.Load<TowerCardSO>("TowerSOs/Spells/Big Bomb"));
                break;
            case "Fireball":
                Debug.Log("Fireball spell gotten from save system.");
                _buttonScripts[1].LoadInCardFromSaveSystem(Resources.Load<TowerCardSO>("TowerSOs/Spells/Fireball"));
                break;
            case "Nuke":
                Debug.Log("Nuke spell gotten from save system.");
                _buttonScripts[4].LoadInCardFromSaveSystem(Resources.Load<TowerCardSO>("TowerSOs/Spells/Nuke"));
                break;
            case "Frost":
                Debug.Log("Frost spell gotten from save system.");
                _buttonScripts[0].LoadInCardFromSaveSystem(Resources.Load<TowerCardSO>("TowerSOs/Spells/Chill"));
                break;
            case "Freeze":
                Debug.Log("Freeze Time spell gotten from save system.");
                _buttonScripts[3].LoadInCardFromSaveSystem(Resources.Load<TowerCardSO>("TowerSOs/Spells/Freeze"));
                break;
            case "Black_Hole":
                Debug.Log("Black Hole spell gotten from save system.");
                _buttonScripts[3].LoadInCardFromSaveSystem(Resources.Load<TowerCardSO>("TowerSOs/Spells/Black Hole"));
                break;
            default:
                Debug.LogError("Unknown Card Name gotten from save system.");
                break;
        }
    }

    public SelectedCard[] SetTierRenderers(int cardsInTier, string tier)
    {
        _currentTier = tier;
        Debug.Log("Current Tier: " + tier);
        SelectedCard[] activeRenderers = new SelectedCard[cardsInTier];
        foreach (GameObject cardRenderer in _cardRenderers)
        {
            cardRenderer.SetActive(false);
        }
        for (int i = 0; i < cardsInTier; i++)
        {
            if (i <= cardsInTier)
            {
                _cardRenderers[i].SetActive(true);
                activeRenderers[i] = _cardRenderers[i].GetComponent<SelectedCard>();
                activeRenderers[i].Unslot();
            }
            else
            {
                Debug.LogError("No Cards in Tier - Check the Button");
            }

        }
        _pillarIndicatorManager.SetTier(_currentTier);
        return activeRenderers.ToArray();
    }

    public void OnStartButtonClicked()
    {
        TransitionScreenManager transitionScreenManager = FindObjectOfType<TransitionScreenManager>();
        transitionScreenManager.StartTranistion(nextSceneName);
        StartButtonLoading startButtonLoading = FindObjectOfType<StartButtonLoading>();
        startButtonLoading.DisableButton();
        //var loadSceneTask = SceneManager.LoadSceneAsync(nextSceneName);
    }
    public void LoadTutorialCards(TowerCardSO[] cards, string tier)
    {
        if (tier == "Common")
        {
            _common = cards.ToList();
        }
        if (tier == "Uncommon")
        {
            _uncommon = cards.ToList();
        }
        if (tier == "Rare")
        {
            _rare = cards.ToList();
        }
        if (tier == "Epic")
        {
            _epic = cards.ToList();
        }
        if (tier == "Legendary")
        {
            _legendary = cards.ToList();
        }        
    }
    public void AddCard(TowerCardSO card)
    {
        if(_currentTier == "Common")
        {
            _common.Add(card);
            _pillarIndicatorManager.CardAdded(_currentTier, _common.Count);
            _buttonScripts[0].DisableCardNeededIndicator();
            Debug.Log(card + " added to manager.");
            if (_common.Count >= 4)
            {
                ActivateGreyOut(_common);
            }
        }
        else if (_currentTier == "Uncommon")
        {
            _uncommon.Add(card);
            _pillarIndicatorManager.CardAdded(_currentTier, _uncommon.Count);
            _buttonScripts[1].DisableCardNeededIndicator();
            Debug.Log(card + " added to manager.");
            if (_uncommon.Count >= 4)
            {
                ActivateGreyOut(_uncommon);
            }
        }
        else if (_currentTier == "Rare")
        {
            _rare.Add(card);
            _pillarIndicatorManager.CardAdded(_currentTier, _rare.Count);
            _buttonScripts[2].DisableCardNeededIndicator();
            Debug.Log(card + " added to manager.");
            if (_rare.Count >= 3)
            {
                ActivateGreyOut(_rare);
            }
        }
        else if (_currentTier == "Epic")
        {
            _epic.Add(card);
            _pillarIndicatorManager.CardAdded(_currentTier, _epic.Count);
            _buttonScripts[3].DisableCardNeededIndicator();
            Debug.Log(card + " added to manager.");
            if (_epic.Count >= 3)
            {
                ActivateGreyOut(_epic);
            }
        }
        else if (_currentTier == "Legendary")
        {
            _legendary.Add(card);
            _pillarIndicatorManager.CardAdded(_currentTier, _legendary.Count);
            _buttonScripts[4].DisableCardNeededIndicator();
            Debug.Log(card + " added to manager.");
            if (_legendary.Count >= 2)
            {
                ActivateGreyOut(_legendary);
            }
        }
        allTiersHaveCard = new[] { _common, _uncommon, _rare, _epic, _legendary }.All(array => array.Count > 0);
    }
    public void RemoveCard(TowerCardSO card)
    {
        if (_currentTier == "Common")
        {
            _common.Remove(card);
            _pillarIndicatorManager.CardRemoved(_currentTier, _common.Count);
            Debug.Log(card + " removed from manager.");
            if (_common.Count !<= 4)
            {
                DeactivateGreyOut();
            }
            if (_common.Count == 0)
            {
                _buttonScripts[0].EnableCardNeededIndicator();
            }
        }
        else if (_currentTier == "Uncommon")
        {
            _uncommon.Remove(card);
            _pillarIndicatorManager.CardRemoved(_currentTier, _uncommon.Count);

            Debug.Log(card + " removed from manager.");
            if (_uncommon.Count !<= 4)
            {
                DeactivateGreyOut();
            }
            if (_uncommon.Count == 0)
            {
                _buttonScripts[1].EnableCardNeededIndicator();
            }
        }
        else if (_currentTier == "Rare")
        {
            _rare.Remove(card);
            _pillarIndicatorManager.CardRemoved(_currentTier, _rare.Count);

            Debug.Log(card + " removed from manager.");
            if (_rare.Count !<= 3)
            {
                DeactivateGreyOut();
            }
            if (_rare.Count == 0)
            {
                _buttonScripts[2].EnableCardNeededIndicator();
            }
        }
        else if (_currentTier == "Epic")
        {
            _epic.Remove(card);
            _pillarIndicatorManager.CardRemoved(_currentTier, _epic.Count);

            Debug.Log(card + " removed from manager.");
            if (_epic.Count !<= 3)
            {
                DeactivateGreyOut();
            }
            if (_epic.Count == 0)
            {
                _buttonScripts[3].EnableCardNeededIndicator();
            }
        }
        else if (_currentTier == "Legendary")
        {
            _legendary.Remove(card);
            _pillarIndicatorManager.CardRemoved(_currentTier, _legendary.Count);

            Debug.Log(card + " removed from manager.");
            if (_legendary.Count !<= 2)
            {
                DeactivateGreyOut();
            }
            if (_legendary.Count == 0)
            {
                _buttonScripts[4].EnableCardNeededIndicator();
            }
        }
        allTiersHaveCard = new[] { _common, _uncommon, _rare, _epic, _legendary }.All(array => array.Count > 0);
    }

    public bool CheckCurrentTier(string tier)
    {
        if (tier == _currentTier)
        {
            return false;
        }
        else
        {
            _currentTier = tier;
            return true;
        }
    }
  
    public void SetCardsOfTier(List<TowerCardSO> cards, string tier)
    {
        if (tier == "Common")
        {
            _common = cards;
        }
        if (tier == "Uncommon")
        {
            _uncommon = cards;
        }
        if (tier == "Rare")
        {
            _rare = cards;
        }
        if (tier == "Epic")
        {
            _epic = cards;
        }
        if (tier == "Legendary")
        {
            _legendary = cards;
        }
    }
    public TowerCardSO[] CheckIfSavedCards(string tier)
    {
        if (tier == "Common")
        {
            return commonCards;
        }
        if (tier == "Uncommon")
        {
            return uncommonCards;
        }
        if (tier == "Rare")
        {
            return rareCards;
        }
        if (tier == "Epic")
        {
            return epicCards;
        }
        if (tier == "Legendary")
        {
            return legendaryCards;
        }
        else
        {
            return null;
        }
    }
    public void ActivateGreyOut(List<TowerCardSO> cardList)
    {
        foreach (GameObject CardRenderer in _cardRenderers)
        {
            if (CardRenderer.activeInHierarchy)
            {
                SelectedCard script = CardRenderer.GetComponent<SelectedCard>();
                if (!script.selected && !cardList.Contains(script.card))
                {
                    script.GreyOut(true);
                }
            }

        }
    }
    public void DeactivateGreyOut()
    {
        foreach (GameObject CardRenderer in _cardRenderers)
        {
            if (CardRenderer.activeInHierarchy)
            {
                SelectedCard script = CardRenderer.GetComponent<SelectedCard>();
                script.GreyOut(false);
            }

        }
    }
}
