using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RedCock.Gameplay;
using TMPro;

public class CharacterController : MonoBehaviour
{
    public float CurrentHunger
    {
        get { return currentHunger; }
        set
        {
            currentHunger = value >= 0 ? value : 0;
            eventsService.PostEvent("hungerValueUpdated", currentHunger);
            hunger.text = string.Format("Hunger {0}", currentHunger);
        }
    }

    private float CurrentThirst
    {
        get { return currentThirst; }
        set
        {
            currentThirst = value >= 0 ? value : 0;
            eventsService.PostEvent("thirstValueUpdated", currentThirst);
            thirst.text = string.Format("Thirst {0}", currentThirst);
        }
    }

    private float CurrentHappiness
    {
        get { return currentHappineess; }
        set
        {
            currentHappineess = value >= 0 ? value : 0;
            eventsService.PostEvent("happinessValueUpdated", currentHappineess);
            happiness.text = string.Format("Happiness {0}", currentHappineess);
        }
    }

    private float CurrentEnergy
    {
        get { return currentEnergy; }
        set
        {
            currentEnergy = value >= 0 ? value : 0;
            eventsService.PostEvent("energyValueUpdated", currentEnergy);
            energy.text = string.Format("Energy {0}", currentEnergy);
        }
    }

    private float currentHunger;
    private float currentThirst;
    private float currentHappineess;
    private float currentEnergy;

    private float hungerDecreaseRate;
    private float thirstDecreaseRate;
    private float happinessDecreaseRate;
    private float energyDecreseRate;

    private float currentTime = 60;
    private bool timerEnabled = false;
    private GameEventsManager eventsService = null;

    [SerializeField] private TMP_Text hunger;    
    [SerializeField] private TMP_Text thirst;    
    [SerializeField] private TMP_Text happiness;    
    [SerializeField] private TMP_Text energy;

    public void Initialize(float hunger, float thirst, float happines, float energy)
    {
        eventsService = GameEventsManager.Instance;

        CurrentHunger = hunger;
        CurrentThirst = thirst;
        CurrentHappiness = happines;
        CurrentEnergy = energy;

        SetDecreaseRates();
        StartTimer();
    }

    private void SetDecreaseRates()
    {
        var settings = GameManager.Instance.GameSettings;

        hungerDecreaseRate = settings.HungerLossRate;
        thirstDecreaseRate = settings.ThirstLossRate;
        happinessDecreaseRate = settings.HappinessLossRate;
        energyDecreseRate = settings.EnergyLossRate;
    }

    private void Update()
    {
        if (timerEnabled)
        {
            UpdateTimer();
        }
    }

    private void StartTimer()
    {
        currentTime = 1;
        timerEnabled = true;
    }

    private void UpdateTimer()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            currentTime = 1;
            UpdateStats();
            Debug.Log("One second has passed");
        }
    }

    private void UpdateStats()
    {
        CurrentHunger -= hungerDecreaseRate;
        CurrentThirst -= thirstDecreaseRate;
        CurrentHappiness -= happinessDecreaseRate;
        CurrentEnergy -= energyDecreseRate;
    }
}