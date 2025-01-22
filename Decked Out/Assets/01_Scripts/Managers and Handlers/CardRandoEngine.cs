using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections;

public class CardRandoEngine : MonoBehaviour
{
    [Header("Game Tool")]
    public WaveManager waveManager;
    public TowerSelection towerSelection;
    public Vector3 leftSpotScale;
    public Vector3 bottomSpotScale;
    public bool cardsOnLeft;
    public GameObject cardHandPanel;
    public Transform bottomSpot;
    public Transform upperSpot;
    public float moveDuration;

    [Header("Card Spaces")]
    public GameObject cardSpace0;
    public GameObject cardSpace1;
    public GameObject cardSpace2;
    public GameObject cardSpace3;
    public GameObject cardSpace4;
    public GameObject blockingButton;

    [Header("Card Stats Panel")]
    public GameObject cardStatsPanel;
    public Image cardStatsBackground;
    public Image cardStatsImage;
    public Image cardStatsIcon;
    public TextMeshProUGUI cardStatsTitleText;
    public TextMeshProUGUI cardStatsInfoText;

    public Slider dmgSlider;
    public TextMeshProUGUI dmgText;
    public Slider rangeSlider;
    public TextMeshProUGUI rangeText;
    public Slider rofSlider;
    public TextMeshProUGUI rofText;
    public Slider durationSlider;
    public TextMeshProUGUI durationText;
    public float sliderCheat;

    [Header("Hand Cards Data")]
    public int handSize;
    public int spellUses = 4;


    [Header("Button Input Modifers")]
    public List<GameObject> buttons = new List<GameObject>();
    public GameObject _lastCardSlot;
    public float longPressDuration = 1.0f;

    [Header("Card 0 Data")]
    public TowerCardSO card0Data;
    public Image cardSpace0Background;
    public string card0Name;
    public TextMeshProUGUI card0SpellUsesText;
    public Image card0TowerImage;
    public Image card0IconImage;
    public int card0TowerID;
    public bool card0Used;
    public int spell0Uses;


    [Header("Card 1 Data")]
    public TowerCardSO card1Data;
    public Image cardSpace1Background;
    public string card1Name;
    public TextMeshProUGUI card1SpellUsesText;
    public Image card1TowerImage;
    public Image card1IconImage;
    public int card1TowerID;
    public bool card1Used;
    public int spell1Uses;

    [Header("Card 2 Data")]
    public TowerCardSO card2Data;
    public Image cardSpace2Background;
    public string card2Name;
    public TextMeshProUGUI card2SpellUsesText;
    public Image card2TowerImage;
    public Image card2IconImage;
    public int card2TowerID;
    public bool card2Used;
    public int spell2Uses;

    [Header("Card 3 Data")]
    public TowerCardSO card3Data;
    public Image cardSpace3Background;
    public string card3Name;
    public TextMeshProUGUI card3SpellUsesText;
    public Image card3TowerImage;
    public Image card3IconImage;
    public int card3TowerID;
    public bool card3Used;
    public int spell3Uses;

    [Header("Card 4 Data")]
    public TowerCardSO card4Data;
    public Image cardSpace4Background;
    public string card4Name;
    public TextMeshProUGUI card4SpellUsesText;
    public Image card4TowerImage;
    public Image card4IconImage;
    public int card4TowerID;
    public bool card4Used;
    public int spell4Uses;



    [Header("Card Rando System")]
    public float totalWeight;
    public List<TowerCardSO> towerCards = new List<TowerCardSO>();
    public List<TowerCardSO> cardsInHand = new List<TowerCardSO>();

    [Header("Card Audio")]
    public AudioSource cardShuffle;

    private GameLoader _loader;
    private DeckbuildingManager _deckManager;

    private bool isSelectingTower;
    private float timer;
    private bool timerOn = false;
    private bool isButtonHeld = false;
    private float buttonHeldTime = 0f;

    float scale;
    private int current_Button_Held;
    private float _startUpDelay = 1f;
    private bool _startup = true;

    private void Start()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }
    private void Initialize()
    {
        _deckManager = ServiceLocator.Get<DeckbuildingManager>();

        transform.position = bottomSpot.position;
        transform.rotation = bottomSpot.rotation;
        spell0Uses = 0;
        spell1Uses = 0;
        spell2Uses = 0;
        spell3Uses = 0;
        spell4Uses = 0;

        blockingButton.gameObject.SetActive(false);
        buttons.Add(cardSpace0);
        buttons.Add(cardSpace1);
        buttons.Add(cardSpace2);
        buttons.Add(cardSpace3);
        buttons.Add(cardSpace4);
        _startup = true;
    }

    private void Update()
    {
        if (_startup)
        {
            _startUpDelay -= Time.deltaTime;
            if (_startUpDelay >= 0)
            {
                NewWave();
                _startup = false;
                _startUpDelay = 0;
            }
        }
        isSelectingTower = towerSelection.isSelectingTower;
        if (!isSelectingTower)
        {
            blockingButton.gameObject.SetActive(false);
        }
        if (isButtonHeld)
        {
            buttonHeldTime += Time.deltaTime;
            //Debug.Log("Button Held for:" + buttonHeldTime);
            if (buttonHeldTime >= longPressDuration)
            {
                switch (current_Button_Held)
                {
                    case 0:
                        ButtonStats(card0Data);
                        break;
                    case 1:
                        ButtonStats(card1Data);
                        break;
                    case 2:
                        ButtonStats(card2Data);
                        break;
                    case 3:
                        ButtonStats(card3Data);
                        break;
                    case 4:
                        ButtonStats(card4Data);
                        break;
                }

            }
        }

    }

    public void NewWave()
    {
        spell0Uses = 0;
        spell1Uses = 0;
        spell2Uses = 0;
        spell3Uses = 0;
        spell4Uses = 0;

        Debug.Log("New Wave Called");
        cardsInHand.Clear();
        blockingButton.gameObject.SetActive(true);
        MoveCardHandPanel(false);
        PlayCardSuffleSound();

        cardSpace0.gameObject.SetActive(true);
        cardSpace1.gameObject.SetActive(true);
        cardSpace2.gameObject.SetActive(true);
        cardSpace3.gameObject.SetActive(true);
        cardSpace4.gameObject.SetActive(true);
        GetCards();

        card0Used = false;
        card1Used = false;
        card2Used = false;
        card3Used = false;
        card4Used = false;

    }
    public void GetCards()
    {
        GetRandomizedCards(handSize);
        GetCardData();
        ButtonData();
        LoadSpellText();
    }

    public void GetCardData()
    {
        card0Data = cardsInHand[0];
        card1Data = cardsInHand[1];
        card2Data = cardsInHand[2];
        card3Data = cardsInHand[3];
        card4Data = cardsInHand[4];
        //cardsInHand.Clear();

    }
    public void ButtonData()
    {
        cardSpace0Background.sprite = card0Data.background;
        card0TowerImage.sprite = card0Data.image;
        card0IconImage.sprite = card0Data.icon;
        card0Name = card0Data.name;
        if (card0Data.uses > 0)
        {
            spell0Uses = card0Data.uses;
            ChangeSpellText();
        }


        cardSpace1Background.sprite = card1Data.background;
        card1TowerImage.sprite = card1Data.image;
        card1IconImage.sprite = card1Data.icon;
        card1Name = card1Data.name;
        if (card1Data.uses > 0)
        {
            spell1Uses = card1Data.uses;
            ChangeSpellText();
        }

        cardSpace2Background.sprite = card2Data.background;
        card2TowerImage.sprite = card2Data.image;
        card2IconImage.sprite = card2Data.icon;
        card2Name = card2Data.name;
        if (card2Data.uses > 0)
        {
            spell2Uses = card2Data.uses;
            ChangeSpellText();
        }

        cardSpace3Background.sprite = card3Data.background;
        card3TowerImage.sprite = card3Data.image;
        card3IconImage.sprite = card3Data.icon;
        card3Name = card3Data.name;
        if (card3Data.uses > 0)
        {
            spell3Uses = card3Data.uses;
            ChangeSpellText();
        }

        cardSpace4Background.sprite = card4Data.background;
        card4TowerImage.sprite = card4Data.image;
        card4IconImage.sprite = card4Data.icon;
        card4Name = card4Data.name;
        if (card4Data.uses > 0)
        {
            spell4Uses = card4Data.uses;
            ChangeSpellText();
        }


    }


    public void Button0DragOff()
    {
        if (isButtonHeld && buttonHeldTime < longPressDuration && current_Button_Held == 0)
        {
            PlaceButton0();
            MoveCardHandPanel(true);
            isButtonHeld = false;
            buttonHeldTime = 0;
        }
    }
    public void Button1ragOff()
    {
        if (isButtonHeld && buttonHeldTime < longPressDuration && current_Button_Held == 1)
        {
            PlaceButton1();
            MoveCardHandPanel(true);
            isButtonHeld = false;
            buttonHeldTime = 0;
        }
    }
    public void Button2DragOff()
    {
        if (isButtonHeld && buttonHeldTime < longPressDuration && current_Button_Held == 2)
        {
            PlaceButton2();
            MoveCardHandPanel(true);
            isButtonHeld = false;
            buttonHeldTime = 0;
        }
    }
    public void Button3DragOff()
    {
        if (isButtonHeld && buttonHeldTime < longPressDuration && current_Button_Held == 3)
        {
            PlaceButton3();
            MoveCardHandPanel(true);
            isButtonHeld = false;
            buttonHeldTime = 0;
        }
    }
    public void Button4DragOff()
    {
        if (isButtonHeld && buttonHeldTime < longPressDuration && current_Button_Held == 4)
        {
            PlaceButton4();
            MoveCardHandPanel(true);
            isButtonHeld = false;
            buttonHeldTime = 0;
        }
    }

    public void MoveCardHandPanel(bool lowering)
    {
        StopAllCoroutines(); // Stop ongoing movements to prevent overlaps
        StartCoroutine(MovePanelToPosition(lowering ? bottomSpot.localPosition : upperSpot.localPosition));
    }

    private IEnumerator MovePanelToPosition(Vector3 targetPosition)
    {
        float elapsedTime = 0;
        float moveDuration = 0.5f; // Duration of the movement in seconds, adjust as needed
        Vector3 startPosition = cardHandPanel.transform.localPosition;

        while (elapsedTime < moveDuration)
        {
            cardHandPanel.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cardHandPanel.transform.localPosition = targetPosition; // Ensure it's exactly at the target at the end
    }

    public void Button0()
    {
        //Debug.Log("Button0 Clicked");
        isButtonHeld = true;
        buttonHeldTime = 0f;
        current_Button_Held = 0;
    }

    public void Button1()
    {
        isButtonHeld = true;
        buttonHeldTime = 0f;
        current_Button_Held = 1;

    }
    public void Button2()
    {
        isButtonHeld = true;
        buttonHeldTime = 0f;
        current_Button_Held = 2;

    }
    public void Button3()
    {
        isButtonHeld = true;
        buttonHeldTime = 0f;
        current_Button_Held = 3;

    }
    public void Button4()
    {

        isButtonHeld = true;
        buttonHeldTime = 0f;
        current_Button_Held = 4;
    }



    public void ButtonStats(TowerCardSO cardDataToShow)
    {
        isButtonHeld = false;
        buttonHeldTime = 0;

        cardStatsPanel.gameObject.SetActive(true);

        Image dmgFillImage = dmgSlider.fillRect.GetComponent<Image>();
        Image rangeFillImage = rangeSlider.fillRect.GetComponent<Image>();
        Image rofFillImage = rofSlider.fillRect.GetComponent<Image>();
        Image durationFillImage = durationSlider.fillRect.GetComponent<Image>();


        cardStatsBackground.sprite = cardDataToShow.background;
        cardStatsImage.sprite = cardDataToShow.image;
        cardStatsIcon.sprite = cardDataToShow.icon;
        cardStatsTitleText.text = cardDataToShow.towerName;
        cardStatsTitleText.color = cardDataToShow.rarityColor;
        cardStatsInfoText.text = cardDataToShow.towerInfo;
        cardStatsInfoText.color = cardDataToShow.rarityColor;

        if (cardDataToShow.damage > 0)
        {
            dmgSlider.value = (cardDataToShow.damage / 25) + sliderCheat;
            dmgFillImage.color = cardDataToShow.rarityColor;
        }
        else if (cardDataToShow.damage <= 0)
        {
            dmgSlider.value = 0;
        }
        dmgText.text = cardDataToShow.damage.ToString();

        //dmgText.color = cardDataToShow.rarityColor;

        if (cardDataToShow.range > 0)
        {
            rangeSlider.value = (cardDataToShow.range / 5) + sliderCheat;
            rangeFillImage.color = cardDataToShow.rarityColor;
        }
        else if (cardDataToShow.range <= 0)
        {
            rangeSlider.value = 0;
        }
        rangeText.text = cardDataToShow.range.ToString();
        //rangeText.color = cardDataToShow.rarityColor;

        if (cardDataToShow.rateOfFire > 0)
        {
            rofSlider.value = (cardDataToShow.rateOfFire / 10) + sliderCheat;
            rofFillImage.color = cardDataToShow.rarityColor;
        }
        else if (cardDataToShow.rateOfFire <= 0)
        {
            rofSlider.value = 0f;
        }
        rofText.text = cardDataToShow.rateOfFire.ToString();
        //rofText.color = cardDataToShow.rarityColor;

        if (cardDataToShow.duration > 0)
        {
            durationSlider.value = (cardDataToShow.duration / 10) + sliderCheat;
            durationFillImage.color = cardDataToShow.rarityColor;
        }
        else if (cardDataToShow.duration <= 0)
        {
            durationSlider.value = 0;
        }
        durationText.text = cardDataToShow.duration.ToString();
        //durationText.color = cardDataToShow.rarityColor;

        //Debug.Log("Card Stats Panel Open");
    }
    public void PlaceButton0()
    {
        _lastCardSlot = cardSpace0;
        if (card0Data.spell == true)
        {
            //  towerSelection.SelectTower();
            towerSelection.SelectSpells();
            towerSelection.spells = card0Name;
            blockingButton.gameObject.SetActive(true);
            spell0Uses--;
            ChangeSpellText(cardSpace0, card0Used, spell0Uses, 0);
            //ChangeSpellText();
        }

        else
        {
            towerSelection.SelectTower();
            towerSelection.towers = card0Name;
            Debug.Log("Card Used & Not Spell");
            cardSpace0.gameObject.SetActive(false);
            blockingButton.gameObject.SetActive(true);
            card0Used = true;
        }


    }

    public void PlaceButton1()
    {
        _lastCardSlot = cardSpace1;
        if (card1Data.spell == true)
        {
            //  towerSelection.SelectTower();
            towerSelection.SelectSpells();
            towerSelection.spells = card1Name;
            blockingButton.gameObject.SetActive(true);
            spell1Uses--;
            ChangeSpellText(cardSpace1, card1Used, spell1Uses, 1);
            //ChangeSpellText();
        }
        else
        {
            towerSelection.SelectTower();
            towerSelection.towers = card1Name;
            Debug.Log("Card Used & Not Spell");
            cardSpace1.gameObject.SetActive(false);
            blockingButton.gameObject.SetActive(true);
            card1Used = true;
        }

    }
    public void PlaceButton2()
    {
        _lastCardSlot = cardSpace2;
        if (card2Data.spell == true)
        {
            //  towerSelection.SelectTower();
            towerSelection.SelectSpells();
            towerSelection.spells = card2Name;
            blockingButton.gameObject.SetActive(true);
            spell2Uses--;
            ChangeSpellText(cardSpace2, card2Used, spell2Uses, 2);
            //ChangeSpellText();
        }
        else
        {
            towerSelection.SelectTower();
            towerSelection.towers = card2Name;
            Debug.Log("Card Used & Not Spell");
            cardSpace2.gameObject.SetActive(false);
            blockingButton.gameObject.SetActive(true);
            card2Used = true;
        }

    }
    public void PlaceButton3()
    {
        _lastCardSlot = cardSpace3;
        if (card3Data.spell == true)
        {
            // towerSelection.SelectTower();
            towerSelection.SelectSpells();
            towerSelection.spells = card3Name;
            blockingButton.gameObject.SetActive(true);
            spell3Uses--;
            ChangeSpellText(cardSpace3, card3Used, spell3Uses, 3);
            //ChangeSpellText();
        }
        else
        {
            towerSelection.SelectTower();
            towerSelection.towers = card3Name;
            Debug.Log("Card Used & Not Spell");
            cardSpace3.gameObject.SetActive(false);
            blockingButton.gameObject.SetActive(true);
            card3Used = true;

        }

    }
    public void PlaceButton4()
    {
        _lastCardSlot = cardSpace4;
        if (card4Data.spell == true)
        {
            //towerSelection.SelectTower();
            towerSelection.SelectSpells();
            towerSelection.spells = card4Name;
            blockingButton.gameObject.SetActive(true);
            spell4Uses--;
            ChangeSpellText(cardSpace4, card4Used, spell4Uses, 4);
            //ChangeSpellText();
        }
        else
        {
            towerSelection.SelectTower();
            towerSelection.towers = card4Name;
            Debug.Log("Card Used & Not Spell");
            cardSpace4.gameObject.SetActive(false);
            blockingButton.gameObject.SetActive(true);
            card4Used = true;
        }


    }


    public void ExitCardStatsPanel()
    {
        cardStatsPanel.gameObject.SetActive(false);
    }


    public List<TowerCardSO> GetRandomizedCards(int count)
    {
        List<TowerCardSO> cardsToShuffle = new List<TowerCardSO>(towerCards);
        totalWeight = 0;
        foreach (TowerCardSO card in cardsToShuffle)
        {
            totalWeight += card.rarityWeight;
        }

        //Debug.Log("Cards in Deck: " + cardsToShuffle.Count);
        //Debug.Log("Total Weight: " + totalWeight);

        for (int i = 0; i < count; i++)
        {
            TowerCardSO randomCard = SelectRandomWeightedCard(cardsToShuffle);
            cardsInHand.Add(randomCard);
        }

        //Debug.Log(cardsInHand.Count);
        return cardsInHand;
    }
    private TowerCardSO SelectRandomWeightedCard(List<TowerCardSO> _cardsToShuffle)
    {
        // Calculate totalWeight for normalization if it doesn't already sum to 100
        float totalWeight = _cardsToShuffle.Sum(card => card.rarityWeight);

        // Generate a random value between 0 and 100 (assuming totalWeight is normalized to 100)
        float randomValue = Random.Range(0f, 100f); // Use 100f to represent 100%

        foreach (TowerCardSO card in _cardsToShuffle)
        {
            // Normalize card weight to a percentage if totalWeight is not 100
            float normalizedWeight = (card.rarityWeight / totalWeight) * 100; // Only needed if totalWeight != 100

            if (randomValue <= normalizedWeight)
            {
                return card;
            }
            else
            {
                randomValue -= normalizedWeight;
            }
        }

        return null; // Fallback in case no card is selected, should not happen if weights are normalized correctly
    }
    public void PlayCardSuffleSound()
    {
        if (!cardShuffle.isPlaying)
        {
            cardShuffle.Play();
        }
    }

    private void ChangeSpellText()
    {
        // Mapping of spell uses to Roman numerals
        Dictionary<int, string> spellUsesToRoman = new Dictionary<int, string>()
    {
        {0, ""}, // Handle the case of 0 separately to deactivate the card space
        {1, "I"},
        {2, "II"},
        {3, "III"},
        {4, "IV"}
    };

        // Assuming you have an array or similar structure for cardSpaces, cardUsed flags, and spellUsesTexts
        GameObject[] cardSpaces = { cardSpace0, cardSpace1, cardSpace2, cardSpace3, cardSpace4 };
        bool[] cardUsed = { card0Used, card1Used, card2Used, card3Used, card4Used };
        TowerCardSO[] cardData = { card0Data, card1Data, card2Data, card3Data, card4Data };
        TextMeshProUGUI[] spellUsesTexts = { card0SpellUsesText, card1SpellUsesText, card2SpellUsesText, card3SpellUsesText, card4SpellUsesText };
        int[] spellUses = { spell0Uses, spell1Uses, spell2Uses, spell3Uses, spell4Uses };

        for (int i = 0; i < spellUses.Length; i++)
        {
            if (spellUses[i] > 0)
            {
                //spellUsesTexts[i].gameObject.SetActive(true);
                cardSpaces[i].SetActive(true);
                spellUsesTexts[i].text = spellUsesToRoman[spellUses[i]];
            }
            else if (spellUses[i] <= 0 && cardData[i].spell == true)
            {
                cardSpaces[i].SetActive(false);
                cardUsed[i] = true;
            }

        }
    }

    private void ChangeSpellText(GameObject cardSpaces, bool cardUsed, int spellUses, int inDex)
    {
        // Mapping of spell uses to Roman numerals
        Dictionary<int, string> spellUsesToRoman = new Dictionary<int, string>()
        {
            {0, ""}, // Handle the case of 0 separately to deactivate the card space
            {1, "I"},
            {2, "II"},
            {3, "III"},
            {4, "IV"}
         };

        TowerCardSO[] cardData = { card0Data, card1Data, card2Data, card3Data, card4Data };
        TextMeshProUGUI[] spellUsesTexts = { card0SpellUsesText, card1SpellUsesText, card2SpellUsesText, card3SpellUsesText, card4SpellUsesText };

        for (int i = 0; i < 5; i++)
        {
            if (spellUses > 0)
            {
                //spellUsesTexts[i].gameObject.SetActive(true);
                cardSpaces.SetActive(true);
                spellUsesTexts[inDex].text = spellUsesToRoman[spellUses];
            }
            else if (spellUses <= 0 && cardData[i].spell == true)
            {
                cardSpaces.SetActive(false);
                cardUsed = true;
            }
        }
    }
    
    public void SpellSlotCheck()
    {
        GameObject[] cardSpaces = { cardSpace0, cardSpace1, cardSpace2, cardSpace3, cardSpace4 };
        // Directly work with the spell uses variables inside the loop
        Debug.Log("Spell Slot Checked");
        for (int i = 0; i < cardSpaces.Length; i++)
        {
            if (_lastCardSlot == cardSpaces[i])
            {
                // Increment the corresponding spellUses variable directly
                switch (i)
                {
                    case 0:
                        spell0Uses++;
                        Debug.Log(spell0Uses);
                        break;
                    case 1:
                        spell1Uses++;
                        Debug.Log(spell1Uses);
                        break;
                    case 2:
                        spell2Uses++;
                        Debug.Log(spell2Uses);
                        break;
                    case 3:
                        spell3Uses++;
                        Debug.Log(spell3Uses);
                        break;
                    case 4:
                        spell4Uses++;
                        Debug.Log(spell4Uses);
                        break;
                }                
                ChangeSpellText(); // Reflect the changes
                return; // Exit the method after updating
            }
        }
    }

    public void LoadSpellText()
    {
        TowerCardSO[] cardDatas = { card0Data, card1Data, card2Data, card3Data, card4Data };
        TextMeshProUGUI[] spellUsesTexts = { card0SpellUsesText, card1SpellUsesText, card2SpellUsesText, card3SpellUsesText, card4SpellUsesText };
        GameObject[] cardSpaces = { cardSpace0, cardSpace1, cardSpace2, cardSpace3, cardSpace4 };
        for (int i = 0; i < cardDatas.Length; i++)
        {
            cardSpaces[i].gameObject.SetActive(true);
            if (cardDatas[i].spell == false)
            {
                spellUsesTexts[i].gameObject.SetActive(false);
            }
            else
            {
                spellUsesTexts[i].gameObject.SetActive(true);
                ChangeSpellText();
            }
        }
       
    }


}
