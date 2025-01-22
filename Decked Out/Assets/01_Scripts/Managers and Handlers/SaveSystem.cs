using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public enum CardCollected
    {
        //Towers
        Archer_Tower, //Archer Tower
        Attraction_Tower, //Attraction Tower
        Cannon, //Cannon Tower
        Sniper_Tower, //Sniper Tower
        Frost_Tower, //Frost Tower
        Earthquake_Tower, //Earthquake Tower
        Wave_Tower, //Wave Tower
        Balista_Tower, //Ballista Tower
        Buff_Tower, //Buff Tower
        Poison_Tower, //Poison Tower
        Mystery_Tower, //Mystery Tower
        Organ_Tower, //Organ Gun Tower
        Flamethrower, //Fire Tower
        Mortar_Tower, //Mortar Tower
        Electric_Tower, // Eletric Tower

        //SPELLS
        Frost, //Chill Spell
        Fireball, //Fireball Spell
        Big_Bomb, //Big Bomb Spell
        Lighting_Bolt, //Lightning  Spell
        Freeze, //Freeze Spell
        Black_Hole, //Black Hole Spell
        Nuke //Nuke Spell
    }
    public enum PurchasedItem
    {
        b
    }

    private bool outPut = false;
    private bool[] allOutPutBool;
    private string[] allOutPutString;
    private int itemCount = 0;
    private int cardCount = 0;

    private Dictionary<CardCollected, string> cardCollectedString;
    private Dictionary<PurchasedItem, string> purchasedItemString;

    private void Awake()
    {
        GameObject[] saveSystem = GameObject.FindGameObjectsWithTag("SaveSystem");

        if (saveSystem.Length > 1)
        {
            Destroy(gameObject);
        }

        cardCollectedString = new Dictionary<CardCollected, string>();

        foreach (CardCollected card in System.Enum.GetValues(typeof(CardCollected)))
        {
            cardCollectedString[card] = card.ToString();
            cardCount++;
        }

        purchasedItemString = new Dictionary<PurchasedItem, string>();

        foreach (PurchasedItem item in System.Enum.GetValues(typeof(PurchasedItem)))
        {
            purchasedItemString[item] = item.ToString();
            itemCount++;
        }

        StartUpCard();

        DontDestroyOnLoad(gameObject);
    }

    //Set Card Collected
    public void SetCardCollected(CardCollected card, bool haveCard)
    {
        if (haveCard)
        {
            PlayerPrefs.SetInt(cardCollectedString[card], 1);
        }
        else
        {
            PlayerPrefs.SetInt(cardCollectedString[card], 0);
        }
    }
    //Get One Card Collected
    public bool GetCardCollected(CardCollected card)
    {
        outPut = false;
        if (PlayerPrefs.HasKey(cardCollectedString[card]))
        {
            if (PlayerPrefs.GetInt(cardCollectedString[card]) == 1)
            {
                outPut = true;
            }
            else
            {
                outPut = false;
            }
        }
        return outPut;
    }
    //Get All Card Collected (in the order how the enum are)
    public bool[] GetAllCardCollected()
    {
        allOutPutBool = new bool[cardCount];
        int count = 0;

        foreach (CardCollected card in System.Enum.GetValues(typeof(CardCollected)))
        {
            allOutPutBool[count] = false;
            count++;
        }
        count = 0;

        foreach (CardCollected card in System.Enum.GetValues(typeof(CardCollected)))
        {
            if (PlayerPrefs.HasKey(cardCollectedString[card]))
            {
                if (PlayerPrefs.GetInt(cardCollectedString[card]) == 1)
                {
                    allOutPutBool[count] = true;
                }
                else
                {
                    allOutPutBool[count] = false;
                }
            }
            count++;
        }

        return allOutPutBool;
    }
    //Reset Card Collected
    public void ResetCardCollected()
    {
        foreach (CardCollected card in System.Enum.GetValues(typeof(CardCollected)))
        {
            PlayerPrefs.SetInt(cardCollectedString[card], 0);
        }
        PlayerPrefs.SetInt(cardCollectedString[CardCollected.Archer_Tower], 1);
        PlayerPrefs.SetInt(cardCollectedString[CardCollected.Cannon], 1);
        PlayerPrefs.SetInt(cardCollectedString[CardCollected.Frost], 1);
        PlayerPrefs.SetInt(cardCollectedString[CardCollected.Frost_Tower], 1);
        PlayerPrefs.SetInt(cardCollectedString[CardCollected.Fireball], 1);
        PlayerPrefs.SetInt(cardCollectedString[CardCollected.Poison_Tower], 1);
        PlayerPrefs.SetInt(cardCollectedString[CardCollected.Nuke], 1);
        PlayerPrefs.SetInt(cardCollectedString[CardCollected.Flamethrower], 1);
    }
    //Chack For Startting Card
    public void StartUpCard()
    {
        if (PlayerPrefs.HasKey(cardCollectedString[CardCollected.Archer_Tower]) == false ||
            PlayerPrefs.HasKey(cardCollectedString[CardCollected.Cannon]) == false ||
            PlayerPrefs.HasKey(cardCollectedString[CardCollected.Frost]) == false ||
            PlayerPrefs.HasKey(cardCollectedString[CardCollected.Frost_Tower]) == false ||
            PlayerPrefs.HasKey(cardCollectedString[CardCollected.Fireball]) == false ||
            PlayerPrefs.HasKey(cardCollectedString[CardCollected.Poison_Tower]) == false ||
            PlayerPrefs.HasKey(cardCollectedString[CardCollected.Nuke]) == false ||
            PlayerPrefs.HasKey(cardCollectedString[CardCollected.Flamethrower]) == false
            )
        {
            PlayerPrefs.SetInt(cardCollectedString[CardCollected.Archer_Tower], 1);
            PlayerPrefs.SetInt(cardCollectedString[CardCollected.Cannon], 1);
            PlayerPrefs.SetInt(cardCollectedString[CardCollected.Frost], 1);
            PlayerPrefs.SetInt(cardCollectedString[CardCollected.Frost_Tower], 1);
            PlayerPrefs.SetInt(cardCollectedString[CardCollected.Fireball], 1);
            PlayerPrefs.SetInt(cardCollectedString[CardCollected.Poison_Tower], 1);
            PlayerPrefs.SetInt(cardCollectedString[CardCollected.Nuke], 1);
            PlayerPrefs.SetInt(cardCollectedString[CardCollected.Flamethrower], 1);
        }
    }
    //Get All Card Name
    public string[] GetAllCardName()
    {
        allOutPutString = new string[cardCount];
        int count = 0;

        foreach (CardCollected card in System.Enum.GetValues(typeof(CardCollected)))
        {
            allOutPutString[count] = card.ToString();
            count++;
        }

        return allOutPutString;
    }

    //Set Purchased Item
    public void SetPurchasedItem(PurchasedItem item, bool haveItem)
    {
        if (haveItem)
        {
            PlayerPrefs.SetInt(purchasedItemString[item], 1);
        }
        else
        {
            PlayerPrefs.SetInt(purchasedItemString[item], 0);
        }
    }
    //Get one Purchased Item
    public bool GetPurchasedItem(PurchasedItem item)
    {
        outPut = false;
        if (PlayerPrefs.HasKey(purchasedItemString[item]))
        {
            if (PlayerPrefs.GetInt(purchasedItemString[item]) == 1)
            {
                outPut = true;
            }
            else
            {
                outPut = false;
            }
        }
        return outPut;
    }
    //Get all Purchased Item (in the order how the enum are)
    public bool[] GetAllPurchasedItem()
    {
        allOutPutBool = new bool[itemCount];
        int count = 0;

        foreach (PurchasedItem item in System.Enum.GetValues(typeof(PurchasedItem)))
        {
            allOutPutBool[count] = false;
            count++;
        }
        count = 0;

        foreach (PurchasedItem item in System.Enum.GetValues(typeof(PurchasedItem)))
        {
            if (PlayerPrefs.HasKey(purchasedItemString[item]))
            {
                if (PlayerPrefs.GetInt(purchasedItemString[item]) == 1)
                {
                    allOutPutBool[count] = true;
                }
                else
                {
                    allOutPutBool[count] = false;
                }
            }
            count++;
        }

        return allOutPutBool;
    }
    //Reset Purchased Item
    public void ResetPurchasedItem()
    {
        foreach (PurchasedItem item in System.Enum.GetValues(typeof(PurchasedItem)))
        {
            PlayerPrefs.SetInt(purchasedItemString[item], 0);
        }
    }

    //Add Gem
    public void AddGem(int count)
    {
        int i = count;
        if (PlayerPrefs.HasKey("gemCount"))
        {
            i += PlayerPrefs.GetInt("gemCount");
        }
        PlayerPrefs.SetInt("gemCount", i);
        Debug.Log(i);
    }
    //minus Gem
    public void MinusGem(int count)
    {
        if (PlayerPrefs.HasKey("gemCount"))
        {
            int currentGems = PlayerPrefs.GetInt("gemCount");
            int subtractedGems = currentGems - count;
            PlayerPrefs.SetInt("gemCount", subtractedGems);
            Debug.Log(subtractedGems);
        }
    }
    //Save Gem Count
    public void SetGemCount(int count)
    {
        PlayerPrefs.SetInt("gemCount", count);
    }
    //Get Gem Count
    public int GetGemCount()
    {
        if (PlayerPrefs.HasKey("gemCount"))
        {
            return PlayerPrefs.GetInt("gemCount");
        }
        else
        {
            return 0;
        }
    }
    //Reset Gem Count
    public void ResetGemCount()
    {
        PlayerPrefs.SetInt("gemCount", 0);
    }

    //Add total kill
    public void AddTotalKill(int count)
    {
        int i = count;
        if (PlayerPrefs.HasKey("TotalKill"))
        {
            i += PlayerPrefs.GetInt("TotalKill");
        }
        PlayerPrefs.SetInt("TotalKill", i);
    }
    //Save Total Kill
    public void SetTotalKill(int count)
    {
        PlayerPrefs.SetInt("TotalKill", count);
    }
    //Get Total Kill
    public int GetTotalKill()
    {
        if (PlayerPrefs.HasKey("TotalKill"))
        {
            return PlayerPrefs.GetInt("TotalKill");
        }
        else
        {
            return 0;
        }
    }
    //Reset Total Kill
    public void ResetTotalKill()
    {
        PlayerPrefs.SetInt("TotalKill", 0);
    }
}
