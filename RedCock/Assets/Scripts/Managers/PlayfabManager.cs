using UnityEngine;
using PlayFab;
using RedCock.Utils;
using System;
using PlayFab.ClientModels;
using RedCock.Gameplay;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

public class PlayfabManager : SingletonBehaviour<PlayfabManager>, IManager
{
    public string Name => "Playfab Manager";
    public Action OnInitialized { get; set; }

    public AuthenticationStatus AuthenticationStatus { get; private set; } = AuthenticationStatus.Pending;

    public string PlayFabId { get; private set; }

    public Task Init()
    {
        Authenticate();
        return null;
    }

    private void Authenticate()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        LoginWithAndroidID();
#elif UNITY_IOS && !UNITY_EDITOR
        LoginWithIosID();
#elif UNITY_EDITOR
        LoginWithCustomID();
#endif
    }

    public void UploadUserData<T>(T data, string key)
    {
        string dataAsJson = JsonConvert.SerializeObject(data);
        Dictionary<string, string> userData = new Dictionary<string, string>();
        userData.Add(key, dataAsJson);

        UpdateUserDataRequest request = new UpdateUserDataRequest
        {
            Data = userData,
            Permission = UserDataPermission.Public
        };

        PlayFabClientAPI.UpdateUserData(request, (result) =>
        {
            Debug.Log("Updated data succes");
        }, (error) =>
        {
            Debug.Log("Fail to upload data " + error.ErrorMessage);
        });
    }

    public void GetUserData(string key)
    {
        PlayFabClientAPI.GetUserData(new(), (result) =>
        {
            string data = null;
            if (result.Data.ContainsKey(key))
            {
                data = result.Data[key].Value;
            }
            GameManager.Instance.OnUserDataRetreived?.Invoke(data);
            Debug.Log("User data succesfully retreived");
        }, (error) =>
        {
            GameManager.Instance.OnUserDataRetreived?.Invoke(null);
            Debug.Log("Error retrieveing user data " + error.ErrorMessage);
        });
    }

    public void GetAnotherUserData(Action<string> onDataRetreived, string key, string playfabID)
    {
        GetUserDataRequest request = new GetUserDataRequest
        {
            Keys = new List<string>() { key },
            PlayFabId = playfabID
        };

        PlayFabClientAPI.GetUserData(request, (result) =>
        {
            string data = null;
            if (result.Data.ContainsKey(key))
            {
                data = result.Data[key].Value;
            }

            onDataRetreived?.Invoke(data);
            Debug.Log("User data succesfully retreived");
        }, (error) =>
        {
            onDataRetreived?.Invoke(null);
            Debug.Log("Error retrieveing user data " + error.ErrorMessage);
        });
    }

    public void UpdateLeaderboard(int ammount)
    {
        UpdatePlayerStatisticsRequest request = new()
        {
            Statistics = new List<StatisticUpdate>() { new StatisticUpdate { StatisticName = "Coins", Value = ammount } }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, (result) =>
        {
            Debug.Log("Updated succes");
        }, (error) =>
        {
            Debug.Log("Failed to update" + error.ErrorMessage);
        });
    }

    public void GetLeaderboard(Action<List<PlayerLeaderboardEntry>> onComplete)
    {
        GetLeaderboardRequest request = new GetLeaderboardRequest()
        {
            StatisticName = "Coins"
        };

        PlayFabClientAPI.GetLeaderboard(request, (result) =>
        {
            onComplete?.Invoke(result.Leaderboard);
            Debug.Log("Leaderboard retrieved with succes");

        }, (error) =>
        {
            onComplete?.Invoke(null);
            Debug.Log("Fail to get leaderboard " + error.ErrorMessage);
        });
    }

    public void UpdatedUserDisplayName(string name)
    {
        UpdateUserTitleDisplayNameRequest request = new()
        {
            DisplayName = name
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request, (result) =>
        {
            Debug.Log("Name update succes");
        }, (error) =>
        {
            Debug.Log("Name update failed");
        });
    }

    public void GetDailyMessage(Action<string> onCompleted)
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), (result) =>
        {
            if (result.Data == null || !result.Data.ContainsKey("dailyMessage"))
            {
                Debug.Log("Error getting daily message");
                onCompleted?.Invoke(null);
            }
            else
            {
                Debug.Log("Daily message is " + result.Data["dailyMessage"]);
                onCompleted?.Invoke(result.Data["dailyMessage"]);
            }
        }, (error) =>
        {
            Debug.Log("Error getting daily message");
            onCompleted?.Invoke(null);
        });
    }

    private void LoginWithCustomID()
    {
        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest
        {
            CreateAccount = true,
            TitleId = PlayFabSettings.TitleId,
            CustomId = SystemInfo.deviceUniqueIdentifier
        };
        PlayFabClientAPI.LoginWithCustomID(request, (result) =>
        {
            PlayFabId = result.PlayFabId;
            AuthenticationStatus = AuthenticationStatus.Succes;
            GetUserData("progress");
            Debug.Log("Authenticated with success");
        }, (error) =>
        {
            GameManager.Instance.OnUserDataRetreived?.Invoke(null);
            AuthenticationStatus = AuthenticationStatus.Failed;
            Debug.Log("Could not authenticate");
        });
    }

#if UNITY_IOS //&& !UNITY_EDITOR
    private void LoginWithIosID()
    {
        LoginWithIOSDeviceIDRequest request = new LoginWithIOSDeviceIDRequest
        {
            CreateAccount = true,
            TitleId = PlayFabSettings.TitleId,
            DeviceId = SystemInfo.deviceUniqueIdentifier
        };
        PlayFabClientAPI.LoginWithIOSDeviceID(request, (result) =>
        {
            PlayFabId = result.PlayFabId;
            AuthenticationStatus = AuthenticationStatus.Succes;
            GetUserData("progress");
            Debug.Log("Authenticated with success");
        }, (error) =>
        {
            GameManager.Instance.OnUserDataRetreived?.Invoke(null);
            AuthenticationStatus = AuthenticationStatus.Failed;
            Debug.Log("Could not authenticate");
        });
    }
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
    private void LoginWithAndroidID()
    {
        LoginWithAndroidDeviceIDRequest request = new LoginWithAndroidDeviceIDRequest
        {
            CreateAccount = true,
            TitleId = PlayFabSettings.TitleId,
            AndroidDeviceId = SystemInfo.deviceUniqueIdentifier
        };

        PlayFabClientAPI.LoginWithAndroidDeviceID(request, (result) =>
        {
            PlayFabId = result.PlayFabId;
            AuthenticationStatus = AuthenticationStatus.Succes;
            GetUserData("progress");
            Debug.Log("Authenticated with success");
        }, (error) =>
        {
            GameManager.Instance.OnUserDataRetreived?.Invoke(null);
            AuthenticationStatus = AuthenticationStatus.Failed;
            Debug.Log("Could not authenticate");
        });
    }
#endif
}

public enum AuthenticationStatus
{
    Pending,
    Failed,
    Succes
}