using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RedCock.Utils;
using System;
using System.Threading.Tasks;
using RedCock.Gameplay;

public class AudioManager : SingletonBehaviour<AudioManager>, IManager
{
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;

    [SerializeField] private AudioClip backgroundMusicClip;

    [SerializeField] private AudioClip buttonPressedClip;
    [SerializeField] private AudioClip itemGeneratedClip;
    [SerializeField] private AudioClip itemCombinedClip;

    public float MusicVolume { get; private set; }
    public float SFXVolume { get; private set; }

    public string Name => "Audio Manager";

    public Action OnInitialized { get; set; }

    public Task Init()
    {
        GameEventsManager.Instance.AddListener(Constants.ITEM_COMBINED_EVENT, PlayItemCombined);
        GameEventsManager.Instance.AddListener(Constants.BUTTON_PRESSED_EVENT, PlayButtonPressed);
        GameEventsManager.Instance.AddListener(Constants.ITEM_GENERATED_EVENT, PlayItemGenerated);

        MusicVolume = GameManager.Instance.CurrentGameProgress.MusicVolume;
        SFXVolume = GameManager.Instance.CurrentGameProgress.SFXVolume;
        PlayMusic();
        return null;
    }

    public void UpdateMusicVolume(float value)
    {
        MusicVolume = value;
        musicAudioSource.volume = value;
    }

    public void UpdateSFXVolume(float value)
    {
        SFXVolume = value;
        sfxAudioSource.volume = value;
    }

    private void PlayMusic()
    {
        musicAudioSource.clip = backgroundMusicClip;
        musicAudioSource.Play();
    }

    private void PlayButtonPressed(object data = null)
    {
        sfxAudioSource.clip = buttonPressedClip;
        sfxAudioSource.Play();
    }

    private void PlayItemGenerated(object data = null)
    {
        sfxAudioSource.clip = itemGeneratedClip;
        sfxAudioSource.Play();
    }

    private void PlayItemCombined(object data = null)
    {
        sfxAudioSource.clip = itemCombinedClip;
        sfxAudioSource.Play();
    }

}