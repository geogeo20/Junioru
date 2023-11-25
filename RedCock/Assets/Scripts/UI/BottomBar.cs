using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomBar : MonoBehaviour
{
    [SerializeField] private List<Transform> buttons;

    public float showedValue;
    public float hidenValue;

    private void Start()
    {
        GameEventsManager.Instance.AddListener(Constants.TOGGLE_BOTTOM_BAR_EVENT, ToggleBottomBar);
    }

    public void OnScreenButtonPressed(int id)
    {
        GameEventsManager.Instance.PostEvent(Constants.SCREEN_CHANGED_EVENT, id);
        ToogleButton(id);
    }

    private void ToggleBottomBar(object data)
    {
        if (data is not bool) return;

        gameObject.SetActive((bool)data);
    }

    private void ToogleButton(object data)
    {
        if (data is not int) return;

        int buttonId = (int)data;

        for (int i = 0; i < buttons.Count; i++)
        {
            var pos = buttons[i].localPosition;

            if (buttonId == i)
            {
                pos.y = showedValue;
            }
            else
            {
                pos.y = hidenValue;
            }

            buttons[i].localPosition = pos;
        }

        GameEventsManager.Instance.PostEvent(Constants.SCREEN_CHANGED_EVENT, data);
        GameEventsManager.Instance.PostEvent(Constants.BUTTON_PRESSED_EVENT);
    }
}