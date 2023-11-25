using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RedCock.Utils;
using UnityEngine;

public class GameEventsManager : SingletonBehaviour<GameEventsManager>, IManager
{
    public delegate void GameEventListener(object args);
    private readonly Dictionary<string, GameEventListener> eventHandlers = new();

    public string Name => "Game Events Manager";

    public Action OnInitialized { get; set; }

    public void AddListener(string eventId, GameEventListener listner)
    {
        if (!eventHandlers.ContainsKey(eventId))
        {
            eventHandlers.Add(eventId, null);
        }
        eventHandlers[eventId] += listner;
    }

    public void PostEvent(string eventId, object arguments = null)
    {
        if (eventHandlers.ContainsKey(eventId))
        {
            eventHandlers[eventId]?.Invoke(arguments);
#if UNITY_EDITOR
            Debug.Log("Posted event: " + eventId + " " + arguments?.ToString());
#endif
        }
    }

    public void RemoveListner(string eventId, GameEventListener listner)
    {
        if (eventHandlers.ContainsKey(eventId))
        {
            eventHandlers[eventId] -= listner;
        }
    }

    public Task Init()
    {
        return null;
    }
}