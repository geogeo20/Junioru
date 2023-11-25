using RedCock.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class UIManager : SingletonBehaviour<UIManager>, IManager
{
    public string Name => "UI Manager";

    public Action OnInitialized { get; set; }

    private List<Screen> screens;

    public Task Init()
    {
        GameEventsManager.Instance.AddListener(Constants.SCREEN_CHANGED_EVENT, ScreenChangeEvent);
        return null;
    }

    private void ScreenChangeEvent(object data)
    {
        if (data is not int) return;

        foreach (var item in screens)
        {
            item.ToggleScreen(item.ID == (int)data);
        }
    }

    public void AddScreen(Screen screen)
    {
        if (screens == null) screens = new();

        screens.Add(screen);
    }
}