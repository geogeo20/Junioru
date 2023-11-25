using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PurchaseResultPopup : Popup
{
    [SerializeField] private TMP_Text resultMessage;


    private void Start()
    {
        GameEventsManager.Instance.AddListener(Constants.PURCHASE_EVENT, OnPurchaseEvent);
    }

    private void OnPurchaseEvent(object data)
    {
        if (data is not bool) return;

        var result = (bool)data;

        if (result)
        {
            resultMessage.text = "Purchase complete";
        }
        else
        {
            resultMessage.text = "Purchase failed try again later";
        }

        TogglePopup(true);

    }

    public void OnButtonPressed()
    {
        GameEventsManager.Instance.PostEvent(Constants.BUTTON_PRESSED_EVENT);
        TogglePopup(false);
    }
}