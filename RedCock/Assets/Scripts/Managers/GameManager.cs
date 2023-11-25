#if UNITY_ANDROID
using Google.Play.Common;
using Google.Play.Review;
#endif

using Newtonsoft.Json;
using RedCock.Utils;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

#if UNITY_IOS
using UnityEngine.iOS;
#endif

namespace RedCock.Gameplay
{
    public class GameManager : SingletonBehaviour<GameManager>, IManager
    {
        public GameSettings GameSettings;
        public string Name => "Game Manager";

        public Action OnInitialized { get; set; }
        public SaveData CurrentGameProgress { get; private set; }
        public Action<string> OnUserDataRetreived { get; set; }

        private CurrencySystem currencySystem;

#if UNITY_ANDROID
		private ReviewManager reviewManager;
		public PlayReviewInfo playReviewInfo { get; private set; }
#endif

        private bool reviewRequestedThisSession;

        public Task Init()
        {
            LoadLocalUserData();
            GameEventsManager.Instance.AddListener(Constants.USER_PROGRESS_UPDATED, UpdateUserProgress);
            GameEventsManager.Instance.AddListener(Constants.BACKGROUND_UPDATED_EVENT, BackgroundUpdatedEvent);
            GameEventsManager.Instance.AddListener(Constants.CHARACTER_UPDATED_EVENT, CharacterUpdatedEvent);
            GameEventsManager.Instance.AddListener(Constants.USER_SETTINGS_UPDATED, UpdateUserSettings);
            OnUserDataRetreived += ParseCloudUserData;
            return null;
        }

        private void LoadLocalUserData()
        {
            var data = LocalDataSaver.ReadGameData<SaveData>(Constants.SAVE_DATA_PATH);
            if (data == null)
            {
                data = new SaveData();
                data.CharacterSaveData = new();
                data.MusicVolume = 1;
                data.SFXVolume = 1;
            }
            CurrentGameProgress = data;

            if (CurrentGameProgress == null)
            {
                CurrentGameProgress = new();
            }
        }

        private void ParseCloudUserData(string data)
        {
            SaveData saveData = new();
            saveData.CharacterSaveData = new();
            if (data != null)
            {
                saveData = JsonConvert.DeserializeObject<SaveData>(data);
            }

            if (saveData.SaveTimeStamp >= CurrentGameProgress.SaveTimeStamp)
            {
                CurrentGameProgress = saveData;
            }

            currencySystem = new CurrencySystem(CurrentGameProgress.Coins);
            GameEventsManager.Instance.PostEvent(Constants.USER_DATA_LOADED, CurrentGameProgress);
        }

        private void UpdateUserProgress(object data)
        {
            CurrentGameProgress.Coins = currencySystem.Coins;
            PlayfabManager.Instance.UpdateLeaderboard(CurrentGameProgress.Coins);
            SaveData();
        }

        private void UpdateUserSettings(object data)
        {
            CurrentGameProgress.MusicVolume = AudioManager.Instance.MusicVolume;
            CurrentGameProgress.SFXVolume = AudioManager.Instance.SFXVolume;
            SaveData();
        }

        private void BackgroundUpdatedEvent(object data)
        {
            if (data is not int) return;

            CurrentGameProgress.CurrentBackgroundIndex = (int)data;
            SaveData();
        }

        private void CharacterUpdatedEvent(object data)
        {
            if (data is not CharacterSaveData) return;
            CurrentGameProgress.CharacterSaveData = data as CharacterSaveData;

            SaveData();
        }

        private void SaveData()
        {
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            int secondsSinceEpoch = (int)t.TotalSeconds;
            Console.WriteLine(secondsSinceEpoch);

            CurrentGameProgress.SaveTimeStamp = secondsSinceEpoch;
            LocalDataSaver.SaveGameData(CurrentGameProgress, Constants.SAVE_DATA_PATH);
            PlayfabManager.Instance.UploadUserData(CurrentGameProgress, "progress");
        }

        public Sprite GetItemSprite(int grade)
        {
            return GameSettings.GridItems.Find(x => x.ItemGrade == grade).ItemTexture;
        }

        public void RequestReview()
        {
            if (reviewRequestedThisSession) return;
            reviewRequestedThisSession = true;
#if UNITY_ANDROID
            StartCoroutine(RequestAndroidReview());
#elif UNITY_IOS
		RequestIOSReview();
#endif
        }

        private void RequestIOSReview()
        {
#if UNITY_IOS
		Device.RequestStoreReview();
#endif
        }

        private IEnumerator RequestAndroidReview()
        {
            Debug.Log("[Review] Try to show review app popup");
#if UNITY_ANDROID
            var requestFlowOperation = reviewManager.RequestReviewFlow();
            yield return requestFlowOperation;
            if (requestFlowOperation.Error != ReviewErrorCode.NoError)
            {
                // Log error. For example, using requestFlowOperation.Error.ToString().
                Debug.LogError(requestFlowOperation.Error.ToString());
                yield break;
            }

            playReviewInfo = requestFlowOperation.GetResult();

            var launchFlowOperation = reviewManager.LaunchReviewFlow(playReviewInfo);
            yield return launchFlowOperation;
            playReviewInfo = null; // Reset the object
            if (launchFlowOperation.Error != ReviewErrorCode.NoError)
            {
                // Log error. For example, using requestFlowOperation.Error.ToString().
                yield break;
            }
#endif
            yield return null;
        }
    }

    [Serializable]
    public class SaveData
    {
        public long SaveTimeStamp;
        public int Coins;
        public int CurrentBackgroundIndex;
        public CharacterSaveData CharacterSaveData;
        public float MusicVolume;
        public float SFXVolume;
    }

    [Serializable]
    public class CharacterSaveData
    {
        public int CurrentCharacterIndex;
        public int CuurentHatIndex;
        public int CurrentTorsoIndex;
        public int CurrentPantsIndex;
        public int CurrentAccesoryIndex;
    }
}