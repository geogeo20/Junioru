using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIStats : MonoBehaviour
{
    public TextMeshProUGUI hunger;
    public TextMeshProUGUI thirst;
    public TextMeshProUGUI happiness;
    public TextMeshProUGUI energy;

    private GameEventsManager eventService;

    private void Start()
    {
        eventService = GameEventsManager.Instance;

        eventService.AddListener("hungerValueUpdated", UpdateHungerValue);
        eventService.AddListener("thirstValueUpdated", UpdateThirstValue);
        eventService.AddListener("happinessValueUpdated", UpdateHappinessValue);
        eventService.AddListener("energyValueUpdated", UpdateEnergyValue);
    }

    private void UpdateHungerValue(object data)
    {
        if (data is float value)
        {
            hunger.text = "Current hunger: " + value;
        }
    }
    private void UpdateThirstValue(object data)
    {
        if (data is float value)
        {
            thirst.text = "Current thirst: " + value;
        }
    }
    private void UpdateHappinessValue(object data)
    {
        if (data is float value)
        {
            happiness.text = "Current happiness: " + value;
        }
    }
    private void UpdateEnergyValue(object data)
    {
        if (data is float value)
        {
            energy.text = "Current ebergy: " + value;
        }
    }
}