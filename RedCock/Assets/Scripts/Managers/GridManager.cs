using RedCock.Gameplay;
using RedCock.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : Screen
{
    [SerializeField] private List<Slot> slots = new List<Slot>();
    [SerializeField] private ItemDrag gridItemPrefab;
    [SerializeField] private Transform itemHolder;
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameSettings settings;
    [SerializeField] private Order order;

    public List<ItemDrag> currentItems;

    private List<int> currentOrderAmmount;
    private int currentOrderReward;

    protected override void Start()
    {
        base.Start();
        ID = 0;
        currentItems = new();
        GameEventsManager.Instance.AddListener(Constants.ITEM_COMBINED_EVENT, ItemCombinedEvent);
        GenerateOrder();
    }

    public float GetCurrentScaleFactor()
    {
        return canvas.scaleFactor;
    }

    public void GenerateGridItem()
    {
        var slotIndex = GetRandomSlot();

        if (slotIndex >= 0)
        {
            var item = Instantiate(gridItemPrefab, itemHolder);

            item.Init(canvas.scaleFactor, this);
            slots[slotIndex].TakeSlot(item);

            currentItems.Add(item);
        }

        ItemCombinedEvent(null);
        GameEventsManager.Instance.PostEvent(Constants.ITEM_GENERATED_EVENT);

    }

    public void RemoveItem(ItemDrag item)
    {
        currentItems.Remove(item);
    }

    private void ItemCombinedEvent(object data)
    {
        int gradeZero = 0;
        int gradeOne = 0;
        int gradeTwo = 0;

        foreach (var item in currentItems)
        {
            switch (item.Grade)
            {
                case 0:
                    gradeZero++;
                    break;
                case 1:
                    gradeOne++;
                    break;
                case 2:
                    gradeTwo++;
                    break;
                default:
                    break;
            }
        }

        if (currentOrderAmmount[0] <= gradeZero &&
            currentOrderAmmount[1] <= gradeOne &&
            currentOrderAmmount[2] <= gradeTwo)
        {
            order.ToogleServeButton(true);
        }
        else
        {
            order.ToogleServeButton(false);
        }

        Debug.Log("Grade zero: " + gradeZero);
        Debug.Log("Grade one: " + gradeOne);
        Debug.Log("Grade two: " + gradeTwo);
    }

    private int GetRandomSlot()
    {
        List<int> freeIndexes = new();

        for (int i = 0; i < slots.Count; i++)
        {
            if (!slots[i].SlotTaken) freeIndexes.Add(i);
        }

        if (freeIndexes.Count == 0)
        {
            return -1;
        }
        else
        {
            int index = Random.Range(0, freeIndexes.Count);
            return freeIndexes[index];
        }
    }

    [ContextMenu("Test oder")]
    private void GenerateOrder()
    {
        Sprite charcterVisual = settings.CharctersOrderingSprites[Random.Range(0, settings.CharctersOrderingSprites.Count)];

        List<Sprite> sprites = new();
        List<int> ammounts = new();

        currentOrderReward = 0;

        foreach (var item in settings.GridItems)
        {
            var itemAmmounts = Random.Range(item.MinInOrder, item.MaxInOrder + 1);
            sprites.Add(item.ItemTexture);
            ammounts.Add(itemAmmounts);
            if (itemAmmounts > 0)
            {
                currentOrderReward += itemAmmounts * item.ItemPrice;
            }
        }

        order.Init(charcterVisual, currentOrderReward, sprites, ammounts, this);
        order.ToogleServeButton(false);

        currentOrderAmmount = ammounts;

    }

    public void ServeOrder()
    {
        foreach (var item in currentItems)
        {
            Destroy(item.gameObject);
        }
        GameEventsManager.Instance.PostEvent(Constants.REWARD_RECEIVED_EVENT, currentOrderReward);
        GameEventsManager.Instance.PostEvent(Constants.ITEM_GENERATED_EVENT);


        GenerateOrder();
        GameManager.Instance.RequestReview();
    }
}