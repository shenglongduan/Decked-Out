using System.Collections;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Purchasing;
using System.Collections.Generic;
using UnityEngine.Purchasing.Extension;

[Serializable]
public class ConsumableItem
{
    public string Name;
    public string Id;
    public string desc;
    public float price;
}

/* [Serializable]
public class NonConsumableItem
{
    public string Name;
    public string Id;
    public string desc;
    public float price;
} */


public class ShopScript : MonoBehaviour, IStoreListener
{
    IStoreController m_StoreContoller;

    public ConsumableItem cItem1;
    public ConsumableItem cItem2;
    public ConsumableItem cItem3;
    // public NonConsumableItem ncItem;

    public TMP_InputField inp;

    public Data data;
    public Payload payload;
    public PayloadData payloadData;

    SaveSystem saveSystem;
    ShopUIManager shopUIManager;

    private void Start()
    {
        SetupBuilder();
        saveSystem = FindObjectOfType<SaveSystem>();
        shopUIManager = FindObjectOfType<ShopUIManager>();
    }

    #region setup and initialize
    void SetupBuilder()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(cItem1.Id, ProductType.Consumable);
        builder.AddProduct(cItem2.Id, ProductType.Consumable);
        builder.AddProduct(cItem3.Id, ProductType.Consumable);
        // builder.AddProduct(ncItem.Id, ProductType.NonConsumable);

        UnityPurchasing.Initialize(this, builder);
    }
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        print("Success");
        m_StoreContoller = controller;
        // CheckNonConsumable(ncItem.Id);
    }
    #endregion

    #region button clicks 
    public void Consumable_Btn1_Pressed()
    {
        m_StoreContoller.InitiatePurchase(cItem1.Id);
    }

    public void Consumable_Btn2_Pressed()
    {
        m_StoreContoller.InitiatePurchase(cItem2.Id);
    }

    public void Consumable_Btn3_Pressed()
    {
        m_StoreContoller.InitiatePurchase(cItem3.Id);
    }

    /* public void NonConsumable_Btn_Pressed()
    {
        m_StoreContoller.InitiatePurchase(ncItem.Id);
    } */
    #endregion

    #region main
    //processing purchase
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        //Retrieve the purchased product
        var product = purchaseEvent.purchasedProduct;

        print("Purchase Complete" + product.definition.id);

        if (product.definition.id == cItem1.Id)
        {
            AddGems(100);
        }
        else if (product.definition.id == cItem2.Id)
        {
            AddGems(350);
        }
        else if (product.definition.id == cItem3.Id)
        {
            AddGems(1000);
        }
        /* else if (product.definition.id == ncItem.Id)
        {
            RemoveAds();
        } */

        return PurchaseProcessingResult.Complete;
    }
    #endregion

    /* void CheckNonConsumable(string id)
    {
        if (m_StoreContoller != null)
        {
            var product = m_StoreContoller.products.WithID(id);
            if (product != null)
            {
                if (product.hasReceipt)//purchased
                {
                    RemoveAds();
                }
                else
                {
                    ShowAds();
                }
            }
        }
    } */

    void CheckSubscription(string id)
    {
        var subProduct = m_StoreContoller.products.WithID(id);
        if (subProduct != null)
        {
            try
            {
                if (subProduct.hasReceipt)
                {
                    var subManager = new SubscriptionManager(subProduct, null);
                    var info = subManager.getSubscriptionInfo();

                    if (info.isSubscribed() == Result.True)
                    {
                        print("We are subscribed");
                        ActivateElitePass();
                    }
                    else
                    {
                        print("Unsubscribed");
                        DeActivateElitePass();
                    }
                }
                else
                {
                    print("Receipt not found !!");
                }
            }
            catch (Exception)
            {
                print("It only works for Google store, app store, amazon store, you are using a fake store!!");
            }
        }
        else
        {
            print("Product not found !!");
        }
    }

    #region error handling
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        print("Initialization failed: " + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        print("Initialization failed: " + error + " - " + message);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        print("Purchase failed: " + failureReason);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        print("Purchase failed: " + failureDescription);
    }
    #endregion

    #region extra 

    [Header("Consumable")]
    public TextMeshProUGUI gemTxt;
    void AddGems(int num)
    {
        saveSystem.AddGem(num);
        shopUIManager.UpdateUI();
    }
    float val;


    /* [Header("Non Consumable")]
    public GameObject AdsPurchasedWindow;
    public GameObject adsBanner;
    void RemoveAds()
    {
        DisplayAds(false);
    }
    void ShowAds()
    {
        DisplayAds(true);
    }
    void DisplayAds(bool x)
    {
        if (!x)
        {
            AdsPurchasedWindow.SetActive(true);
            adsBanner.SetActive(false);
        }
        else
        {
            AdsPurchasedWindow.SetActive(false);
            adsBanner.SetActive(true);
        }
    } */

    [Header("Subscription")]
    public GameObject subActivatedWindow;
    public GameObject premiumBanner;

    void ActivateElitePass()
    {
        setupElitePass(true);
    }
    void DeActivateElitePass()
    {
        setupElitePass(false);
    }
    void setupElitePass(bool x)
    {
        if (x)// active
        {
            subActivatedWindow.SetActive(true);
            premiumBanner.SetActive(true);
        }
        else
        {
            subActivatedWindow.SetActive(false);
            premiumBanner.SetActive(false);
        }
    }

    #endregion
}

[Serializable]
public class SkuDetails
{
    public string productId;
    public string type;
    public string title;
    public string name;
    public string iconUrl;
    public string description;
    public string price;
    public long price_amount_micros;
    public string price_currency_code;
    public string skuDetailsToken;
}

[Serializable]
public class PayloadData
{
    public string orderId;
    public string packageName;
    public string productId;
    public long purchaseTime;
    public int purchaseState;
    public string purchaseToken;
    public int quantity;
    public bool acknowledged;
}

[Serializable]
public class Payload
{
    public string json;
    public string signature;
    public List<SkuDetails> skuDetails;
    public PayloadData payloadData;
}

[Serializable]
public class Data
{
    public string Payload;
    public string Store;
    public string TransactionID;
}
