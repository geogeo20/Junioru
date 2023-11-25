using RedCock.Gameplay;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReedemPopup : Popup
{
    [SerializeField] private TMP_Text priceLabel;

    private int price;

    private void Start()
    {
        GameEventsManager.Instance.AddListener(Constants.TOGGLE_REEDEM_POPUP, TogglePopup);
        price = GameManager.Instance.GameSettings.CoffeReedemPrice;
        priceLabel.text = price.ToString();
    }

    public void OnCloseButtonPressed()
    {
        GameEventsManager.Instance.PostEvent(Constants.BUTTON_PRESSED_EVENT);
        TogglePopup(false);
    }

    public void OnReedemButtonPressed()
    {
        if (price <= GameManager.Instance.CurrentGameProgress.Coins)
        {
            GameEventsManager.Instance.PostEvent(Constants.REWARD_RECEIVED_EVENT, -price);
        }
        GameEventsManager.Instance.PostEvent(Constants.BUTTON_PRESSED_EVENT);
    }
}