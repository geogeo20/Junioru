using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Settings", order = 1)]
public class GameSettings : ScriptableObject
{
    [Header("Starting character stats values")]
    public int Hunger;
    public int Thirst;
    public int Happiness;
    public int Energy;

    [Header("Starting currency ammount")]
    public int Coins;
    public int Gold;

    [Header("Stats loss rate")]
    public int HungerLossRate;
    public int HappinessLossRate;
    public int ThirstLossRate;
    public int EnergyLossRate;

    public int CoffeReedemPrice;

    [Header("Grid Game configuration")]

    public List<GridItemConfig> GridItems;
    public List<Sprite> CharctersOrderingSprites;

    public List<string> CharactersName;
}