using RedCock.Gameplay;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TopBar : MonoBehaviour
{
    [SerializeField] private GameObject backButtonHolder;
    [SerializeField] private GameObject settingsButtonHolder;
    [SerializeField] private GameObject dressButtonHolder;
    [SerializeField] private GameObject leaderboardsButtonHolder;
    [SerializeField] private GameObject coinsHolder;

    [SerializeField] private TMP_Text coinsAmmountLabel;


    private void Start()
    {
        GameEventsManager.Instance.AddListener(Constants.TOGGLE_EDIT_MODE_EVENT, OnEditModEvent);
        GameEventsManager.Instance.AddListener(Constants.COINS_AMMOUNT_UPDATED, OnCoinsAmmountUpdated);
        GameEventsManager.Instance.AddListener(Constants.SCREEN_CHANGED_EVENT, ScreenChangedEvent);
        Initiliaze();
    }

    private void Initiliaze()
    {
        coinsAmmountLabel.text = GameManager.Instance.CurrentGameProgress.Coins.ToString();
    }

    private void ScreenChangedEvent(object data)
    {
        if (data is not int) return;

        int screenId = (int)data;

        dressButtonHolder.SetActive(screenId == 1);
    }

    public void OnBackButtonPressed()
    {
        GameEventsManager.Instance.PostEvent(Constants.BACK_BUTTON_EVENT);
        GameEventsManager.Instance.PostEvent(Constants.TOGGLE_BOTTOM_BAR_EVENT, true);
        GameEventsManager.Instance.PostEvent(Constants.TOGGLE_EDIT_MODE_EVENT, false);
        GameEventsManager.Instance.PostEvent(Constants.BUTTON_PRESSED_EVENT);

    }

    public void OnLeaderboardButtonPressed()
    {
        GameEventsManager.Instance.PostEvent(Constants.TOGGLE_LEADERBOARD_POPUP, true);
        GameEventsManager.Instance.PostEvent(Constants.BUTTON_PRESSED_EVENT);
    }

    private void OnEditModEvent(object data)
    {
        if (data is not bool) return;

        dressButtonHolder.SetActive(!(bool)data);
        leaderboardsButtonHolder.SetActive(!(bool)data);
        backButtonHolder.SetActive((bool)data);
    }

    private void OnCoinsAmmountUpdated(object data)
    {
        if (data is not int) return;
        coinsAmmountLabel.text = data.ToString();
    }

    public void OnDressButtonPressed()
    {
        GameEventsManager.Instance.PostEvent(Constants.TOGGLE_EDIT_MODE_EVENT, true);
        GameEventsManager.Instance.PostEvent(Constants.TOGGLE_BOTTOM_BAR_EVENT, false);
        GameEventsManager.Instance.PostEvent(Constants.BUTTON_PRESSED_EVENT);
    }

    public void OnSettingsButtonPressed()
    {
        GameEventsManager.Instance.PostEvent(Constants.TOGGLE_SETTINGS_POPUP, true);
        GameEventsManager.Instance.PostEvent(Constants.BUTTON_PRESSED_EVENT);
    }

    public void OnReedemButtonPressed()
    {
        GameEventsManager.Instance.PostEvent(Constants.TOGGLE_REEDEM_POPUP, true);
        GameEventsManager.Instance.PostEvent(Constants.BUTTON_PRESSED_EVENT);
    }
}