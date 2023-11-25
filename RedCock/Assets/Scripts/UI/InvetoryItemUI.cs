using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvetoryItemUI : MonoBehaviour
{
    [SerializeField] private Image targetImage;

    private Sprite currentSprite;
    private string itemId;

    public void Initiliaze(string id, Sprite sprite)
    {
        currentSprite = sprite;
        targetImage.sprite = currentSprite;
        itemId = id;
    }

    public void UpdateVisual(Sprite newSprite)
    {
        currentSprite = newSprite;
        targetImage.sprite = currentSprite;
    }
}