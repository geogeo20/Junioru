using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SettingsPopup : Popup
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [SerializeField] private TMP_Text userIDLabel; 

    private void Start()
    {
        GameEventsManager.Instance.AddListener(Constants.TOGGLE_SETTINGS_POPUP, TogglePopup);
        musicSlider.onValueChanged.AddListener(OnMusicVolumeAdjusted);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeAdjusted);

        musicSlider.value = AudioManager.Instance.MusicVolume;
        sfxSlider.value = AudioManager.Instance.SFXVolume;
        userIDLabel.text = PlayfabManager.Instance.PlayFabId;
    }

    private void OnMusicVolumeAdjusted(float value)
    {
        AudioManager.Instance.UpdateMusicVolume(value);
    }

    private void OnSFXVolumeAdjusted(float value)
    {
        AudioManager.Instance.UpdateSFXVolume(value);
    }

    public void OnCloseButtonPressed()
    {
        GameEventsManager.Instance.PostEvent(Constants.TOGGLE_SETTINGS_POPUP, false);
        GameEventsManager.Instance.PostEvent(Constants.BUTTON_PRESSED_EVENT);
        GameEventsManager.Instance.PostEvent(Constants.USER_SETTINGS_UPDATED);
    }
}