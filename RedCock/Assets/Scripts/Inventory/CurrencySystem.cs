using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencySystem
{
    public int Coins
    {
        get
        {
            return coins;
        }
        set
        {
            coins = value;
            GameEventsManager.Instance.PostEvent(Constants.COINS_AMMOUNT_UPDATED, coins);
        }
    }

    private int coins;

    public CurrencySystem (int coins)
    {
        Initialize(coins);
    }

    private void Initialize(int coins)
    {
        Coins = coins;
        GameEventsManager.Instance.AddListener(Constants.REWARD_RECEIVED_EVENT, OnRewardReceived);
    }
    
    private void OnRewardReceived(object data)
    {
        if (data is not int) return;

        Coins += (int)data;

        GameEventsManager.Instance.PostEvent(Constants.USER_PROGRESS_UPDATED);
    }
}