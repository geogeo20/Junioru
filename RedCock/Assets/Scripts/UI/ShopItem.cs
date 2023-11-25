using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private string productID;
    [SerializeField] private int payout;

    [SerializeField] private TMP_Text priceLabel;
    [SerializeField] private TMP_Text ammountLabel;


    private void Start()
    {
        SetShopItem();    
    }

    private void SetShopItem()  
    {
        Product product = CodelessIAPStoreListener.Instance.GetProduct(productID);
        priceLabel.text = product.metadata.localizedPriceString;
        ammountLabel.text = string.Format("-{0}", payout.ToString());
    }

    public void OnPurchaseComplete()
    {
        Debug.Log("Purchase complete");
        GameEventsManager.Instance.PostEvent(Constants.REWARD_RECEIVED_EVENT, payout);
        GameEventsManager.Instance.PostEvent(Constants.PURCHASE_EVENT, true);
    }

    public void OnPurchaseFailed()
    {
        Debug.Log("Purchase failed");
        GameEventsManager.Instance.PostEvent(Constants.PURCHASE_EVENT, false);
    }
}