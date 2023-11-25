using RedCock.Gameplay;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainScreen : Screen
{
    [SerializeField] private Image background;
    [SerializeField] private List<Sprite> backgroundVariants;
    [SerializeField] private GameObject buttonsHolder;
    [SerializeField] private List<CharacterDressSystem> characters;
    [SerializeField] private TMP_Text characterNameLabel;

    private int currentBackgroundIndex;
    private int currentCharacterIndex;
    private CharacterSaveData currentCharacter;
    private List<string> charactersNamesList;

    protected override void Start()
    {
        base.Start();
        Init();
    }

    private void Init()
    {
        ID = 1;

        SaveData data = GameManager.Instance.CurrentGameProgress;

        charactersNamesList = GameManager.Instance.GameSettings.CharactersName;

        currentBackgroundIndex = data.CurrentBackgroundIndex;
        currentCharacterIndex = data.CharacterSaveData.CurrentCharacterIndex;
        background.sprite = backgroundVariants[currentBackgroundIndex];


        if (data.CharacterSaveData == null)
        {
            currentCharacter = new();
        }
        else
        {
            currentCharacter = data.CharacterSaveData;
        }

        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].gameObject.SetActive(i == currentCharacterIndex);
            if (i == currentCharacterIndex) characters[i].Init(currentCharacter);
        }

        characterNameLabel.text = charactersNamesList[currentCharacterIndex];

        GameEventsManager.Instance.AddListener(Constants.TOGGLE_EDIT_MODE_EVENT, ToogleEditMode);
    }

    private void ToogleEditMode(object data)
    {
        if (data is not bool) return;
        buttonsHolder.SetActive((bool)data);
    }

    private void SetBackground(int increment)
    {
        currentBackgroundIndex += increment;
        if (currentBackgroundIndex < 0) currentBackgroundIndex = 2;
        else currentBackgroundIndex %= 3;
        background.sprite = backgroundVariants[currentBackgroundIndex];

        GameEventsManager.Instance.PostEvent(Constants.BACKGROUND_UPDATED_EVENT, currentBackgroundIndex);
    }

    public void NextBackground(int increment)
    {
        SetBackground(increment);
    }

    public void SetCharacter(int increment)
    {
        currentCharacterIndex += increment;
        if (currentCharacterIndex < 0) currentCharacterIndex = 2;
        else currentCharacterIndex %= 3;

        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].gameObject.SetActive(i == currentCharacterIndex);
            if (i == currentCharacterIndex) characters[i].Init(new CharacterSaveData(), true);
        }
        characterNameLabel.text = charactersNamesList[currentCharacterIndex];

        GameEventsManager.Instance.PostEvent(Constants.CHARACTER_UPDATED_EVENT, new CharacterSaveData { CurrentCharacterIndex = currentCharacterIndex});
    }

    public void ToggleScreen(bool toggle)
    {
        gameObject.SetActive(toggle);
    }
}