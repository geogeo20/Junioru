using RedCock.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDressSystem : MonoBehaviour
{
    [SerializeField] private Image hatElement;
    [SerializeField] private Image torsoElement;
    [SerializeField] private Image pantsElement;
    [SerializeField] private Image accesoryElement;

    [SerializeField] private List<Sprite> hatVariants;
    [SerializeField] private List<Sprite> torsoVariants;
    [SerializeField] private List<Sprite> pantsVariants;
    [SerializeField] private List<Sprite> accesoryVariants;

    [SerializeField] private GameObject buttonsHolder;
    [SerializeField] private int characterIndex;

    private int currentHatIndex;
    private int currentTorsoIndex;
    private int currentPantsIndex;
    private int currentAccesoryIndex;

    private void Start()
    {
        GameEventsManager.Instance.AddListener(Constants.TOGGLE_EDIT_MODE_EVENT, ToogleEditMode);
    }

    public void Init(CharacterSaveData data, bool editMode = false)
    {
        currentHatIndex = data.CuurentHatIndex;
        currentTorsoIndex = data.CurrentTorsoIndex;
        currentPantsIndex = data.CurrentPantsIndex;
        currentAccesoryIndex = data.CurrentAccesoryIndex;
        hatElement.sprite = hatVariants[currentHatIndex];
        torsoElement.sprite = torsoVariants[currentTorsoIndex];
        pantsElement.sprite = pantsVariants[currentPantsIndex];
        accesoryElement.sprite = accesoryVariants[currentAccesoryIndex];

        ToogleEditMode(editMode);
    }

    private void ToogleEditMode(object data)
    {
        if (data is not bool) return;
        buttonsHolder.SetActive((bool)data);
    }

    private void SetElement(int element, int increment = 0)
    {
        switch (element)
        {
            case 0:
                currentHatIndex += increment;
                if (currentHatIndex < 0) currentHatIndex = 2;
                else currentHatIndex %= 3;
                hatElement.sprite = hatVariants[currentHatIndex];
                break;
            case 1:
                currentTorsoIndex += increment;
                if (currentTorsoIndex < 0) currentTorsoIndex = 2;
                else currentTorsoIndex %= 3;
                torsoElement.sprite = torsoVariants[currentTorsoIndex];
                break;
            case 2:
                currentPantsIndex += increment;
                if (currentPantsIndex < 0) currentPantsIndex = 2;
                else currentPantsIndex %= 3;
                pantsElement.sprite = pantsVariants[currentPantsIndex];
                break;
            case 3:
                currentAccesoryIndex += increment;
                if (currentAccesoryIndex < 0) currentAccesoryIndex = 2;
                else currentAccesoryIndex %= 3;
                accesoryElement.sprite = accesoryVariants[currentAccesoryIndex];
                break;
            default:
                break;
        }

        CharacterSaveData saveData = new CharacterSaveData
        {
            CurrentCharacterIndex = characterIndex,
            CuurentHatIndex = currentHatIndex,
            CurrentTorsoIndex = currentTorsoIndex,
            CurrentPantsIndex = currentPantsIndex,
            CurrentAccesoryIndex = currentAccesoryIndex
        };

        GameEventsManager.Instance.PostEvent(Constants.CHARACTER_UPDATED_EVENT, saveData);
        GameEventsManager.Instance.PostEvent(Constants.BUTTON_PRESSED_EVENT);
    }

    public void NextElement(int element)
    {
        SetElement(element, 1);
    }

    public void PreviousElement(int element)
    {
        SetElement(element, -1);
    }
}