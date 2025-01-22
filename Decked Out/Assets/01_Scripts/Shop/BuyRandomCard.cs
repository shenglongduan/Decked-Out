
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyRandomCard : MonoBehaviour
{
    private SaveSystem saveSystem;
    [SerializeField] int prices;
    [SerializeField] ShopUIManager _uiManager;

    private void Start()
    {
        GameObject saveSystemObject = GameObject.FindGameObjectWithTag("SaveSystem");
        saveSystem = saveSystemObject.GetComponent<SaveSystem>();
    }

    public void BuyCard()
    {
        bool[] cardList = saveSystem.GetAllCardCollected();
        string[] cardNameList = saveSystem.GetAllCardName();
        int count = 0;
        int cardGet = 0;
        int gemCount = saveSystem.GetGemCount();

        if (gemCount >= prices)
        {
            foreach (bool card in cardList)
            {
                if (card == false)
                {
                    count++;
                }
            }
            if (count >= 1)
            {
                cardGet = Random.Range(0, count);

                for (int i = 0; i < cardList.Length; i++)
                {
                    if (cardList[i] == false && cardGet == 0)
                    {
                        SaveSystem.CardCollected collected = (SaveSystem.CardCollected)System.Enum.Parse(typeof(SaveSystem.CardCollected), cardNameList[i]);
                        saveSystem.SetCardCollected(collected, true);
                        saveSystem.MinusGem(prices);
                        _uiManager.RegisterNewCard(cardNameList[i]);
                        break;
                    }
                    else if (cardList[i] == false)
                    {
                        cardGet--;
                    }
                }
            }
        }
        _uiManager.UpdateUI();
    }
}
