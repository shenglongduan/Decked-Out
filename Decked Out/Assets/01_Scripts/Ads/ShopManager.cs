using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Purchasing;



public class ShopManager : MonoBehaviour, IStoreListener
{
    IStoreController m_storeController;

    public ConsumableItem cItem;

    [SerializeField]

    public class ConsumableItem
    {
        public string Name;
        public string ID;
        public string Desc;
        public float Price;
    }

    void Start()
    {
        int gems = PlayerPrefs.GetInt("");
        //gemText.text = CombineInstance.ToString();
        SetupBuilder();
    }

    void SetupBuilder()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(cItem.ID, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        print("Success");
        m_storeController = controller; 
    }




    public void IAP_Gem100Pressed()
    {
        //AddGems(100);
        m_storeController.InitiatePurchase(cItem.ID);
    }

    // ----- Processing Purchase ----- //

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        //Retrive the purchased product
        var product = purchaseEvent.purchasedProduct;

        print("Purchase Complete" + product.definition.id);

        if (product.definition.id==cItem.ID)//consumable item is pressed
        {
            //AddGems(100);
        }

        return PurchaseProcessingResult.Complete;
    }






    public void OnInitializeFailed(InitializationFailureReason error)
    {
        print("initialize failed" + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        print("initialize failed" + error + message);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        print("purchase failed");
    }

    
}
