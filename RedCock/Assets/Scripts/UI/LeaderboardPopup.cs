using PlayFab.ClientModels;
using RedCock.Gameplay;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardPopup : Popup
{
    [SerializeField] private LeaderboardItem leaderboardItemReference;
    [SerializeField] private Transform leaderboardHolder;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_Text rankLabel;
    [SerializeField] private TMP_Text scoreLabel;
    [SerializeField] private TMP_InputField messageInputField;

    [SerializeField] private GameObject infoHolder;

    private int nameMinRangeNumber = 1234;
    private int nameMaxRangeNumber = 9999;

    private string userDisplayName;
    private string dailyMessage;

    private bool userInList = false;

    private void Start()
    {
        GameEventsManager.Instance.AddListener(Constants.TOGGLE_LEADERBOARD_POPUP, EnableLeaderboard);
        nameInputField.onEndEdit.AddListener(ChangeName);
        messageInputField.onEndEdit.AddListener(ChangeMessage);
        SetDailyMessage();
    }

    private void SetDailyMessage()
    {
        PlayfabManager.Instance.GetDailyMessage((result) =>
        {
            if (result != null)
            {
                dailyMessage = result;
            }
        });

        PlayfabManager.Instance.GetAnotherUserData((result) =>
        {
            if (!string.IsNullOrEmpty(result))
            {
                messageInputField.text = result.Replace("\"", "");
            }
            else
            {
                messageInputField.text = dailyMessage.Replace("\"", "");
            }
        }, "message", PlayfabManager.Instance.PlayFabId);
    }

    private void SetDefaultUser()
    {
        if (!userInList)
        {
            infoHolder.SetActive(true);
            rankLabel.text = "-";
            userDisplayName = string.Format("Pui{0}", Random.Range(nameMinRangeNumber, nameMaxRangeNumber));
            nameInputField.text = userDisplayName;

            scoreLabel.text = GameManager.Instance.CurrentGameProgress.Coins.ToString();
            
            PlayfabManager.Instance.UpdatedUserDisplayName(userDisplayName);
        }
        else
        {
            infoHolder.SetActive(false);
        }
    }

    public void RefreshLeaderboard()
    {
        PlayfabManager.Instance.GetLeaderboard((result) =>
        {
            if (result != null) InitLeaderboard(result);
        });
    }

    private void InitLeaderboard(List<PlayerLeaderboardEntry> list)
    {
        foreach (Transform item in leaderboardHolder)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in list)
        {
            var leaderboardItem = Instantiate(leaderboardItemReference, leaderboardHolder);
            leaderboardItem.Init(item.Position, item.DisplayName, item.StatValue);
            leaderboardItem.gameObject.SetActive(true);

            if (item.PlayFabId == PlayfabManager.Instance.PlayFabId)
            {
                userInList = true;
                SetCurrentUser(item);
            }
            GetMessage(item.PlayFabId, leaderboardItem);
        }

        SetDefaultUser();
    }

    public void OnCloseButtonPressed()
    {
        GameEventsManager.Instance.PostEvent(Constants.TOGGLE_LEADERBOARD_POPUP, false);
        GameEventsManager.Instance.PostEvent(Constants.BUTTON_PRESSED_EVENT);
    }

    private void EnableLeaderboard(object data)
    {
        TogglePopup((bool)data);
        if ((bool)data) RefreshLeaderboard();
    }

    private void ChangeName(string newValue)
    {
        PlayfabManager.Instance.UpdatedUserDisplayName(newValue);
    }

    private void SetCurrentUser(PlayerLeaderboardEntry player)
    {
        nameInputField.text = player.DisplayName;
        if (string.IsNullOrEmpty(player.DisplayName))
        {
            userDisplayName = string.Format("Pui{0}", Random.Range(nameMinRangeNumber, nameMaxRangeNumber));
            nameInputField.text = userDisplayName;
            PlayfabManager.Instance.UpdatedUserDisplayName(userDisplayName);
        }
        rankLabel.text = string.Format("#{0}", player.Position + 1);
        scoreLabel.text = player.StatValue.ToString();
    }

    private void ChangeMessage(string newValue)
    {
        PlayfabManager.Instance.UploadUserData(newValue, "message");
    }

    private void GetMessage(string playfabId, LeaderboardItem item)
    {
        PlayfabManager.Instance.GetAnotherUserData((msg) =>
        {
            if (msg != null) 
            {
                msg = msg.Replace("\"", "");
                item.SetMessage(msg);
                if (playfabId == PlayfabManager.Instance.PlayFabId)
                {
                    messageInputField.text = msg;
                }
            }
            else
            {
                item.SetMessage(dailyMessage);
            }
        }, "message", playfabId);
    }

}