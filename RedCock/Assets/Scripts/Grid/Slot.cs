using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    public bool SlotTaken { get; private set; } = false;
    private ItemDrag currentItem;

    public string currentItemName;

    public void OnDrop(PointerEventData eventData)
    {
        ItemDrag item = eventData.pointerDrag.GetComponent<ItemDrag>();

        if (!SlotTaken)
        {
            TakeSlot(item);
        }
        else
        {
            item.ResetPosition();
        }
    }
    public void TakeSlot(ItemDrag item)
    {
        SlotTaken = true;
        currentItem = item;
        currentItem.SnapToSlot(GetComponent<RectTransform>().anchoredPosition, this);
    }

    public void ClearSlot()
    {
        SlotTaken = false;
        currentItem = null;
    }

    public class ItemGrade
    {
        public int grade;
        public Sprite gradeSprite;
    }
}